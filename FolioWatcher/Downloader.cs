using FolioWatcher.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace FolioWatcher
{
    public class Downloader
    {
        private readonly HttpClient _httpClient;

        public Downloader()
        {
            _httpClient = new HttpClient();
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Running...");

            var settings = LoadSettings();

            var previousProducts = new List<string>(settings.Products);

            var lastChangeToBuyPage = DownloadLastChangeToBuyPage();

            HtmlDocument doc = new HtmlDocument();

            doc.LoadHtml(lastChangeToBuyPage);

            var currentProducts = doc.DocumentNode.Descendants("main")
                .FirstOrDefault(main => main.Attributes["id"]?.Value == "maincontent")?
                .Descendants("a")
                .Where(a => a.Attributes["class"]?.Value == "product-item-link")
                .Select(a => a.InnerHtml.Trim())
                .ToList() ?? new List<string>();

            var newProducts = currentProducts
                .Where(product => !previousProducts.Contains(product))
                .ToList();

            var displayProducts = currentProducts
                .Select(product => newProducts.Contains(product) ? product + " (+)" : product)
                .ToList();

            if (newProducts.Any())
            {
                Console.WriteLine("Sending email...");

                var emailer = new Emailer(settings);

                await emailer.SendEmailAsync(settings.SmtpToEmail, settings.SmtpToName, "Folio Society Last Chance to Buy", string.Join("<br />", displayProducts));

                Console.WriteLine("Sending complete");
            }

            settings.Products = currentProducts;

            SaveSettings(settings);

            Console.WriteLine("Running complete");
        }

        private string DownloadLastChangeToBuyPage()
        {
            Console.WriteLine($"Downloading last chance to buy page...");

            var url = "https://www.foliosociety.com/row/miscellaneous/last-chance-to-buy";

            var responseBody = _httpClient.GetStringAsync(url).Result;

            Console.WriteLine($"Downloading last chance to buy page complete");

            return responseBody;
        }

        private SettingsDto LoadSettings()
        {
            Console.WriteLine("Loading settings...");

            var json = File.ReadAllText("Config/Settings.json");

            var result = JsonConvert.DeserializeObject<SettingsDto>(json) ?? throw new InvalidOperationException();

            Console.WriteLine("Loading settings complete");

            return result;
        }

        private void SaveSettings(SettingsDto settings)
        {
            Console.WriteLine("Saving settings...");

            string json = JsonConvert.SerializeObject(settings);

            File.WriteAllText("Config/Settings.json", json);

            Console.WriteLine("Saving settings complete");
        }
    }
}
