using FileHandling.Models.Domain.Pdf;

namespace FileHandling.Repository.Abstract.Pdfs
{
    public interface IProductPdfRepository
    {
        Task<bool> AddPdf(Pdf model);

        Task<bool> DeletePdf(string name, int id);

        public string GetPdfNameString(string name, DateTime date);

        Task<string> GetPdfName(string name, DateTime date);

        Task<Pdf> GetPdf(string name);

        Task<List<Pdf>> GetAllPdfs();

        Task<List<Pdf>> GetAllPublishablePdfs(int std);
        Task<List<Pdf>> GetAllDeletedPdfs(int std);
        Task<List<Pdf>> GetPdfByStandard(int standard);


        Task<int> PublishPdf(string name, int std);
    }
}
