using Newtonsoft.Json.Linq;
using Pglet.Controls;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet.Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //TestJson();
            await TestWSClient();
            //TestDiffs();
            //await TestApp();
            //await TestControls();
            //await TestPage();
        }

        private static async Task TestWSClient()
        {
            var cts = new CancellationTokenSource();

            PgletClient2 pgc = new PgletClient2();

            var page = await pgc.ConnectPage("page-1", serverUrl: "http://localhost:3000", cancellationToken: cts.Token);

            //pgc.ServeApp((page) =>
            //{
            //    Console.WriteLine("Session!");
            //    return Task.CompletedTask;
            //}, "app-1", serverUrl: "http://localhost:3000", cancellationToken: cts.Token).Wait();

            //Console.ReadLine();
            await Task.Delay(20000);
        }

        private static void TestJson()
        {
            var msg = new Pglet.Protocol.Message
            {
                Id = "",
                Action = "registerHostClient",
                Payload = new Pglet.Protocol.RegisterHostClientRequestPayload
                {
                    PageName = "test-page",
                    IsApp = true
                }
            };

            var j = JsonUtility.Serialize(msg);
            Console.WriteLine(j);

            var obj = JsonUtility.Deserialize<Pglet.Protocol.Message>(j);
            var payload = JsonUtility.Deserialize<Pglet.Protocol.RegisterHostClientRequestPayload>(obj.Payload as JObject);
        }

        private static async Task TestControls()
        {
            var page = await PgletClient.ConnectPage("index", noWindow: true);
            await page.CleanAsync();

            page.Title = "Example 1";
            page.HorizontalAlign = Align.Start;
            await page.UpdateAsync();

            //page.ThemePrimaryColor = "";
            //page.ThemeTextColor = "";
            //page.ThemeBackgroundColor = "";

            var firstName = new TextBox { Label = "First name" };
            var lastName = new TextBox { Label = "Last name" };
            var notes = new TextBox { Label = "Notes", Multiline = true, Visible = false };

            var vaccinated = new Checkbox
            {
                Label = "Vaccinated",
                OnChange = async (e) =>
                {
                    Console.WriteLine("vaccinated changed: " + e.Data);
                    notes.Visible = (e.Control as Checkbox).Value;
                    await page.UpdateAsync();
                }
            };

            var testBtn = new Button
            {
                Text = "Test!",
                OnClick = async (e) =>
                {
                    Console.WriteLine("clicked!");
                    Console.WriteLine($"First name: {firstName.Value}");
                    Console.WriteLine($"Last name: {lastName.Value}");
                    Console.WriteLine($"Vaccinated: {vaccinated.Value}");
                    Console.WriteLine($"Notes name: {notes.Value}");

                    firstName.Value = "";
                    lastName.Value = "";
                    await page.UpdateAsync();

                    await Task.Delay(5000);

                    Console.WriteLine("done!");
                }
            };

            var stack = new Stack
            {
                Horizontal = true,
                HorizontalAlign = Align.SpaceBetween,
                Controls =
                {
                    new Icon { Name = "Shop", Color = "orange" },
                    new Icon { Name = "DependencyAdd", Color = "green" }
                }
            };

            // 1st render
            await page.AddAsync(
                stack,
                firstName,
                lastName,
                vaccinated,
                notes,
                testBtn);

            stack.Margin = "10";
            stack.Controls.Add(new Icon { Name = "Edit", Color = "red" });
            stack.Controls.RemoveAt(0);

            // button with menu
            page.Controls.Add(new Button
            {
                Split = true,
                Text = "Button with menu",
                OnClick = (e) => { Console.WriteLine("Button click!"); },
                MenuItems =
                {
                    new MenuItem
                    {
                        Text = "Item 1",
                        OnClick = (e) => { Console.WriteLine("Menu item click!"); }
                    },
                    new MenuItem
                    {
                        Text = "Sub menu",
                        SubMenuItems =
                        {
                            new MenuItem
                            {
                                Text = "Sub menu item 1",
                                OnClick = (e) => { Console.WriteLine("Sub menu 1 item click!"); },
                            },
                            new MenuItem
                            {
                                Text = "Sub menu item 2",
                                OnClick = (e) => { Console.WriteLine("Sub menu 2 item click!"); },
                            }
                        }
                    }
                }
            });

            // checkbox
            page.Controls.Add(new Checkbox
            {
                Label = "Check it, check it, check it",
                BoxSide = BoxSide.End,
                Value = true
            });

            // dialog
            var dlg = new Dialog
            {
                Title = "Title 1",
                SubText = "Sub Text",
                Controls =
                {
                    new Text { Value = "Body text" }
                },
                FooterControls = 
                {
                    new Button { Text = "OK" },
                    new Button { Text = "Cancel" }
                }
            };
            page.Controls.Add(dlg);
            page.Controls.Add(new Button
            {
                Text = "Show dialog",
                OnClick = async (e) =>
                {
                    dlg.Open = true;
                    await page.UpdateAsync();
                }
            });

            // 2nd update
            await page.UpdateAsync();

            // BarChart
            stack.Controls.Add(new BarChart
            {
                DataMode = BarChartDataMode.Percentage,
                Points =
                {
                    new BarChartDataPoint { X = 10, Y = 20, Color = "Yellow", Legend = "Disk C:" },
                    new BarChartDataPoint { X = 10, Y = 100, Color = "Green", Legend = "Disk D:" }
                }
            });
            await stack.UpdateAsync();


            var p1 = new Person { FirstName = "John", LastName = "Smith", Age = 30, Employee = true };
            var p2 = new Person { FirstName = "Samantha", LastName = "Fox", Age = 43, Employee = false };
            var p3 = new Person { FirstName = "Alice", LastName = "Brown", Age = 25, Employee = true };

            var grid = new Grid
            {
                PreserveSelection = true,
                SelectionMode = GridSelectionMode.Multiple,
                OnSelect = (e) =>
                {
                    Console.WriteLine(e.Data);
                    foreach(var item in (e.Control as Grid).SelectedItems)
                    {
                        Console.WriteLine(item);
                    }
                },
                Columns =
                {
                    new GridColumn
                    {
                        Name = "Employee",
                        FieldName = "Employee",
                        MaxWidth = 100,
                        TemplateControls =
                        {
                            new Checkbox { ValueField = "Employee" }
                        }
                    },
                    new GridColumn
                    {
                        Name = "First name",
                        FieldName = "FirstName",
                        TemplateControls =
                        {
                            new TextBox { Value = "{FirstName}" }
                        }
                    },
                    new GridColumn { Name = "Last name", FieldName = "LastName" },
                    new GridColumn { Name = "Age", FieldName = "Age" }
                },
                Items =
                {
                    p1, p2, p3
                }
            };

            int n = 1;
            var btnAddRecord = new Button
            {
                Text = "Add record",
                OnClick = async (e) =>
                {
                    grid.Items.RemoveAt(0);
                    grid.Items.Add(new Person
                    {
                        FirstName = $"First {n}",
                        LastName = $"Last {n}",
                        Age = n
                    });
                    await grid.UpdateAsync();
                    n++;
                }
            };

            var btnShowRecords = new Button
            {
                Text = "Show records",
                OnClick = (e) =>
                {
                    (grid.Items[0] as Person).Age = 22;
                    foreach(var p in grid.Items)
                    {
                        Console.WriteLine(p);
                    }
                }
            };

            page.Add(grid, btnAddRecord, btnShowRecords);

            //await Task.Delay(5000);
            //testBtn.OnClick = null;

            //page.ThemePrimaryColor = "#3ee66d";
            //page.ThemeTextColor = "#edd2b7";
            //page.ThemeBackgroundColor = "#262626";
            //await page.UpdateAsync();

            // 3rd update
            //await page.Clean();

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }

        private static async Task TestPage()
        {
            var page = await PgletClient.ConnectPage("index", noWindow: true);
            await page.Connection.SendAsync("clean page");

            int i = 0;
            var result = await page.Connection.SendAsync(@"add
stack horizontal
  button text='-'
  text value='0'
  button text='+'");

            var ids = result.Split(' ');

            string minBtn = ids[1];
            string id = ids[2];
            string plusBtn = ids[3];

            Debug.WriteLine(result);

            while (true)
            {
                var e = page.Connection.WaitEvent();
                Debug.WriteLine(e);
                if (e.Target == plusBtn)
                {
                    i++;
                    await page.Connection.SendAsync($"set {id} value={i}");
                }
                else if (e.Target == minBtn)
                {
                    i--;
                    await page.Connection.SendAsync($"set {id} value={i}");
                }
            }
        }

        private static async Task TestApp()
        {
            await PgletClient.ServeApp(async (page) =>
            {
                page.OnClose = (e) =>
                {
                    Console.WriteLine("Session closed");
                };

                page.OnHashChange = (e) =>
                {
                    Console.WriteLine("Hash changed: " + e.Data);
                };

                Console.WriteLine($"Session started: {page.Connection.PipeId}");
                Console.WriteLine($"Hash: {page.Hash}");

                var txt = new TextBox();
                await page.AddAsync(txt, new Button
                {
                    Text = "Test!",
                    OnClick = async (e) =>
                    {
                        await page.CleanAsync();
                        Console.WriteLine(txt.Value);
                    }
                });

                await Task.Delay(5000);
                Console.WriteLine("Session end");

            }, "index", noWindow: true);
        }

        private static void TestDiffs()
        {
            //var a = "a,b,c,d";
            //var b = "b,d,c,e";
            //TestDiff(a, b);

            //a = "a,b,c,d,e,f,g";
            //b = "e,f,g,x,y,z";
            //TestDiff(a, b);

            //a = "a,b,c,d,e,f,g";
            //b = "a,1,c,2,e,3,g,4";
            //TestTextDiff(a, b);

            var i1 = new int[] { 1, 2, 3, 4, 5, 6 };
            var i2 = new int[] { 1, 7, 3, 8, 5, 9 };
            TestIntDiff(i1, i2);
        }

        private static void TestTextDiff(string a, string b)
        {
            var a1 = a.Replace(',', '\n');
            var b1 = b.Replace(',', '\n');
            var f = Diff.DiffText(a1, b1, false, false, false);

            string[] aLines = a1.Split('\n');
            string[] bLines = b1.Split('\n');

            void WriteLine(int nr, string typ, string aText)
            {
                Console.WriteLine($"{typ}({nr}) - {aText}");
            }

            int n = 0;
            for (int fdx = 0; fdx < f.Length; fdx++)
            {
                Diff.Item aItem = f[fdx];

                // write unchanged lines
                while ((n < aItem.StartB) && (n < bLines.Length))
                {
                    //WriteLine(n, "", bLines[n]);
                    n++;
                } // while

                // write deleted lines
                for (int m = 0; m < aItem.deletedA; m++)
                {
                    WriteLine(n, "delete", aLines[aItem.StartA + m]);
                } // for

                // write inserted lines
                while (n < aItem.StartB + aItem.insertedB)
                {
                    WriteLine(n, "add", bLines[n]);
                    n++;
                } // while
            } // while

            // write rest of unchanged lines
            while (n < bLines.Length)
            {
                WriteLine(n, null, bLines[n]);
                n++;
            } // while
        }

        private static void TestIntDiff(int[] a, int[] b)
        {
            var f = Diff.DiffInt(a, b);

            void WriteLine(int nr, string typ, int num)
            {
                Console.WriteLine($"{typ}({nr}) - {num}");
            }

            int n = 0;
            for (int fdx = 0; fdx < f.Length; fdx++)
            {
                Diff.Item aItem = f[fdx];

                // write unchanged lines
                while ((n < aItem.StartB) && (n < b.Length))
                {
                    //WriteLine(n, "", bLines[n]);
                    n++;
                } // while

                // write deleted lines
                for (int m = 0; m < aItem.deletedA; m++)
                {
                    WriteLine(n, "delete", a[aItem.StartA + m]);
                } // for

                // write inserted lines
                while (n < aItem.StartB + aItem.insertedB)
                {
                    WriteLine(n, "add", b[n]);
                    n++;
                } // while
            } // while

            // write rest of unchanged lines
            while (n < b.Length)
            {
                WriteLine(n, null, b[n]);
                n++;
            } // while
        }
    }
}
