namespace FIleUpload.Models
{
    public class TextFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }

        public int? WordCount { get; set; }

        public string? FrequentWord { get; set; }

        public string? Error { get; set; }

    }
}
