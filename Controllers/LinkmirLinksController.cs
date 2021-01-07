using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using linkmir.DbModels;

namespace linkmir.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkmirLinksController : ControllerBase
    {
        private readonly LinkmirDbContext _context;

        public LinkmirLinksController(LinkmirDbContext context)
        {
            _context = context;
        }

        // GET: api/LinkmirLinks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LinkmirLink>>> GetLinks()
        {
            return await _context.Links.ToListAsync();
        }

        // GET: api/LinkmirLinks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LinkmirLink>> GetLinkmirLink(string id)
        {
            var linkmirLink = await _context.Links.FindAsync(id);

            if (linkmirLink == null)
            {
                return NotFound();
            }

            return linkmirLink;
        }

        // PUT: api/LinkmirLinks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLinkmirLink(string id, LinkmirLink linkmirLink)
        {
            if (id != linkmirLink.ShortLink)
            {
                return BadRequest();
            }

            _context.Entry(linkmirLink).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinkmirLinkExists(id))
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

        // POST: api/LinkmirLinks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LinkmirLink>> PostLinkmirLink(LinkmirLink linkmirLink)
        {
            _context.Links.Add(linkmirLink);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LinkmirLinkExists(linkmirLink.ShortLink))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLinkmirLink", new { id = linkmirLink.ShortLink }, linkmirLink);
        }

        // DELETE: api/LinkmirLinks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinkmirLink(string id)
        {
            var linkmirLink = await _context.Links.FindAsync(id);
            if (linkmirLink == null)
            {
                return NotFound();
            }

            _context.Links.Remove(linkmirLink);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LinkmirLinkExists(string id)
        {
            return _context.Links.Any(e => e.ShortLink == id);
        }
    }
}
