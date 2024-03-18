using FileHandling.Models.Domain.Pdf;
using FileHandling.Models.Domain.VideoModels;
using FileHandling.Models.DTO;
using FileHandling.Repository.Abstract.Videos;
using Microsoft.AspNetCore.Mvc;

namespace FileHandling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController(IProductVideoRepository productRepo, IVideoService fileService) : ControllerBase
    {
        private IVideoService _fileService = fileService;
        private IProductVideoRepository _productRepo = productRepo;
        [HttpPost]
        public async Task<IActionResult> AddVideo([FromForm] Video model)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass the valid data";
                return Ok(status);
            }
            if (model.VideoFile != null)
            {
                var fileResult = _fileService.SaveVideo(model.VideoFile);
                if (fileResult.Item1 == 1)
                {
                    model.ResourceVideo = fileResult.Item2; // getting name of image
                }
                else
                {
                    return BadRequest(fileResult);
                }
                var productResult = await _productRepo.AddVideo(model);
                if (productResult)
                {
                    status.StatusCode = 1;
                    status.Message = "Added successfully";
                }
                else
                {
                    status.StatusCode = 0;
                    status.Message = "Error on adding product";

                }
            }
            return Ok(status);

        }




        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteVideo(string videoName, DateTime date)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass the valid data";
                return Ok(status);
            }


            if (videoName != null)
            {
                //var fileResult = _fileService.DeleteVideo(Name);
                //if (fileResult != true)
                //{
                //    status.StatusCode = 1;
                //    status.Message = "Enter valid name";// getting name of image
                //}

                var productResult = await _productRepo.DeleteVideo(videoName, date);
                if (productResult)
                {
                    status.StatusCode = 1;
                    status.Message = "Deleted successfully";
                }
                else
                {
                    status.StatusCode = 0;
                    status.Message = "Error on Deleting product";

                }
            }
            return Ok(status);
        }









        [HttpGet("get/{fileName}")]
        public async Task<IActionResult> GetVideo(string fileName, DateTime date)
        {
            var status = new Status();
            string Name = await _productRepo.GetVideoName(fileName, date);

            var result = _fileService.GetVideo(Name);

            if (result.Item1 == 1)
            {
                status.StatusCode = 1;
                status.Message = "Video Retrieved successfully";
                return File(result.Item2, result.Item3); // Return the file content with the appropriate content type

            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on Showing product";
                return NotFound(); // File not found or an error occurred
            }
        }


        [HttpGet("getall")]
        public async Task<IActionResult> GetAllVideos()
        {
            var status = new Status();
            var videos = await _productRepo.GetAllVideos();

            if (videos != null && videos.Any())
            {
                var videoResults = new List<FileResponseModel>();

                foreach (var video in videos)
                {
                    var result = _fileService.GetVideo(video.ResourceVideo);

                    if (result.Item1 == 1)
                    {
                        var videoResult = new FileResponseModel
                        {
                            StatusCode = 1,
                            Message = "Pdf Retrieved Successfully",
                            PdfName = video.ResourceName,
                            Category = video.ResourceCategory,
                            Description = video.ResourceDescription,
                            Subject = video.Subject,
                            Created = video.DateCreated,
                            Standard = video.Standard,
                            PdfContent = result.Item2,
                            ContentType = result.Item3
                        };
                        videoResults.Add(videoResult);
                    }
                }

                return Ok(videoResults);
            }

            status.StatusCode = 0;
            status.Message = "No Videos found";
            return NotFound(status);
        }



        [HttpPost("Standard/{std}")]
        public async Task<IActionResult> GetAllVideos(int std)
        {
            var status = new Status();
            var videos = await _productRepo.GetVideoByStandard(std);

            if (videos != null && videos.Any())
            {
                var videoResults = new List<FileResponseModel>();

                foreach (var video in videos)
                {
                    var result = _fileService.GetVideo(video.ResourceVideo);

                    if (result.Item1 == 1)
                    {
                        var videoResult = new FileResponseModel
                        {
                            StatusCode = 1,
                            Message = "Video Retrieved Successfully",
                            PdfName = video.ResourceName,
                            Category = video.ResourceCategory,
                            Description = video.ResourceDescription,
                            Subject = video.Subject,
                            Created = video.DateCreated,
                            Standard = video.Standard,
                            PdfContent = result.Item2,
                            ContentType = result.Item3
                        };
                        videoResults.Add(videoResult);
                    }
                }

                return Ok(videoResults);
            }

            status.StatusCode = 0;
            status.Message = "No Videos found";
            return NotFound(status);
        }


        [HttpPost("Standard/Publishable/{std}")]
        public async Task<IActionResult> GetAllPublishablePdfs(int std)
        {
            var status = new Status();
            var videos = await _productRepo.GetAllPublishableVideos(std);

            if (videos != null && videos.Any())
            {
                var pdfResults = new List<FileResponseModel>();

                foreach (var video in videos)
                {
                    var result = _fileService.GetVideo(video.ResourceVideo);

                    if (result.Item1 == 1)
                    {
                        var pdfResult = new FileResponseModel
                        {
                            StatusCode = 1,
                            Message = "Video Retrieved Successfully",
                            PdfName = video.ResourceName,
                            Category = video.ResourceCategory,
                            Description = video.ResourceDescription,
                            Subject = video.Subject,
                            Created = video.DateCreated,
                            Standard = video.Standard,
                            PdfContent = result.Item2,
                            ContentType = result.Item3
                        };
                        pdfResults.Add(pdfResult);
                    }
                }

                return Ok(pdfResults);
            }

            status.StatusCode = 0;
            status.Message = "No Videos found";
            return NotFound(status);
        }


        [HttpPost("Standard/Deleted/{std}")]
        public async Task<IActionResult> ShowAllDeletedVideos(int std)
        {
            var status = new Status();
            var videos = await _productRepo.GetAllDeletedVideos(std);

            if (videos != null && videos.Any())
            {
                var pdfResults = new List<FileResponseModel>();

                foreach (var video in videos)
                {
                    var result = _fileService.GetVideo(video.ResourceVideo);

                    if (result.Item1 == 1)
                    {
                        var pdfResult = new FileResponseModel
                        {
                            StatusCode = 1,
                            Message = "Video Retrieved Successfully",
                            PdfName = video.ResourceName,
                            Category = video.ResourceCategory,
                            Description = video.ResourceDescription,
                            Subject = video.Subject,
                            Created = video.DateCreated,
                            Standard = video.Standard,
                            PdfContent = result.Item2,
                            ContentType = result.Item3
                        };
                        pdfResults.Add(pdfResult);
                    }
                }

                return Ok(pdfResults);
            }

            status.StatusCode = 0;
            status.Message = "No Videos found";
            return NotFound(status);
        }


        [HttpPost("Publish")]
        public async Task<IActionResult> PublishVideo(string name, int std)
        {
            Status status = new Status();
            int stat = await _productRepo.PublishVideo(name, std);
            if (stat == 1)
            {
                status.StatusCode = 1;
                status.Message = "File Published Successfully";
                return Ok(status);
            }
            status.StatusCode = 0;
            status.Message = "There is some issue while publishing";
            return BadRequest(status);
        }

    }
}
