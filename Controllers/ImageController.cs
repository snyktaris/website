using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BioAppMvc.Services;
using System.Threading.Tasks;

namespace BioAppMvc.Controllers
{
    public class ImageController : Controller
    {
        private readonly BlobService _blobService;

        public ImageController(BlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var fileName = file.FileName;
            var contentType = file.ContentType;

            // Upload the file to BlobStorage
            using (var stream = file.OpenReadStream())
            {
                // Pass file stream, file name, and content type to BlobService for upload
                await _blobService.UploadFileAsync(stream, fileName, contentType);
            }

            // Get the URL of the uploaded image (the method is assumed to be in BlobService)
            var imageUrl = _blobService.GetBlobUrl(fileName);

            // Return the URL as part of the response
            return Ok(new { url = imageUrl });
        }
    }
}
