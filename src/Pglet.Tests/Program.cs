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
            //await TestApp();
            await TestPage();
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
                var e = await page.Connection.WaitEvent();
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
                Debug.WriteLine($"Session started: {page.Connection.PipeId}");
                await Task.Delay(30000);
                Debug.WriteLine("Session end");

            }, CancellationToken.None, "index", noWindow: true);
        }
    }
}
