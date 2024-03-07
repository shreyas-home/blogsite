using BlogSite.API.Data;
using BlogSite.API.Models.Domain;
using BlogSite.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext dBContext;

        public CategoryRepository(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dBContext.Categories.AddAsync(category);
            await dBContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var category = await dBContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return null;
            }

            dBContext.Categories.Remove(category);
            await dBContext.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dBContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await dBContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await dBContext.Categories.FirstOrDefaultAsync(x=>x.Id == category.Id);
            if (existingCategory != null)
            {
                dBContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await dBContext.SaveChangesAsync();
                return category;
            }
            return null;
        }
    }
}
