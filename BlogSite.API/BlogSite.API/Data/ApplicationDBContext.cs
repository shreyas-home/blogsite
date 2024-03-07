using BlogSite.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.API.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts  { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }

    }
}
