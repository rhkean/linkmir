using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using linkmir.DbModels;
using linkmir.Models;

namespace linkmir.Controllers
{
    [Route("")]
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
        public NoContentResult GetLinks()
        {
            return NoContent();
        }

        // GET: stats
        [HttpGet("stats/{shortlink}")]
        public async Task<ActionResult<LinkmirLink>>  GetStatsOnLink(string shortlink)
        {
            var linkmirLink = await _context.Links.FindAsync(shortlink);

            if (linkmirLink == null)
            {
                return NotFound();
            }

            return linkmirLink;
        }

        // GET: stats
        [HttpGet("stats/")]
        public async Task<ActionResult<LinkmirLink>>  GetQueryData(QueryDTO query)
        {
            var linkmirLink = await _context.Links.FindAsync(query.Domain);

            if (linkmirLink == null)
            {
                return NotFound();
            }

            return linkmirLink;
        }

        // GET: api/LinkmirLinks/5
        [HttpGet("{shortlink}")]
        public async Task<ActionResult<LinkDTO>> GetLinkmirLink(string shortlink, bool? redirect = false)
        {
            var linkmirLink = await _context.Links.FindAsync(shortlink);

            if (linkmirLink == null)
            {
                return NotFound();
            }

            var toReturn = new LinkDTO
            {
                ShortLink = BuildLinkmirUrl(shortlink),
                Link = linkmirLink.OriginalUri
            };
            if(redirect.GetValueOrDefault())
            {
                return Redirect(toReturn.Link);
            }
            else
            {
                return toReturn;
            }
        }

        // POST: api/LinkmirLinks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LinkDTO>> SubmitLink(SubmissionRequestDTO submission)
        {
            int submissionCount = await submission.AddOrUpdateLink(_context);
            if (submissionCount == 0)
            {
                return BadRequest();
            }

            var toReturn = new LinkDTO
            {
                Link = submission.Link,
                ShortLink = BuildLinkmirUrl(submission.ShortLink)
            };

            // return CreatedAtAction("GetLinkmirLink", new { id = linkmirLink.ShortLink }, linkmirLink);
            if(submissionCount == 1)
            {
                return CreatedAtAction(nameof(GetLinkmirLink), new { shortlink = submission.ShortLink}, toReturn);
            }
            return Ok(toReturn);
        }

        private bool LinkmirLinkExists(string id)
        {
            return _context.Links.Any(e => e.ShortLink == id);
        }

        private string BuildLinkmirUrl(string shortLink)
        {
            return Request.GetDisplayUrl() + shortLink;
        }
    }
}
