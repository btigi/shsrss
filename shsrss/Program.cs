using Microsoft.Extensions.Configuration;
using shsrss.Model;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var config = builder.Build();

var feedUrl = config["FeedUrl"];
var destinationFolder = config["DestinationFolder"];

if (string.IsNullOrEmpty(feedUrl) || string.IsNullOrEmpty(destinationFolder))
{
    Console.WriteLine("Missing settings - exiting");
    return;
}

Console.WriteLine("Attempting to download file");
var fileResult = await GeFile(feedUrl);

if (fileResult.success)
{
    Console.WriteLine("Attempting to write main file");
    File.WriteAllText("index.xml", fileResult.result);
    Console.WriteLine("Attempting to upload main file");
    File.Copy("index.xml", Path.Combine(destinationFolder, "index.xml"), true);
    Console.WriteLine("Copying of main file complete");

    Console.WriteLine("Deserializing file");
    var serializer = new XmlSerializer(typeof(rss));
    var byteArray = Encoding.UTF8.GetBytes(fileResult.result);
    using var memoryStream = new MemoryStream(byteArray);
    var reader = new StreamReader(memoryStream);
    var rss = (rss)serializer.Deserialize(reader);
    reader.Close();

    Console.WriteLine("Processing data");
    foreach (var item in rss.channel.item)
    {
        if (item != null && item.title.Length >= 65)
        {
            item.title = item.title[..55];
            item.title += "...";
        }
    }

    var xsSubmit = new XmlSerializer(typeof(rss));
    var xml = "";
    using var sww = new StringWriter();
    using XmlWriter writer = XmlWriter.Create(sww);
    var ns = new XmlSerializerNamespaces();
    ns.Add("", "");
    xsSubmit.Serialize(writer, rss, ns);
    xml = sww.ToString();
    File.WriteAllText("index-shs.xml", xml);
    Console.WriteLine("Attempting to copy altered file");
    File.Move("index-shs.xml", Path.Combine(destinationFolder, "index-shs.xml"), true);
    Console.WriteLine("Copy of altered file complete");
}

static async Task<(bool success, string result)> GeFile(string url)
{
    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
    using (HttpClient client = new HttpClient())
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.81 Safari/537.36 Edg/104.0.1293.47");

        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var result = await response.Content.ReadAsStringAsync();
                return (!String.IsNullOrEmpty(result), result ?? String.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured {ex.Message}");
            }
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return (false, String.Empty);
        }

        return (false, String.Empty);
    }
}