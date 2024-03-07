using BlogSite.API.Models.Domain;
using BlogSite.API.Models.DTO;
using BlogSite.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }


        // Get /api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await imageRepository.GetAll();

            // Map Domain models to DTO
            var res = new List<BlogImageDTO>();
            foreach (var image in images)
            {
                res.Add(new BlogImageDTO
                {
                    Title = image.Title,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Id = image.Id,
                    Url = image.Url,
                    DateCreated = image.DateCreated,

                });
            }

            return Ok(res);
        }

        //POST /api/images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
            [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);

            if(ModelState.IsValid)
            {
                // Upload file
                var blogImage = new BlogImage
                { 
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    Title = title,
                    FileName = fileName,
                    DateCreated = DateTime.Now,
                };

                blogImage = await imageRepository.Upload(file, blogImage);

                // Map Domain model to DTO
                var res = new BlogImageDTO
                {
                    Title = blogImage.Title,
                    FileName = blogImage.FileName,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    Id = blogImage.Id,
                    Url = blogImage.Url,
                };

                return Ok(res);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if(file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size is too large");
            }
        }
    }
}
