using BlogSite.API.Data;
using BlogSite.API.Models.Domain;
using BlogSite.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContectAccessor;
        private readonly ApplicationDBContext dBContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContectAccessor,ApplicationDBContext dBContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContectAccessor = httpContectAccessor;
            this.dBContext = dBContext;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await dBContext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            // upload image - API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath,"Images",$"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // create URL
            //https://blogsite.com/images/filename.jpg
            var httpRequest = httpContectAccessor.HttpContext.Request;
            var url = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";
            blogImage.Url = url;

            // update image to database
            await dBContext.BlogImages.AddAsync(blogImage);
            await dBContext.SaveChangesAsync();

            return blogImage;
        }
    }
}
