using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<LinkStatsDTO>>  GetStatsOnLink(string shortlink)
        {
            var toReturn = await LinkmirLinkModel.GetLinkStats(_context, shortlink);

            if (toReturn == null)
            {
                return NotFound();
            }

            toReturn.ShortLink = BuildLinkmirUrl(toReturn.ShortLink);
            return toReturn;
        }

        // GET: stats
        [HttpGet("stats/")]
        public async Task<ActionResult<QueryDTO>>  GetQueryData(QueryDTO query)
        {
            var toReturn = await LinkmirLinkModel.QueryLinks(_context, query);

            if (toReturn == null)
            {
                return NotFound();
            }

            return toReturn;
        }

        // GET: api/LinkmirLinks/5
        [HttpGet("{shortlink}")]
        public async Task<ActionResult<LinkDTO>> GetLinkmirLink(string shortlink, bool? redirect = false)
        {
            var link = await LinkmirLinkModel.GetLinkStats(_context, shortlink);

            if (link == null)
            {
                return NotFound();
            }

            var toReturn = new LinkDTO
            {
                ShortLink = BuildLinkmirUrl(shortlink),
                Link = link.Link
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
        public async Task<ActionResult<LinkDTO>> SubmitLink(LinkDTO submission)
        {
            var link = new LinkmirLinkModel(submission);
            int submissionCount = await link.AddOrUpdateLink(_context);
            if (submissionCount == 0)
            {
                return BadRequest();
            }

            var toReturn = new LinkDTO
            {
                Link = link.Link,
                ShortLink = BuildLinkmirUrl(link.ShortLink)
            };

            // return CreatedAtAction("GetLinkmirLink", new { id = linkmirLink.ShortLink }, linkmirLink);
            if(submissionCount == 1)
            {
                return CreatedAtAction(nameof(GetLinkmirLink), new { shortlink = link.ShortLink}, toReturn);
            }
            return Ok(toReturn);
        }

        private string BuildLinkmirUrl(string shortLink)
        {
            // return Request.GetDisplayUrl() + shortLink;
            var requestUri = new Uri(Request.GetDisplayUrl());
            var authorityUri = new Uri(requestUri.GetLeftPart(UriPartial.Authority));
            var shortUri = new Uri(authorityUri, shortLink);
            return shortUri.AbsoluteUri;
        }
    }
}
