

using FIleUpload.Controllers;
using FIleUpload.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
FileDb db;
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

void UpdateDb(int WordCount, string MFW, string fileName)
{
    Console.WriteLine(fileName);
    var rec = (from t in db.TextFile
               where t.Name == fileName
               select t).First();
    if (rec.WordCount == null)
    {
        rec.WordCount = WordCount;
    }
    if (string.IsNullOrEmpty(rec.FrequentWord))
    {
        rec.FrequentWord = MFW;
    }
    if (rec != null)
    {
        db.TextFile.Update(rec);
        db.SaveChanges();
    }
}

db = new FileDb(config.GetConnectionString("connstr"));

var x = Environment.GetCommandLineArgs();

if (x == null)
{
    throw new ArgumentNullException(nameof(x));
}
if (x.Length == 0)
{
    throw new ArgumentException();
}
string FileText = File.ReadAllText(x[1]);

string[] words = FileText.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

int wordCount = words.Length;

Dictionary<string, int> wordF = new Dictionary<string, int>();

foreach (string word in words)
{
    if (wordF.ContainsKey(word))
    {
        wordF[word]++;
    }
    else
    {
        wordF[word] = 1;
    }
}
var mostFreqWord = wordF.OrderByDescending(pair => pair.Value).First();





UpdateDb(wordCount, mostFreqWord.Key, x[1]);


