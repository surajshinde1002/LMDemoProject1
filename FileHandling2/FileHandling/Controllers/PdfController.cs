using FileHandling.data;
using FileHandling.Models.Domain.Pdf;
using FileHandling.Models.DTO;
using FileHandling.Repository.Abstract.Images;
using FileHandling.Repository.Abstract.Pdfs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace FileHandling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController(IProductPdfRepository productPdfRepo, IPdfService pdfService, FileContext db) : ControllerBase
    {
        private IPdfService _fileService = pdfService;
        private IProductPdfRepository _productRepo = productPdfRepo;
        private FileContext _db;


        //1
        [HttpPost]
        public async Task<IActionResult> AddPdf([FromForm] Pdf model)
        {
            string bucketName = "learning-s3-lm";
            string prefix = "group3_FileHandling";
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass the valid data";
                return Ok(status);
            }
            if (model.PdfFile != null)
            {
                var fileResult = _fileService.SavePdf(model.PdfFile);
                //var fileResult = await _fileService.UploadFileToS3Async(model.PdfFile, bucketName, prefix);
                if (fileResult.Item1 == 1)
                {
                    model.ResourcePdf = fileResult.Item2; // getting name of image
                }
                else
                {
                    return BadRequest(fileResult);
                }
                var productResult = await _productRepo.AddPdf(model);
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

        //2
        [HttpDelete("productPdf/delete/{pdfName}/{id}")]
        public async Task<IActionResult> DeletePdf(string pdfName, int id)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass the valid data";
                return Ok(status);
            }

            if (pdfName != null)
            {
                //var fileResult = _fileService.DeletePdf(Name);
                //if (fileResult != true)
                //{
                //    status.StatusCode = 0;
                //    status.Message = "Enter valid name";// getting name of image
                //}

                var productResult = await _productRepo.DeletePdf(pdfName, id);
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

        //3
        [HttpGet("get/pdf/{fileName}")]
        public async Task<IActionResult> GetPdf(string fileName)
        {
            var status = new Status();
            Pdf pdf = await _productRepo.GetPdf(fileName);
            if (pdf != null)
            {
                var result = _fileService.GetPdf(pdf.ResourcePdf);

                if (result.Item1 == 1)
                {

                    return File(result.Item2, result.Item3);
                }
            }

            status.StatusCode = 0;
            status.Message = "Error on showing Product";
            return NotFound(status);

        }

        //4
        [HttpGet("get/all/pdfs")]
        public async Task<IActionResult> GetAllPdfs()
        {
            var status = new Status();
            var pdfs = await _productRepo.GetAllPdfs();

            if (pdfs != null && pdfs.Any())
            {
                var pdfResults = new List<FileResponseModel>();

                foreach (var pdf in pdfs)
                {
                    var result = _fileService.GetPdf(pdf.ResourcePdf);

                    if (result.Item1 == 1)
                    {
                        var pdfResult = new FileResponseModel
                        {
                            id = pdf.Id,
                            StatusCode = 1,
                            Message = "Pdf Retrieved Successfully",
                            PdfName = pdf.ResourceName,
                            Category = pdf.ResourceCategory,
                            Description = pdf.ResourceDescription,
                            Subject = pdf.Subject,
                            Created = pdf.DateCreated,
                            Standard = pdf.Standard,
                            PdfContent = result.Item2,
                            //ContentType = result.Item3
                            ContentType = result.Item3[..1]

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

        //5
        [HttpPost("Pdf/Standard/{std}")]
        public async Task<IActionResult> GetAllPdfs(int std)
        {
            var status = new Status();
            var pdfs = await _productRepo.GetPdfByStandard(std);

            if (pdfs != null && pdfs.Any())
            {
                var pdfResults = new List<FileResponseModel>();

                foreach (var pdf in pdfs)
                {
                    var result = _fileService.GetPdf(pdf.ResourcePdf);

                    if (result.Item1 == 1)
                    {
                        var pdfResult = new FileResponseModel
                        {
                            id = pdf.Id,
                            StatusCode = 1,
                            Message = "Pdf Retrieved Successfully",
                            PdfName = pdf.ResourceName,
                            Category = pdf.ResourceCategory,
                            Description = pdf.ResourceDescription,
                            Subject = pdf.Subject,
                            Created = pdf.DateCreated,
                            Standard = pdf.Standard,
                            PdfContent = result.Item2,
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

        //6
        [HttpPost("Pdf/Standard/Publishable/{std}")]
        public async Task<IActionResult> GetAllPublishablePdfs(int std)
        {
            var status = new Status();
            var pdfs = await _productRepo.GetAllPublishablePdfs(std);

            if (pdfs != null && pdfs.Any())
            {
                var pdfResults = new List<FileResponseModel>();

                foreach (var pdf in pdfs)
                {
                    var result = _fileService.GetPdf(pdf.ResourcePdf);

                    if (result.Item1 == 1)
                    {
                        var pdfResult = new FileResponseModel
                        {
                            id = pdf.Id,
                            StatusCode = 1,
                            Message = "Pdf Retrieved Successfully",
                            PdfName = pdf.ResourceName,
                            Category = pdf.ResourceCategory,
                            Description = pdf.ResourceDescription,
                            Subject = pdf.Subject,
                            Created = pdf.DateCreated,
                            Standard = pdf.Standard,
                            PdfContent = result.Item2,
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

        //7
        [HttpPost("Pdf/Standard/Deleted/{std}")]
        public async Task<IActionResult> ShowAllDeletedPdfs(int std)
        {
            var status = new Status();
            var pdfs = await _productRepo.GetAllDeletedPdfs(std);

            if (pdfs != null && pdfs.Any())
            {
                var pdfResults = new List<FileResponseModel>();

                foreach (var pdf in pdfs)
                {
                    var result = _fileService.GetPdf(pdf.ResourcePdf);

                    if (result.Item1 == 1)
                    {
                        var pdfResult = new FileResponseModel
                        {
                            id = pdf.Id,
                            StatusCode = 1,
                            Message = "Pdf Retrieved Successfully",
                            PdfName = pdf.ResourceName,
                            Category = pdf.ResourceCategory,
                            Description = pdf.ResourceDescription,
                            Subject = pdf.Subject,
                            Created = pdf.DateCreated, // Corrected line
                            Standard = pdf.Standard,
                            PdfContent = result.Item2,
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

        //8
        [HttpPost("Publish/{name}/{id}")]
        public async Task<IActionResult> PublishPdf(string name, int id)
        {
            Status status = new Status();
            int stat = await _productRepo.PublishPdf(name, id);
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


        [HttpGet]
        [Route("DownloadFile/{filename}/{date}")]
        public async Task<IActionResult> DownloadFile(string filename, DateTime date)
        {
            string name = _productRepo.GetPdfNameString(filename, date);
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "uploads\\Pdf's", name);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            //return File(bytes, contenttype, Path.GetFileName(filepath));
            return File(bytes, contenttype, filename+Path.GetExtension(filepath));
        }

    }
}
