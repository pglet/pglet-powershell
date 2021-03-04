using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Pglet.Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await TestApp();
            await TestPage();
        }

        private static async Task TestPage()
        {
            var page = await Pglet.Page("index", noWindow: true);
            string id = await page.Connection.Send("add text value='Hello, C#!'");
            Debug.WriteLine(id);
        }

        private static async Task TestApp()
        {
            await Pglet.App(async (page) =>
            {
                Debug.WriteLine($"Session started: {page.Connection.PipeId}");
                await Task.Delay(30000);
                Debug.WriteLine("Session end");

            }, "index", noWindow: true);
        }
    }
}
