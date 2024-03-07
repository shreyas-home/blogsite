using BlogSite.API.Data;
using BlogSite.API.Models.Domain;
using BlogSite.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace BlogSite.API.Repositories.Implementation
{
    public class BlogpostRepository : IBlogpostRepository
    {
        private readonly ApplicationDBContext dBContext;

        public BlogpostRepository(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost post)
        {
            await dBContext.BlogPosts.AddAsync(post);
            await dBContext.SaveChangesAsync();
            return post;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogpost = await dBContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (existingBlogpost != null)
            {
                dBContext.BlogPosts.Remove(existingBlogpost);
                await dBContext.SaveChangesAsync();
                return existingBlogpost;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dBContext.BlogPosts.Include(x=>x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await dBContext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<BlogPost?> GetByurlHandleAsync(string urlHandle)
        {
            return await dBContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost post)
        {
            var existingBlopost = await dBContext.BlogPosts.Include(x => x.Categories)
                .FirstOrDefaultAsync(x=>x.Id == post.Id);
            
            if (existingBlopost is null)
            {
                return null;
            }

            // update blogpost
            dBContext.Entry(existingBlopost).CurrentValues.SetValues(post);

            // update categories
            existingBlopost.Categories = post.Categories;

            await dBContext.SaveChangesAsync();

            return post;
        }
    }
}
