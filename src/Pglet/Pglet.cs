﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pglet
{
    public static class Pglet
    {
        public static async Task<Page> Page(string name = null, bool web = false,
            string server = null, string token = null, bool noWindow = false, int ticker = 0)
        {
            var args = ParseArgs("page", name: name, web: web, server: server, token: token, noWindow: noWindow, ticker: ticker);

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = GetPgletPath(),
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();

            string line = null;
            while (!proc.StandardOutput.EndOfStream)
            {
                line = proc.StandardOutput.ReadLine();
            }

            if (proc.ExitCode != 0)
            {
                throw new Exception($"Pglet process exited with code {proc.ExitCode}");
            }

            var match = Regex.Match(line, @"(?<pipe_id>[^\s]+)\s(?<page_url>[^\s]+)");
            var pipeId = match.Groups["pipe_id"].Value;
            var pageUrl = match.Groups["page_url"].Value;

            // create and open connection
            var conn = new Connection(pipeId);
            await conn.Open();

            return new Page(conn, pageUrl);
        }

        public static async Task App(Func<Page, Task> sessionHandler, string name = null, bool web = false,
            string server = null, string token = null, bool noWindow = false, int ticker = 0)
        {
            var args = ParseArgs("app", name: name, web: web, server: server, token: token, noWindow: noWindow, ticker: ticker);

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = GetPgletPath(),
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();

            string line = null;
            string pageUrl = null;
            while (!proc.StandardOutput.EndOfStream)
            {
                line = proc.StandardOutput.ReadLine();
                if (pageUrl == null)
                {
                    pageUrl = line;
                }
                else
                {
                    // create and open connection
                    var conn = new Connection(line);
                    await conn.Open();

                    var page = new Page(conn, pageUrl);
                    var h = sessionHandler(page);
                }
            }
        }

        private static string ParseArgs(string action, string name = null, bool web = false,
            string server = null, string token = null, bool noWindow = false, int ticker = 0)
        {
            var args = new List<string>
            {
                action
            };

            if (!string.IsNullOrEmpty(name))
            {
                args.Add($"\"{name}\"");
            }

            if (web)
            {
                args.Add("--web");
            }

            if (noWindow)
            {
                args.Add("--no-window");
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

            return string.Join(" ", args);
        }

        private static string GetPgletPath()
        {
            if (RuntimeInfo.IsWindows)
            {
                return Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), ".pglet", "bin", "pglet.exe");
            }
            else
            {
                return Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".pglet", "bin", "pglet");
            }
        }
    }
}