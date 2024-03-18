using FileHandling.Repository.Abstract.Videos;

namespace FileHandling.Repository.Implementation.Videos
{
    public class VideoService : IVideoService
    {
        public IWebHostEnvironment _environment;
        public VideoService(IWebHostEnvironment environment)
        {

            _environment = environment;

        }


        public Tuple<int, string> SaveVideo(IFormFile imageFile)
        {
            try
            {
                var contentPath = _environment.ContentRootPath;
                //path = "c://Projects/Productminiapi/uploads , Something like that

                var path = Path.Combine(contentPath, "uploads");
                var ext = Path.GetExtension(imageFile.FileName);

                if (ext == ".mp4" || ext == ".mpeg")
                {
                    path = Path.Combine(contentPath, "uploads\\Videos");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }

                //Check the allowed extensions

                var allowedExtensions = new string[] { ".mp4", ".mpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }

                string uniqueString = Guid.NewGuid().ToString();

                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                //var fileWithPath = Path.Combine(path, imageFile.FileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, ex.Message);
            }

        }



        //public bool DeleteVideo(string videoFileName)
        //{
        //    try
        //    {
        //        var wwwPath = _environment.ContentRootPath;
        //        var path = Path.Combine(wwwPath, "uploads\\Videos", videoFileName);
        //        if (File.Exists(path))
        //        {

        //            File.Delete(path);
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        public Tuple<int, byte[], string> GetVideo(string fileName)
        {
            try
            {
                var wwwPath = _environment.ContentRootPath;

                var path = Path.Combine(wwwPath, "uploads\\Videos", fileName);

                if (File.Exists(path))
                {
                    var fileBytes = File.ReadAllBytes(path);

                    // Determine the content type based on the file extension
                    string contentType;
                    switch (Path.GetExtension(fileName).ToLower().Trim())
                    {
                        case ".mp4":
                            contentType = "video/mp4";
                            break;
                        case ".mpeg":
                            contentType = "video/mpeg";
                            break;
                        default:
                            contentType = "application/octet-stream";
                            break;
                    }

                    return new Tuple<int, byte[], string>(1, fileBytes, contentType);
                }
                else
                {
                    return new Tuple<int, byte[], string>(0, null, null); // File not found
                }
            }
            catch (Exception ex)
            {
                return new Tuple<int, byte[], string>(0, null, null); // Return null in case of an exception
            }
        }


    }
}
