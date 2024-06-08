using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseManagement.Market.API.Models;

namespace PurchaseManagement.Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStatisticsController : ControllerBase
    {
        private readonly RepositoryContext _context;

        public ProductStatisticsController(RepositoryContext context)
        {
            _context = context;
        }

        // GET: api/ProductStatistics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductStatistics>>> GetProductStatistics()
        {
            return await _context.ProductStatistics.ToListAsync();
        }

        // GET: api/ProductStatistics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductStatistics>> GetProductStatistics(int id)
        {
            var productStatistics = await _context.ProductStatistics.FindAsync(id);

            if (productStatistics == null)
            {
                return NotFound();
            }

            return productStatistics;
        }

        // PUT: api/ProductStatistics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductStatistics(int id, ProductStatistics productStatistics)
        {
            if (id != productStatistics.Id)
            {
                return BadRequest();
            }

            _context.Entry(productStatistics).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductStatisticsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductStatistics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductStatistics>> PostProductStatistics(ProductStatistics productStatistics)
        {
            _context.ProductStatistics.Add(productStatistics);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductStatistics", new { id = productStatistics.Id }, productStatistics);
        }

        // DELETE: api/ProductStatistics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductStatistics(int id)
        {
            var productStatistics = await _context.ProductStatistics.FindAsync(id);
            if (productStatistics == null)
            {
                return NotFound();
            }

            _context.ProductStatistics.Remove(productStatistics);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductStatisticsExists(int id)
        {
            return _context.ProductStatistics.Any(e => e.Id == id);
        }
    }
}
