using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation_System.Data;

namespace Reservation_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }

        [HttpGet]
        public IActionResult GetCategories()
        {

            var categories = _dbContext.Categories.Select(c=> new {c.CategoryId, c.CategoryName}).ToList();

            return Ok(categories);
        }



    }
}
