using Pglet.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pglet.Tests.Fx
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await TestApp();
        }

        private static async Task TestApp()
        {
            await new PgletClient().ServeApp(async (page) =>
            {
                page.OnClose = (e) =>
                {
                    Console.WriteLine("Session closed");
                };

                page.OnHashChange = (e) =>
                {
                    Console.WriteLine("Hash changed: " + e.Data);
                };

                //Console.WriteLine($"Session started: {page.Connection.PipeId}");
                Console.WriteLine($"Hash: {page.Hash}");

                var txt = new TextBox();
                await page.AddAsync(txt, new Button { Text = "Test!", OnClick = (e) =>
                {
                    Console.WriteLine(txt.Value);
                }});

                await Task.Delay(5000);

                //throw new Exception("Error!!!");

                Console.WriteLine("Session end");

            }, "page-aaa", noWindow: true);
        }
    }
}
