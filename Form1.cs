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
using System.Net.Http.Headers;
using System.Net;
using System.Diagnostics;

namespace itext_project
{

    public partial class Form1 : Form
    {
        string mtg_collectionEndpoint = "https://api.scryfall.com/cards/collection";
        string mtg_collectionBodyObj;
        JObject mtg_collectionResponseObj;
        //for get request on all sets
        JObject mtg_collectionSetsObj;
        string endpoint = "https://api.scryfall.com/";
        string method_Sets = "sets";
        string method_Cards = "cards/search?q=set:";
        string cardSetcode;
        JObject mtg_collectionCardsObj;


        private async Task getAllSetsJSON()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(endpoint);
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            string url = method_Sets;
            client.Timeout = TimeSpan.FromSeconds(3000);
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                dynamic mtg_setResponse = await response.Content.ReadAsStringAsync();
                dynamic setOutput = JObject.Parse(mtg_setResponse);
                mtg_collectionSetsObj = setOutput;
                JArray set_CollectionArray = (JArray)mtg_collectionSetsObj["data"];

                foreach (JObject set in set_CollectionArray)
                {
                    string friendlyName = set["name"].ToString(); // This is the name we want to show in the combobox
                    string code = set["code"].ToString(); // mtg set code

                    // Create a new SetInfo object and add it to the ComboBox
                    SetInfo setInfo = new SetInfo
                    {
                        FriendlyName = friendlyName,
                        Code = code
                    };
                    cmb_pickSet.Items.Add(setInfo);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task getAllSetCardsJSON()
        {

        }
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

        private async void Form1_Load(object sender, EventArgs e)
        {
            await getAllSetsJSON();
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
                    // lets prompt the user to save this somewhere
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                        saveFileDialog.Title = "Save PDF Document";
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string pdfPath = saveFileDialog.FileName;
                            await cardLoop(pdfPath);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Request body is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //we call this method during PDF Document creation
        private async Task<string> DownloadImageAsync(string imageUrl, string folderPath, string imageName)
        {
            using var httpClient = new HttpClient();
            var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

            // Ensure the directory exists
            Directory.CreateDirectory(folderPath);

            string sanitizedImageName = SanitizeFileName(imageName);

            // Define the path and save the image
            var imagePath = Path.Combine(folderPath, sanitizedImageName + ".jpg");
            await File.WriteAllBytesAsync(imagePath, imageBytes);

            return imagePath;
        }
        private string SanitizeFileName(string fileName)
        {
            // Remove invalid characters from the file name
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c.ToString(), "");
            }
            return fileName;
        }
        private async Task cardLoop(string pdfPath)
        {
            using (var pdfWriter = new PdfWriter(pdfPath))
            using (var pdfDocument = new PdfDocument(pdfWriter))
            using (var document = new Document(pdfDocument))
            {
                int numberOfColumns = 3;
                var table = new Table(numberOfColumns);
                int cardCount = 0;

                JArray card_CollectionArray = (JArray)mtg_collectionResponseObj["data"];
                
                //for the progress bar
                Loading.Maximum = card_CollectionArray.Count; // Set maximum to the number of card objects
                Loading.Value = 0; // Initialize progress bar value

                foreach (JObject card in card_CollectionArray)
                {
                    // Access properties of each card in the "data" array
                    string cardName = card["name"].ToString();
                    string cardId = card["id"].ToString();

                    // Access nested nodes, such as "image_uris"
                    JObject imageUris = (JObject)card["image_uris"];
                    string imageUrlSmall = imageUris["small"].ToString();
                    string imageUrlNormal = imageUris["normal"].ToString();

                    // Access values in the "prices" nested object
                    JObject prices = (JObject)card["prices"];
                    string usdPrice = prices["usd"]?.ToString() ?? "N/A";

                    // Access specific fields in "legalities"
                    JObject legalities = (JObject)card["legalities"];
                    string standardLegality = legalities["standard"].ToString();

                    string folderPath = Path.Combine(Environment.CurrentDirectory, "CollectionName");

                    //for every card object, download the image from url in imageUrlNormal
                    string imagePath = await DownloadImageAsync(imageUrlNormal, folderPath, cardName);

                    //build the stored image for use in the PDF
                    var imageData = ImageDataFactory.Create(imagePath);

                    //Assuming the default resolution of the printed document is 72dpi, we need to generate the card with the following dimensions in pixels
                    iText.Layout.Element.Image image = new Image(imageData).ScaleAbsolute((float)178.56, (float)248.4);

                    var cell = new Cell().Add(image).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(cell);


                    /*.Add(new Paragraph("CardName: " + cardName))
                    .Add(new Paragraph("CardID: " + cardId))
                    .Add(new Paragraph("ImageURL: " + imageUrlSmall))
                    .Add(new Paragraph("Price: " + usdPrice))
                    .Add(new Paragraph("Legality: " + standardLegality))
                    .Add(new Paragraph(""))
                    .Add(new Paragraph(""))
                    .Add(new Paragraph(""))
                    .Add(image);
                    */
                    Loading.Value++;
                }
                document
                        .Add(table);
                MessageBox.Show("PDF saved successfully at " + pdfPath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }


        }

        private async void btn_CreatePDFset_Click(object sender, EventArgs e)
        {
            // Ensure an item is selected
            if (cmb_pickSet.SelectedItem is SetInfo selectedSet) // No prior definition needed
            {
                // Now you can access the properties of the selected SetInfo object
                cardSetcode = selectedSet.Code; // Get the Code property
                string friendlyName = selectedSet.FriendlyName; // Get the FriendlyName property

                var client = new HttpClient();
                client.BaseAddress = new Uri(endpoint);
                client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = method_Cards + cardSetcode;
                client.Timeout = TimeSpan.FromSeconds(3000);
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    dynamic mtg_setResponse = await response.Content.ReadAsStringAsync();
                    dynamic setOutput = JObject.Parse(mtg_setResponse);
                    mtg_collectionResponseObj = setOutput;
                    // lets prompt the user to save this somewhere
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                        saveFileDialog.Title = "Save PDF Document";
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string pdfPath = saveFileDialog.FileName;
                            await cardLoop(pdfPath);
                        }
                    }



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select a set from the list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
    public class SetInfo
    {
        public string FriendlyName { get; set; }
        public string Code { get; set; }

        public override string ToString() => FriendlyName; // Display the FriendlyName in the ComboBox
    }


}