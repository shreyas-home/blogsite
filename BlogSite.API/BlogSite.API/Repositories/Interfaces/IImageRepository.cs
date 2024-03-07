using BlogSite.API.Models.Domain;

namespace BlogSite.API.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

        Task<IEnumerable<BlogImage>> GetAll();
    }
}
