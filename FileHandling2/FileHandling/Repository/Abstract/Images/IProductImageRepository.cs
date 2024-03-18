using FileHandling.Models.Domain.ImageModels;
using FileHandling.Models.Domain.Pdf;

namespace FileHandling.Repository.Abstract.Images
{
    public interface IProductImageRepository
    {
        bool AddImage(Image model);
       
        bool DeleteImage(string name);

        public string GetImageName(string name);

        public List<Image> GetFilesByStandard(int standard);
        public List<Image> GetFilesByCategory(int standard , string Category);
    }
}
