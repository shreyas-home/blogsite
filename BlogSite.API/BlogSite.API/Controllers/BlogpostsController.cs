using BlogSite.API.Models.Domain;
using BlogSite.API.Models.DTO;
using BlogSite.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogpostsController : ControllerBase
    {
        private readonly IBlogpostRepository blogpostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogpostsController(IBlogpostRepository blogpostRepository,ICategoryRepository categoryRepository)
        {
            this.blogpostRepository = blogpostRepository;
            this.categoryRepository = categoryRepository;
        }

        // POST /api/blogposts
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateBlogpost([FromBody]CreateBlogpostRequestDto requestDto)
        {
            // Map DTO to Domain model
            var blogPost = new BlogPost
            {
                Author = requestDto.Author,
                Title = requestDto.Title,
                ShortDescription = requestDto.ShortDescription,
                Content = requestDto.Content,
                FeaturedImageUrl = requestDto.FeaturedImageUrl,
                PublishedDate = requestDto.PublishedDate,
                UrlHandle = requestDto.UrlHandle,
                IsVisible = requestDto.IsVisible,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in requestDto.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await blogpostRepository.CreateAsync(blogPost);

            // Map Domain model to DTO
            var res = new BlogpostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                IsVisible = blogPost.IsVisible,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id=x.Id,
                    Name=x.Name,
                    UrlHandle=x.UrlHandle,
                }).ToList()
            };

            return Ok(res);

        }

        // GET /api/blogposts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogposts()
        {
            var blogposts = await blogpostRepository.GetAllAsync();

            // Map domain model to DTO
            var res = new List<BlogpostDTO>();
            foreach (var blogPost in blogposts)
            {
                res.Add(new BlogpostDTO
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription,
                    Content = blogPost.Content,
                    UrlHandle = blogPost.UrlHandle,
                    IsVisible = blogPost.IsVisible,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Categories = blogPost.Categories.Select(x => new CategoryDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,
                    }).ToList()
                });
            }

            return Ok(res);
        }

        // GET /api/blogposts/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogpostById([FromRoute] Guid id)
        {
            var blogPost = await blogpostRepository.GetByIdAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            // Map domain model to DTO
            var res = new BlogpostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                IsVisible = blogPost.IsVisible,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };

            return Ok(res);
        }

        // GET /api/blogposts/{urlHandle}
        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogpostByurlHandle([FromRoute] string urlHandle)
        {
            var blogPost = await blogpostRepository.GetByurlHandleAsync(urlHandle);
            if (blogPost == null)
            {
                return NotFound();
            }

            // Map domain model to DTO
            var res = new BlogpostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                IsVisible = blogPost.IsVisible,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };

            return Ok(res);

        }

        // PUT /api/blogposts/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateBlogpost([FromRoute] Guid id, UpdateBlogpostRequestDto requestDto)
        { 
            // Map DTO to Domain model
            var blogPost = new BlogPost
            {
                Id = id,
                Author = requestDto.Author,
                Title = requestDto.Title,
                ShortDescription = requestDto.ShortDescription,
                Content = requestDto.Content,
                FeaturedImageUrl = requestDto.FeaturedImageUrl,
                PublishedDate = requestDto.PublishedDate,
                UrlHandle = requestDto.UrlHandle,
                IsVisible = requestDto.IsVisible,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in requestDto.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            // Call Repository to update
            var updatedBlogpost = await blogpostRepository.UpdateAsync(blogPost);

            if(updatedBlogpost == null)
            {
                return NotFound();
            }

            // Map domain model to DTO

            var res = new BlogpostDTO
            {
                Id = updatedBlogpost.Id,
                Title = updatedBlogpost.Title,
                ShortDescription = updatedBlogpost.ShortDescription,
                Content = updatedBlogpost.Content,
                UrlHandle = updatedBlogpost.UrlHandle,
                IsVisible = updatedBlogpost.IsVisible,
                FeaturedImageUrl = updatedBlogpost.FeaturedImageUrl,
                PublishedDate = updatedBlogpost.PublishedDate,
                Author = updatedBlogpost.Author,
                Categories = updatedBlogpost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()

            };

            return Ok(res);
        }

        // DELETE /api/blogposts/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlogpost(Guid id)
        {
            var blogPost = await blogpostRepository.DeleteAsync(id);
            if(blogPost == null) 
            {
                return NotFound();
            }

            // Map Domain model to DTO
            var res = new BlogpostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                IsVisible = blogPost.IsVisible,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                
            };

            return Ok(res);
        }
    }
}
