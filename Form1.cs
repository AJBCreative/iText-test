using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Security.Policy;
using Image = iText.Layout.Element.Image;

namespace itext_project
{
    public partial class Form1 : Form
    {
        string mtg_collectionEndpoint = "https://api.scryfall.com/cards/collection";
        string mtg_collectionBodyObj;
        JObject mtg_collectionResponseObj;
        public Form1()
        {
            InitializeComponent();
        }

        private void collectionJSON()
        {
            JObject getCollection = new JObject();
            JArray identifiersArray = new JArray
            {
                new JObject { { "id", "683a5707-cddb-494d-9b41-51b4584ded69" } },
                new JObject { { "name", "Ancient Tomb" } },
                new JObject
                {
                    { "set", "mrd" },
                    { "collector_number", "150" }
                }
            };
            getCollection.Add("identifiers", identifiersArray);
            mtg_collectionBodyObj = getCollection.ToString();
        }

            private void lbl_pdfInput_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_pdfGeneration_Click(object sender, EventArgs e)
        {
            using var document = new Document(new PdfDocument(new PdfWriter("helloworld-pdf.pdf")));
            document.Add(new Paragraph(txt_pdfInputs.Text.ToString()));
        }

        private async void btn_mtgCollectionAPI_Click(object sender, EventArgs e)
        {
            collectionJSON();
            var client = new HttpClient();
            client.BaseAddress = new Uri(mtg_collectionEndpoint);
            client.DefaultRequestHeaders.Add("User-Agent", "C# custom application");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(30);
            try
            {
                if (mtg_collectionBodyObj != "")
                {
                    var postContent = new StringContent(
                        mtg_collectionBodyObj,
                        System.Text.Encoding.UTF8,
                        "application/json"
                        );
                    HttpResponseMessage response = await client.PostAsync(mtg_collectionEndpoint, postContent);
                    response.EnsureSuccessStatusCode();
                    dynamic str_mtgCollectionJSON = await response.Content.ReadAsStringAsync();
                    dynamic output = JObject.Parse(str_mtgCollectionJSON);
                    mtg_collectionResponseObj = output;
                    await cardLoop();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async Task<string> DownloadImageAsync(string imageUrl, string folderPath, string imageName)
        {
            using var httpClient = new HttpClient();
            var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

            // Ensure the directory exists
            Directory.CreateDirectory(folderPath);

            // Define the path and save the image
            var imagePath = Path.Combine(folderPath, imageName + ".jpg");
            await File.WriteAllBytesAsync(imagePath, imageBytes);

            return imagePath;
        }
        private async Task cardLoop()
        {
            using (var pdfWriter = new PdfWriter("mtg_collection.pdf"))
            using (var pdfDocument = new PdfDocument(pdfWriter))
            using (var document = new Document(pdfDocument))
            {
                JArray card_CollectionArray = (JArray)mtg_collectionResponseObj["data"];
                foreach (JObject card in card_CollectionArray)
                {
                    // Access properties of each card in the "data" array
                    string cardName = card["name"].ToString();
                    string cardId = card["id"].ToString();

                    // Example 3: Access nested nodes, such as "image_uris"
                    JObject imageUris = (JObject)card["image_uris"];
                    string imageUrlSmall = imageUris["small"].ToString();
                    string imageUrlNormal = imageUris["normal"].ToString();

                    // Example 4: Access values in the "prices" nested object
                    JObject prices = (JObject)card["prices"];
                    string usdPrice = prices["usd"]?.ToString() ?? "N/A";

                    // Example 5: Access specific fields in "legalities"
                    JObject legalities = (JObject)card["legalities"];
                    string standardLegality = legalities["standard"].ToString();

                    string folderPath = Path.Combine(Environment.CurrentDirectory, "CollectionName");
                    string imagePath = await DownloadImageAsync(imageUrlNormal, folderPath, cardName);
                    var imageData = ImageDataFactory.Create(imagePath);
                    //Assuming the default resolution of the printed document is 72dpi, we need to generate the card with the following dimensions in pixels
                    iText.Layout.Element.Image image = new Image(imageData).ScaleAbsolute((float)178.56, (float)248.4);
                    var pdfImage = new iText.Layout.Element.Image(imageData);



                    document
                        .Add(new Paragraph("CardName: " + cardName))
                        .Add(new Paragraph("CardID: " + cardId))
                        .Add(new Paragraph("ImageURL: " + imageUrlNormal))
                        .Add(new Paragraph("Price: " + usdPrice))
                        .Add(new Paragraph("Legality: " + standardLegality))
                        .Add(new Paragraph(""))
                        .Add(new Paragraph(""))
                        .Add(new Paragraph(""))
                        .Add(image);
                }
            }
            
            
        }
    }
}