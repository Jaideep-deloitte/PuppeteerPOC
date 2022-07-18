using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PuppeteerSharp;
using System.Reflection.Metadata;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var options = new LaunchOptions
            {
                Headless = true
            };

            Console.WriteLine("Downloading chromium");
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            Console.WriteLine("Navigating google");
            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync("http://www.facebook.com");

                Console.WriteLine("Generating PDF");
                await page.PdfAsync(Path.Combine("C:\\Users\\jaidtanwar\\source\\repos\\FunctionApp1\\FunctionApp1\\", "facebook.pdf"));


                Console.WriteLine("Export completed");


                //Console.ReadLine();
                //if (!args.Any(arg => arg == "auto-exit"))
                //{
                //    Console.ReadLine();
                //}
            }


            string responseMessage = "Downloaded PDF";

            return new OkObjectResult(responseMessage);
        }
    }
}
