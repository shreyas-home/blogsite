using BlogSite.API.Models.Domain;

namespace BlogSite.API.Repositories.Interfaces
{
    public interface IBlogpostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost post);

        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetByIdAsync(Guid id);

        Task<BlogPost?> GetByurlHandleAsync(string urlHandle);

        Task<BlogPost?> UpdateAsync(BlogPost post);

        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
