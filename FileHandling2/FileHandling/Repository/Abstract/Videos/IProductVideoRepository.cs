using FileHandling.Models.Domain.Pdf;
using FileHandling.Models.Domain.VideoModels;

namespace FileHandling.Repository.Abstract.Videos
{
    public interface IProductVideoRepository
    {
        Task<bool> AddVideo(Video model);

        Task<bool> DeleteVideo(string name, DateTime date);

        Task<string> GetVideoName(string name, DateTime date);

        Task<Video> GetVideo(string name);

        Task<List<Video>> GetAllVideos();

        Task<List<Video>> GetAllPublishableVideos(int std);
        Task<List<Video>> GetAllDeletedVideos(int std);
        Task<List<Video>> GetVideoByStandard(int standard);

        Task<int> PublishVideo(string name, int std);
    }
}
