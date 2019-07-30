using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practic.Model;

namespace Practic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public CategoriesController(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var category = await _context.Categories.Where(c => c.CategoryID == categoryId).SingleOrDefaultAsync();
            return Ok(category);
        }
    }
}