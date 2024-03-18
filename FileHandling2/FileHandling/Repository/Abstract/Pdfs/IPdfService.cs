using FileHandling.Models.Domain;
using FileHandling.Models.Domain.Pdf;

namespace FileHandling.Repository.Abstract.Pdfs
{
    public interface IPdfService
    {
        public Tuple<int, string> SavePdf(IFormFile imageFile);

        //public Task<Tuple<int, string>> UploadFileToS3Async(IFormFile file, string bucketName, string prefix);


        //public bool DeletePdf(string pdfFileName);
        public Tuple<int, byte[], string> GetPdf(string fileName);
        
    }
}
