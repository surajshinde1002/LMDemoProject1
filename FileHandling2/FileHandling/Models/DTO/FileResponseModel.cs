namespace FileHandling.Models.DTO
{
    public class FileResponseModel
    {
        public int? id { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public string PdfName { get; set; }

        public string Category { get; set; }
        public string Subject { get; set; }

        public string Description { get; set; }
        public DateTime? Created { get; set; }

        public int? Standard {  get; set; }
        public byte[] PdfContent { get; set; }
        public string ContentType { get; set; }
    }
}
