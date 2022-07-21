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
using Aspose.Pdf;
using System.Collections.Generic;

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
                // it works when create screenshot
                await page.SetViewportAsync(new ViewPortOptions { Width = 200, Height = 200});
                //await page.GoToAsync("https://visualstudio.microsoft.com/msdn-platforms/", new NavigationOptions() { WaitUntil = new WaitUntilNavigation[] { WaitUntilNavigation.Networkidle0 }, Timeout = 0 });
                //await page.EvaluateExpressionAsync("document.querySelector('.fusion-column-content')");
                await page.GoToAsync("http://www.google.com");
                //await page.WaitForSelectorAsync("img.lnXdpd");
                //var watchDog = await page.WaitForFunctionAsync(" () => window.innerWidth < 100");
                //await page.SetViewportAsync(new ViewPortOptions { Width = 50, Height = 50 });
                Console.WriteLine("Generating PDF");

                //Generating PDF
                // change path as per your project path
                var path = $"C:\\Users\\jaidtanwar\\OfficeProject\\PuppeteerPOC\\FunctionApp1\\";
                await page.PdfAsync(Path.Combine(path, "facebook.pdf"));
                // it works
                //await page.ScreenshotAsync(Path.Combine(path, "facebook.jpg"));
                Console.WriteLine("Export completed");

                //Convert to PDF to PPT
                //Document pdfDocument = new Document(Path.Combine(path, "facebook.pdf"));
                //PptxSaveOptions pptxOptions = new PptxSaveOptions();
                //// Save output file
                //pdfDocument.Save(Path.Combine(path, "facebook.pptx"), pptxOptions);


            }


            string responseMessage = "Downloaded PDF and PPT";

            return new OkObjectResult(responseMessage);
        }
    }
}
