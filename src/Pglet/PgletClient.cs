using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet
{
    public static class PgletClient
    {
        public const string PGLET_VERSION = "0.4.4";

        private static string _pgletExe;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public static async Task<Page> ConnectPage(string name = null, bool local = false,
            string server = null, string token = null, bool noWindow = false, bool allEvents = true, int ticker = 0, string permissions = null,
            Func<Connection, string, Page> createPage = null, CancellationToken? cancellationToken = null)
        {
            var ct = cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None;
            var args = ParseArgs("page", name: name, local: local, server: server, token: token, noWindow: noWindow, allEvents: allEvents, ticker: ticker, permissions: permissions);

            string result = ExecuteProcess(await GetPgletPath(), args);

            var match = Regex.Match(result, @"(?<pipe_id>[^\s]+)\s(?<page_url>[^\s]+)");
            if (match.Success)
            {
                var pipeId = match.Groups["pipe_id"].Value;
                var pageUrl = match.Groups["page_url"].Value;

                // create and open connection
                var conn = new Connection(pipeId);
                await conn.OpenAsync(ct);

                Page page;
                if (createPage != null)
                {
                    page = createPage(conn, pageUrl);
                }
                else
                {
                    page = new Page(conn, pageUrl);
                }
                await page.LoadHash();
                return page;
            }
            else
            {
                throw new Exception($"Invalid pglet results: {result}");
            }
        }

        public static async Task ServeApp(Func<Page, Task> sessionHandler, string name = null, bool local = false,
            string server = null, string token = null, bool noWindow = false, bool allEvents = true, int ticker = 0, string permissions = null,
            Func<Connection, string, Page> createPage = null, Action<string> pageCreated = null, CancellationToken ? cancellationToken = null)
        {
            var ct = cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None;
            var args = ParseArgs("app", name: name, local: local, server: server, token: token, noWindow: noWindow, allEvents: allEvents, ticker: ticker, permissions: permissions);

            using (var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = await GetPgletPath(),
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false
                }
            })
            {
                proc.Start();

                using (BlockingCollection<string> bc = new BlockingCollection<string>())
                {
                    string line = null;
                    string pageUrl = null;
                    var t = Task.Run(() =>
                    {
                        while (!proc.StandardOutput.EndOfStream)
                        {
                            line = proc.StandardOutput.ReadLine();
                            if (pageUrl == null)
                            {
                                pageUrl = line;
                                pageCreated?.Invoke(pageUrl);
                            }
                            else
                            {
                                bc.Add(line);
                            }
                        }
                        bc.Add(null);
                    });

                    while (true)
                    {
                        try
                        {
                            line = bc.Take(ct);

                            if (line == null)
                            {
                                return;
                            }

                            // create and open connection
                            var conn = new Connection(line);
                            await conn.OpenAsync(ct);

                            Page page;
                            if (createPage != null)
                            {
                                page = createPage(conn, pageUrl);
                            }
                            else
                            {
                                page = new Page(conn, pageUrl);
                            }
                            await page.LoadHash();
                            var h = sessionHandler(page).ContinueWith(async t =>
                            {
                                if (t.IsFaulted)
                                {
                                    await page.ErrorAsync("There was an error while processing your request: " + (t.Exception as AggregateException).InnerException.Message);
                                }
                            });
                        }
                        catch (OperationCanceledException)
                        {
                            proc.Kill();
                            return;
                        }
                    }
                }
            }
        }

        private static string ParseArgs(string action, string name = null, bool local = false,
            string server = null, string token = null, bool noWindow = false, bool allEvents = false, int ticker = 0, string permissions = null)
        {
            var args = new List<string>
            {
                action
            };

            if (!string.IsNullOrEmpty(name))
            {
                args.Add($"\"{name}\"");
            }

            if (local)
            {
                args.Add("--local");
            }

            if (noWindow)
            {
                args.Add("--no-window");
            }

            if (allEvents)
            {
                args.Add("--all-events");
            }

            if (ticker > 0)
            {
                args.Add("--ticker");
                args.Add(ticker.ToString());
            }

            if (!string.IsNullOrEmpty(server))
            {
                args.Add("--server");
                args.Add(server);
            }

            if (!string.IsNullOrEmpty(token))
            {
                args.Add("--token");
                args.Add($"\"{token}\"");
            }

            if (!string.IsNullOrEmpty(permissions))
            {
                args.Add("--permissions");
                args.Add($"\"{permissions}\"");
            }

            if (RuntimeInfo.IsLinux || RuntimeInfo.IsMac)
            {
                args.Add("--uds");
            }

            return string.Join(" ", args);
        }

        private static async Task<string> GetPgletPath()
        {
            await _semaphore.WaitAsync();

            try
            {
                if (_pgletExe == null)
                {
                    _pgletExe = await Install();
                }
                return _pgletExe;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private static async Task<string> Install()
        {
            var pgletExe = "pglet.exe";
            if (RuntimeInfo.IsLinux || RuntimeInfo.IsMac)
            {
                pgletExe = "pglet";
            }

            // check if pglet.exe is already in PATH
            var paths = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in paths.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, pgletExe);
                if (File.Exists(fullPath))
                {
                    pgletExe = fullPath;
                    return pgletExe;
                }
            }

            var homeDir = RuntimeInfo.IsWindows ? Environment.GetEnvironmentVariable("USERPROFILE") : Environment.GetEnvironmentVariable("HOME");
            var pgletHomeDir = Path.Combine(homeDir, ".pglet");
            var pgletBinDir = Path.Combine(pgletHomeDir, "bin");
            pgletExe = Path.Combine(pgletBinDir, pgletExe);

            var ver = PGLET_VERSION;
            var fileName = $"pglet-{ver}-windows-amd64.zip";
            if (RuntimeInfo.IsLinux)
            {
                fileName = $"pglet-{ver}-linux-amd64.tar.gz";
            }
            else if (RuntimeInfo.IsMac)
            {
                fileName = $"pglet-{ver}-darwin-amd64.tar.gz";
            }

            string installedVer = null;
            try
            {
                installedVer = ExecuteProcess(pgletExe, "--version");
            }
            catch { }

            if (installedVer != null && installedVer == ver)
            {
                // required version is already installed
                return pgletExe;
            }

            Debug.Write($"Installing Pglet v{ver}...");
            var pgletUri = $"https://github.com/pglet/pglet/releases/download/v{ver}/{fileName}";

            var tempDir = RuntimeInfo.IsWindows ? Environment.GetEnvironmentVariable("TEMP") : "/tmp";
            var packagePath = Path.Combine(tempDir, fileName);
            await (new WebClient()).DownloadFileTaskAsync(new Uri(pgletUri), packagePath);

            if (Directory.Exists(pgletBinDir))
            {
                Directory.Delete(pgletBinDir, true);
            }

            Directory.CreateDirectory(pgletBinDir);

            Unpack(packagePath, pgletBinDir);
            File.Delete(packagePath);

            if (RuntimeInfo.IsLinux || RuntimeInfo.IsMac)
            {
                ExecuteProcess("chmod", $"+x {pgletExe}");
            }

            Debug.WriteLine("OK");

            return pgletExe;
        }

        private static void Unpack(string archivePath, string destDirectory)
        {
            if (RuntimeInfo.IsWindows)
            {
                // unpack zip
                ZipFile.ExtractToDirectory(archivePath, destDirectory);
            }
            else
            {
                // unpack tar
                using (Stream stream = File.OpenRead(archivePath))
                using (var reader = ReaderFactory.Open(stream))
                {
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            reader.WriteEntryToDirectory(destDirectory, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
                }
            }
        }

        private static string ExecuteProcess(string fileName, string arguments)
        {
            using (var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false
                }
            })
            {
                proc.Start();

                string line = null;
                while (!proc.StandardOutput.EndOfStream)
                {
                    line = proc.StandardOutput.ReadLine();
                }

                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    throw new Exception($"{fileName} process exited with code {proc.ExitCode}");
                }

                return line;
            }
        }
    }
}
