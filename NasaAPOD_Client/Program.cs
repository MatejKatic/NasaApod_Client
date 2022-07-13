using System;
using Apod;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Xml;
using System.Net;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace NasaAPOD_Client
{
        public class Program
        {
            public static async Task Main()
            {

                using var client = new ApodClient("(Your API KEY)");
                using var httpClient = new HttpClient();
            var response = await client.FetchApodAsync();
            if (response.StatusCode != ApodStatusCode.OK)
                {
                    Console.WriteLine(response.Error.ErrorCode);
                    Console.WriteLine(response.Error.ErrorMessage);
                    return;
                }
            ApodModel apodModel = new ApodModel(response.Content.Copyright, response.Content.Date.Date, response.Content.Explanation,
                response.Content.ContentUrlHD, response.Content.MediaType, response.Content.ServiceVersion,
                response.Content.Title, response.Content.ContentUrl);

            try
            {
                XDocument doc = new XDocument(new XElement("APODs",
                                      new XElement("API",
                                        new XElement("copyright", apodModel.copyright),
                                        new XElement("date", apodModel.date.ToString("yyyy-MM-dd")),
                                        new XElement("explanation", apodModel.explanation),
                                        new XElement("hdurl", apodModel.hdurl),
                                        new XElement("media_type", apodModel.media_type),
                                        new XElement("service_version", apodModel.service_version),
                                        new XElement("title", apodModel.title),
                                        new XElement("url", apodModel.url))));
                doc.Save(Directory.GetCurrentDirectory() + "//NasaApod.xml");

                Console.WriteLine("XML is created");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }


            var startDate = GetValidDate("Enter a date between 1995-06-16 and today's date in an yyyy-MM-dd format.");
            var endDate = GetValidDate("Enter another date between the first date and today's date in an yyyy-MM-dd format.");

            Console.WriteLine("Fetching APOD data..");
            var Dateresponse = await client.FetchApodAsync(startDate, endDate);

            foreach (var apod in Dateresponse.AllContent)
            {
                if (apod.MediaType != MediaType.Image) { continue; }

                var uri = new Uri(apod.ContentUrl);
                var directoryPath = @"images/";
                var fileName = apod.Date.ToString("yyyy-MM-dd");
                Console.WriteLine($"Downloading image for {fileName}");
                await DownloadImageToFileAsync(uri, directoryPath, fileName, httpClient);
            }

            Console.WriteLine("Download complete!");
        }

        private static async Task DownloadImageToFileAsync(Uri imageUri, string directoryPath, string fileName, HttpClient httpClient)
        {

            var uriWithoutQuery = imageUri.GetLeftPart(UriPartial.Path);
            var fileExtension = Path.GetExtension(uriWithoutQuery);

            var path = Path.Combine(directoryPath, $"{fileName}{fileExtension}");
            Directory.CreateDirectory(directoryPath);

            var imageBytes = await httpClient.GetByteArrayAsync(imageUri);
            await File.WriteAllBytesAsync(path, imageBytes);
        }

        private static DateTime GetValidDate(string prompt)
        {
            Console.WriteLine(prompt);

            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("That is not a valid date. Try again.");
            }

            return date;
        }

    }
   }

    

