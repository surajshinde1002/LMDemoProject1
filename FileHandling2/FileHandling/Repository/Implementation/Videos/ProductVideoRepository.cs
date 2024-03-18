using FileHandling.data;
using FileHandling.Models.Domain.Pdf;
using FileHandling.Models.Domain.VideoModels;
using FileHandling.Repository.Abstract.Videos;
using Microsoft.EntityFrameworkCore;

namespace FileHandling.Repository.Implementation.Videos
{
    public class ProductVideoRepository : IProductVideoRepository
    {
        private readonly FileContext _context;

        public ProductVideoRepository(FileContext context)
        {
            _context = context;
        }
        public async Task<bool> AddVideo(Video model)
        {

            try
            {
                _context.Videos.Add(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> DeleteVideo(string name, DateTime date)
        {

            List<Video> list = await _context.Videos.ToListAsync();

            if (list != null)
            {
                foreach (var product in list)
                {
                    if (product.ResourceName == name && product.DateCreated == date)
                    {
                        product.Flag = 0;
                        //_context.Pdfs.Remove(product);
                        await _context.SaveChangesAsync();
                        return true;

                    }


                }

            }
            return false;
        }


        public async Task<string> GetVideoName(string name, DateTime date)
        {
            List<Video> list = await _context.Videos.ToListAsync();

            if (list != null)
            {
                foreach (var product in list)
                {
                    if (product.ResourceName == name && product.DateCreated == date)
                    {

                        return product.ResourceVideo;

                    }


                }

            }
            return "Video not found";

        }



        public async Task<List<Video>> GetAllVideos()
        {
            return await _context.Videos.ToListAsync();
        }

        public async Task<List<Video>> GetVideoByStandard(int standard)
        {
            List<Video> list = await _context.Videos.ToListAsync();
            List<Video> temp = new List<Video>();

            foreach (var product in list)
            {
                if (product.Standard == standard && product.Flag == 1)
                {
                    temp.Add(product);
                }
            }
            return temp;
        }

        public async Task<Video> GetVideo(string name)
        {
            List<Video> list = await _context.Videos.ToListAsync();

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

        public async Task<List<Video>> GetAllPublishableVideos(int std)
        {
            List<Video> list = await _context.Videos.ToListAsync();
            List<Video> all = new List<Video>();
            foreach (var product in list)
            {
                if (product.Standard == std && product.Flag == 2)
                {
                    all.Add(product);
                }
            }
            return all;
        }

        public async Task<int> PublishVideo(string name, int std)
        {
            List<Video> list = await _context.Videos.ToListAsync();
            DateTime date = DateTime.Now;
            foreach (var item in list)
            {
                if (item.ResourceName == name && item.Standard == std)
                {
                    item.Flag = 1;
                    item.DateCreated = date;
                    await _context.SaveChangesAsync();
                    return 1;
                }
            }
            return 0;
        }

        public async Task<List<Video>> GetAllDeletedVideos(int std)
        {
            List<Video> list = await _context.Videos.ToListAsync();
            List<Video> all = new List<Video>();
            foreach (var product in list)
            {
                if (product.Standard == std && product.Flag == 0)
                {
                    all.Add(product);
                }
            }
            return all;
        }
    }
}
