using BlogSite.API.Data;
using BlogSite.API.Models.Domain;
using BlogSite.API.Models.DTO;
using BlogSite.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.API.Controllers
{
    //https://localhost:port/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }


        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto req)
        {
            //Map DTO to Domain model
            var category = new Category
            {
                Name = req.Name,
                UrlHandle = req.UrlHandle,
            };

            category = await categoryRepository.CreateAsync(category);

            //Map Domain model to DTO
            var res = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };
            return Ok(res);
        }


        // GET : https://localhost:7267/api/Categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();

            // Map domain model to DTO
            var res = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                res.Add(new CategoryDTO
                {
                    Name = category.Name,
                    UrlHandle = category.UrlHandle,
                    Id = category.Id
                });
            }

            return Ok(res);
        }

        // GET : https://localhost:7267/api/Categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var category = await categoryRepository.GetById(id);

            if(category is null)
            {
                return NotFound();
            }

            var res = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(res);
        }

        // PUT : https://localhost:7267/api/Categories/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id,UpdateCategoryRequestDto requestDto)
        {
            // Map DTO to domain model
            var category = new Category
            {
                Id = id,
                Name = requestDto.Name,
                UrlHandle = requestDto.UrlHandle,
            };

            category = await categoryRepository.UpdateAsync(category);
            if(category is null)
            {
                return NotFound();
            }

            // Map Domain model to DTO
            var res = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(res);

        }


        // DELETE : https://localhost:7267/api/Categories/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            // Map domain model to DTO
            var res = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };
            return Ok(res);
        }
    }
}
