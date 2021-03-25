using KweetService.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KweetService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KweetController : ControllerBase
    {
        private readonly KweetDatabaseContext _dbContext;

        public KweetController(KweetDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kweet>>> GetKweets()
        {
            return await _dbContext.Kweets.ToListAsync();
        }

        [HttpGet("{kweetId}")]
        public async Task<ActionResult<Kweet>> GetKweetById(int id)
        {
            return await _dbContext.Kweets.FirstOrDefaultAsync(k => k.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateKweet([FromBody] string message)
        {
            var kweet = new Kweet
            {
                Message = message
            };

            _dbContext.Kweets.Add(kweet);
            await _dbContext.SaveChangesAsync();

            return Created($"/api/kweet/{kweet.Id}", kweet);
        }

        [HttpDelete("{kweetId}")]
        public async Task<IActionResult> DeleteKweet(int id)
        {
            var kweet = await _dbContext.Kweets.FirstOrDefaultAsync(k => k.Id == id);

            if(kweet == default)
            {
                return BadRequest();
            }

            _dbContext.Kweets.Remove(kweet);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateKweet(int kweetId, Kweet kweet)
        {
            kweet.Id = kweetId;
            _dbContext.Attach(kweet).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
