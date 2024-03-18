using FileHandling.data;
using FileHandling.Models.Domain.Pdf;
using FileHandling.Repository.Abstract.Pdfs;
using Microsoft.EntityFrameworkCore;

namespace FileHandling.Repository.Implementation.Pdfs
{
    public class ProductPdfRepository : IProductPdfRepository
    {
        private readonly FileContext _context;

        public ProductPdfRepository(FileContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPdf(Pdf model)
        {
           
                await _context.Pdfs.AddAsync(model);
                await _context.SaveChangesAsync();
                return true;
           
        }


        public async Task<bool> DeletePdf(string name, int id)
        {
            Pdf pdfToDelete = await _context.Pdfs.FirstOrDefaultAsync(p => p.ResourceName == name && p.Id == id);

            if (pdfToDelete != null)
            {
                pdfToDelete.Flag = 0;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
            //List<Pdf> list = await _context.Pdfs.ToListAsync();

            //if (list != null)
            //{
            //    foreach (var product in list)
            //    {
            //        if (product.ResourceName == name && product.Id == id)
            //        {
            //            product.Flag = 0;
            //            //_context.Pdfs.Remove(product);
            //            await _context.SaveChangesAsync();
            //            return true;

            //        }


            //    }

            //}
            //return false;
        }


        public async Task<string> GetPdfName(string name, DateTime date)
        {
            List<Pdf> list = await _context.Pdfs.ToListAsync();

            if (list != null)
            {
                foreach (var product in list)
                {
                    if (product.ResourceName == name && product.DateCreated == date)
                    {

                        return product.ResourcePdf;

                    }


                }

            }
            return "Check the Pdf name";

        }


        public async Task<Pdf> GetPdf(string name)
        {
            List<Pdf> list = await _context.Pdfs.ToListAsync();

            if (list != null)
            {
                foreach (var product in list)
                {
                    if (product.ResourceName == name)
                    {

                        return product;

                    }


                }

            }
            return null;
        }


        public async Task<List<Pdf>> GetAllPdfs()
        {
            return await _context.Pdfs.ToListAsync();
        }

        public async Task<List<Pdf>> GetAllPublishablePdfs(int std)
        {
            return await _context.Pdfs
                       .Where(pdf => pdf.Standard == std && pdf.Flag == 2)
                       .ToListAsync();
        }

        public async Task<List<Pdf>> GetPdfByStandard(int std)
        {
            List<Pdf> list = await _context.Pdfs.ToListAsync();
            List<Pdf> temp = new List<Pdf>();

            foreach (var product in list)
            {
                if (product.Standard == std && product.Flag == 1)
                {
                    temp.Add(product);
                }
            }
            return temp;
        }



        public async Task<List<Pdf>> GetAllDeletedPdfs(int std)
        {
            List<Pdf> list = await _context.Pdfs.ToListAsync();
            List<Pdf> all = new List<Pdf>();
            foreach (var product in list)
            {
                if (product.Standard == std && product.Flag == 0)
                {
                    all.Add(product);
                }
            }
            return all;
        }


        public async Task<int> PublishPdf(string name, int id)
        {
            List<Pdf> list = await _context.Pdfs.ToListAsync();
            DateTime date = DateTime.Now;
            foreach (var item in list)
            {
                if (item.ResourceName == name && item.Id == id)
                {
                    item.Flag = 1;
                    item.DateCreated = date;
                    await _context.SaveChangesAsync();
                    return 1;
                }
            }
            return 0;
        }

        public string GetPdfNameString(string name, DateTime date)
        {
            List<Pdf> list = _context.Pdfs.ToList();

            if (list != null)
            {
                foreach (var product in list)
                {
                    if (product.ResourceName == name && product.DateCreated == date)
                    {

                        return product.ResourcePdf;

                    }


                }

            }
            return "Check the Pdf name";

        }

    }
}
