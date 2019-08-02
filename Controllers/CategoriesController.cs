using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practic.Model;

namespace Practic.Controllers
{
    /*
     * The code below is extra for now. I did not use it. 
     * It is category controller that fetch category information depending on category ID.
     */
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public CategoriesController(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        /*
         * Controller that helps fetching categories by category ID.
         */
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var category = await _context.Categories.Where(c => c.CategoryID == categoryId).SingleOrDefaultAsync();
            return Ok(category);
        }
    }
}