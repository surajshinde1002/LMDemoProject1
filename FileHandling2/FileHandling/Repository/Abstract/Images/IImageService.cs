namespace FileHandling.Repository.Abstract.Images
{
    public interface IImageService
    {
        public Tuple<int, string> SaveImage(IFormFile imageFile);

        public bool DeleteImage(string imageFileName);
        //public bool DeleteVideo(string videoFileName);
        

        public Tuple<int, byte[], string> GetImage(string fileName);
        //public Tuple<int, byte[], string> GetVideo(string fileName);
        //public Tuple<int, byte[], string> GetPdf(string fileName);
    }
}
