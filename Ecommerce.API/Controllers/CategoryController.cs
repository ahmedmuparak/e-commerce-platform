using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.DTOs.CategoryDTOs;

namespace Ecommerce.API.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository repo;

        public CategoryController(ICategoryRepository repo)
        {
            this.repo = repo;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await repo.GetById(id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDTO dto)
        {
            if (dto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                Name = dto.Name
            };

            await repo.Add(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }


        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Category = await repo.GetAll();
            return Ok(Category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , UpdateCategoryDTO dto)
        {
            var Category = await repo.GetById(id);
            if (Category == null)
                return NotFound();

            Category.Name = dto.Name;
            await repo.Update(Category);
            return Ok("Updated Successfully");
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await repo.GetById(id);
            if(category == null)
                return NotFound();
            await repo.Delete(category);
            return Ok();
        }
    }
}
