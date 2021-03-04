using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Pglet.Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await TestApp();
        }

        private static async Task TestPage()
        {
            var page = await Pglet.Page("index", noWindow: true);
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
