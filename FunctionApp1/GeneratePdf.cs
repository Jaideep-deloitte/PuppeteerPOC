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
//using Aspose.Pdf;
using System.Collections.Generic;
using OpenXmlPowerTools;
using System.Drawing;
using DocumentFormat.OpenXml.Packaging;
using System.Linq;
using System.Drawing.Imaging;

namespace FunctionApp1
{
    public static class GeneratePdf
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string name = req.Query["name"];
                string url = req.Query["url"];

                using var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions
                {
                    Path = Path.GetTempPath()
                });

                var options = new LaunchOptions
                {
                    Headless = true

                };
                
                Console.WriteLine("Downloading chromium");

                await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
                //await browserFetcher.
                byte[] pdf;
                Console.WriteLine("Navigating google");
                using (var browser = await Puppeteer.LaunchAsync(options))
                using (var page = await browser.NewPageAsync())
                {
                    // it works when create screenshot
                    //await page.SetViewportAsync(new ViewPortOptions { Width = 1000 , Height = 200});
                    //await page.GoToAsync("https://visualstudio.microsoft.com/msdn-platforms/", new NavigationOptions() { WaitUntil = new WaitUntilNavigation[] { WaitUntilNavigation.Networkidle0 }, Timeout = 0 });
                    //await page.EvaluateExpressionAsync("document.querySelector('.fusion-column-content')");
                    if (url != null)
                    {
                        await page.GoToAsync(url);
                    }
                    else
                    {
                        await page.GoToAsync("http://www.google.com");
                    }
                    //await page.WaitForSelectorAsync("img.lnXdpd");
                    //var watchDog = await page.WaitForFunctionAsync(" () => window.innerWidth < 100");
                    //await page.SetViewportAsync(new ViewPortOptions { Width = 50, Height = 50 });
                    Console.WriteLine("Generating PDF");

                    //Generating PDF
                    // change path as per your project path
                    //var path = $"C:\\Users\\jaidtanwar\\OfficeProject\\PuppeteerPOC\\FunctionApp1\\";
                    pdf = await page.ScreenshotDataAsync();
                    // it works
                    //await page.ScreenshotAsync(Path.Combine(path, "facebook.jpg"));
                    Console.WriteLine("Export completed");


                    //Convert to PDF to PPT
                    //Document pdfDocument = new Document(Path.Combine(path, "facebook.pdf"));
                    //PptxSaveOptions pptxOptions = new PptxSaveOptions();
                    //// Save output file
                    //pdfDocument.Save(Path.Combine(path, "facebook.pptx"), pptxOptions);

                    //System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(pdf);
                    //Bitmap bitImage = new Bitmap((Bitmap)System.Drawing.Image.FromStream(streamBitmap));

                    //using (PresentationDocument prstDoc = PresentationDocument.Open("D:\\Deploy\\PuppeteerPOC\\FunctionApp1\\Contoso.pptx", true))
                    //{
                    //    string imgId = "rId" + new Random().Next(2000).ToString();
                    //    ImagePart imagePart = prstDoc.PresentationPart.SlideParts.FirstOrDefault().AddImagePart(ImagePartType.Jpeg);
                    //    imagePart.FeedData(new MemoryStream(pdf));
                    //    DocumentFormat.OpenXml.Drawing.Blip blip = prstDoc.PresentationPart.SlideParts.FirstOrDefault().Slide.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().FirstOrDefault();
                    //    //blip.Embed = imgId;
                    //    prstDoc.PresentationPart.SlideParts.FirstOrDefault().Slide.Save();
                    //    prstDoc.PresentationPart.Presentation.Save();
                    //    prstDoc.Close();
                    //}

                    //MemoryStream ms1 = new MemoryStream(pdf);

                    //using (var ms1 = new MemoryStream(pdf))
                    //{
                    //    using (var targetMs = new MemoryStream())
                    //    {
                    //        using (Image images = Image.FromStream(ms1))
                    //        {
                    //            images.Save(@"D:\Deploy\PuppeteerPOC\FunctionApp1\Contoso2.png", ImageFormat.Png);
                    //            //var imageBytes1 = targetMs.ToArray();
                    //            //response = Convert.ToBase64String(imageBytes1);
                    //        }
                    //    }
                    //}

                    //var templatePresentation = "Modified.pptx";
                    //var outputPresentation = "Modified.pptx";
                    //byte[] baPresentation = pdf.ToArray();

                    //var pmlMainPresentation = new PmlDocument(@"D:\Deploy\PuppeteerPOC\FunctionApp1\Contoso.pptx");
                    //OpenXmlMemoryStreamDocument streamDoc = new OpenXmlMemoryStreamDocument(pmlMainPresentation);
                    //PresentationDocument document = streamDoc.GetPresentationDocument();
                    //var thumbNailPart = document.ThumbnailPart;
                    ////ReplaceThumbnail(thumbNailPart, @"C:\Path\to\image\image.jpg");
                    //document.SaveAs("D:\\Deploy\\PuppeteerPOC\\FunctionApp1\\"+outputPresentation);

                }


                string responseMessage = "Downloaded PDF and PPT";
                MemoryStream ms = new MemoryStream(pdf);
                return new FileStreamResult(ms, "image/png")
                {
                    FileDownloadName = (name != null ? name + ".png" : DateTime.Now.ToString())
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                log.LogInformation(e.Message);
                log.LogInformation(e.StackTrace);
                return new BadRequestResult();
            }
        }
    }
}
