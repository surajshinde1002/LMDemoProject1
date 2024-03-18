namespace FileHandling.Repository.Abstract.Videos
{
    public interface IVideoService
    {
        public Tuple<int, string> SaveVideo(IFormFile imageFile);

        //public bool DeleteVideo(string videoFileName);


        public Tuple<int, byte[], string> GetVideo(string fileName);
    }
}
