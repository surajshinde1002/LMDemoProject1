using FileHandling.data;
using FileHandling.Models.Domain.ImageModels;
using FileHandling.Models.Domain.Pdf;
using FileHandling.Repository.Abstract.Images;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FileHandling.Repository.Implementation.Images
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly FileContext _context;

        public ProductImageRepository(FileContext context)
        {
            _context = context;
        }
        public bool AddImage(Image model)
        {
            try
            {
                _context.Images.Add(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool DeleteImage(string name)
        {

            List<Image> list = _context.Images.ToList();

            if (list != null)
            {
                foreach (var product in list)
                {
                    if (product.ResourceName == name)
                    {
                        _context.Images.Remove(product);
                        _context.SaveChanges();
                        return true;

                    }


                }

            }
            return false;
        }


        public string GetImageName(string name)
        {
            List<Image> list = _context.Images.ToList();

            if (list != null)
            {
                foreach (var product in list)
                {
                    if (product.ResourceName == name)
                    {

                        return product.ResourceImage;

                    }


                }

            }
            return "Check the imagename";

        }

        public List<Image> GetFilesByStandard(int std)
        {
            List<Image> list = _context.Images.ToList();
            List<Image> temp = new List<Image>();

            foreach (var product in list)
            {
                if (product.Standard == std)
                {
                    temp.Add(product);
                }
            }
            return temp;
        }


        public List<Image> GetFilesByCategory(int std, string category)
        {
            List<Image> list = _context.Images.ToList();
            List<Image> temp = new List<Image>();

            foreach (var product in list)
            {
                if (product.Standard == std && product.Category == category)
                {
                    temp.Add(product);
                }
            }
            return temp;
        }


    }
}
