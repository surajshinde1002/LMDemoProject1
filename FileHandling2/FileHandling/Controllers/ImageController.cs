using FileHandling.Models.Domain.ImageModels;
using FileHandling.Models.DTO;
using FileHandling.Repository.Abstract.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace FileHandling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController(IProductImageRepository productRepo, IImageService fileService) : ControllerBase
    {
        private IImageService _fileService = fileService;
        private IProductImageRepository _productRepo = productRepo;



        [HttpPost]
        public IActionResult AddImage([FromForm] Image model)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass the valid data";
                return Ok(status);
            }
            if (model.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    model.ResourceImage = fileResult.Item2; // getting name of image
                }
                var productResult = _productRepo.AddImage(model);
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


        [HttpDelete("productImage/delete")]
        public IActionResult DeleteImage(string imageName)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass the valid data";
                return Ok(status);
            }


            string Name = _productRepo.GetImageName(imageName);


            if (imageName != null)
            {
                var fileResult = _fileService.DeleteImage(Name);
                if (fileResult != true)
                {
                    status.StatusCode = 1;
                    status.Message = "Enter valid name";// getting name of image
                }

                var productResult = _productRepo.DeleteImage(imageName);
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













        [HttpGet("get/image/{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            var status = new Status();
            string Name = _productRepo.GetImageName(fileName);

            var result = _fileService.GetImage(Name);

            if (result.Item1 == 1)
            {
                status.StatusCode = 1;
                status.Message = "ImageRetrieved successfully";
                return File(result.Item2, result.Item3); // Return the file content with the appropriate content type
                
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on Showing product";
                return NotFound(); // File not found or an error occurred
            }
        }



        [HttpPost("file/standard/{std}")]
        public async Task<IActionResult> GetAllFiles(int std)
        {
            var status = new Status();
            var pdfs = _productRepo.GetFilesByStandard(std);

            if (pdfs != null && pdfs.Any())
            {
                var pdfResults = new List<FileResponseModel>();

                foreach (var pdf in pdfs)
                {
                    var result = _fileService.GetImage(pdf.ResourceImage);

                    if (result.Item1 == 1)
                    {
                        var pdfResult = new FileResponseModel
                        {
                            StatusCode = 1,
                            Message = "Pdf Retrieved Successfully",
                            PdfName = pdf.ResourceName,
                            Category = pdf.Category,
                            PdfContent = result.Item2,
                            Standard = pdf.Standard,
                            ContentType = result.Item3
                        };
                        pdfResults.Add(pdfResult);
                    }
                }

                return Ok(pdfResults);
            }

            status.StatusCode = 0;
            status.Message = "No PDFs found";
            return NotFound(status);
        }


        [HttpPost("file/standard/{std}/{Category}")]
        public async Task<IActionResult> GetAllFiles(int std, string Category)
        {
            var status = new Status();
            var pdfs = _productRepo.GetFilesByCategory(std, Category);

            if (pdfs != null && pdfs.Any())
            {
                var pdfResults = new List<FileResponseModel>();

                foreach (var pdf in pdfs)
                {
                    var result = _fileService.GetImage(pdf.ResourceImage);

                    if (result.Item1 == 1)
                    {
                        var pdfResult = new FileResponseModel
                        {
                            StatusCode = 1,
                            Message = "Pdf Retrieved Successfully",
                            PdfName = pdf.ResourceName,
                            Category = pdf.Category,
                            PdfContent = result.Item2,
                            Standard = pdf.Standard,
                            ContentType = result.Item3
                        };
                        pdfResults.Add(pdfResult);
                    }
                }

                return Ok(pdfResults);
            }

            status.StatusCode = 0;
            status.Message = "No PDFs found";
            return NotFound(status);
        }



    }
}
