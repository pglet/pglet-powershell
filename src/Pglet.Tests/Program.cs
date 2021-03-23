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
            //TestDiffs();
            //await TestApp();
            await TestControls();
            //await TestPage();
        }

        private static async Task TestControls()
        {
            var page = await Pglet.Page("index", noWindow: true);
            await page.Connection.SendAsync("clean page");

            //page.ThemePrimaryColor = "";
            //page.ThemeTextColor = "";
            //page.ThemeBackgroundColor = "";

            var firstName = new Textbox { Label = "First name" };
            var lastName = new Textbox { Label = "Last name" };
            var notes = new Textbox { Label = "Notes", Multiline = true, Visible = false };

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
                }
            };

            var stack = new Stack
            {
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

            page.Controls.Add(new Stack { });

            // 2nd update
            await page.UpdateAsync();

            await Task.Delay(5000);
            //testBtn.OnClick = null;

            //page.ThemePrimaryColor = "#3ee66d";
            //page.ThemeTextColor = "#edd2b7";
            //page.ThemeBackgroundColor = "#262626";
            //await page.Update();

            // 3rd update
            //await page.Clean();

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }

        private static async Task TestPage()
        {
            var page = await Pglet.Page("index", noWindow: true);
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
            await Pglet.App(async (page) =>
            {
                Console.WriteLine($"Session started: {page.Connection.PipeId}");
                await Task.Delay(30000);
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
