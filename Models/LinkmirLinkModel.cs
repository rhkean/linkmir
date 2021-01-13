using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

using linkmir.DbModels;

namespace linkmir.Models
{
    public class LinkmirLinkModel : LinkDTO
    {
        protected LinkmirLinkDbItem _link;

        public LinkmirLinkModel() {}
        public LinkmirLinkModel(LinkDTO dto)
        {
            ShortLink = dto.ShortLink;
            Link = dto.Link;
        }

        public async Task<int> AddOrUpdateLink(LinkmirDbContext context)
        {
            int toReturn = 0;
            _link = new LinkmirLinkDbItem(Link);

            if(_link.IsValid())
            {
                var link = await context.Links.FindAsync(_link.ShortLink);
                if (link == null)
                {
                    link = _link;
                    context.Links.Add(link);
                }
                else
                {
                    link.SubmissionCount++;
                }

                await context.SaveChangesAsync();

                ShortLink = link.ShortLink;
                toReturn = link.SubmissionCount;
            }
            return toReturn;
        }

        public static async Task<LinkStatsDTO> GetLinkStats(LinkmirDbContext context, string shortlink)
        {
            var link = await context.Links.FindAsync(shortlink);
            if (link != null)
            {
                link.AccessCount++;

                await context.SaveChangesAsync();
            }

            var toReturn = new LinkStatsDTO
            {
                Link = link.Link,
                ShortLink = link.ShortLink,
                Domain = link.Domain,
                Subdomain = link.Subdomain,
                SubmissionCount = link.SubmissionCount,
                AccessCount = link.AccessCount
            };

            return toReturn;
        }

        public static async Task<QueryDTO> QueryLinks(LinkmirDbContext context, QueryDTO query)
        {
            var toReturn = query;

            // a link cannot have a blank domain, so assume user meant "*"
            if (string.IsNullOrWhiteSpace(query.Domain))
            {
                query.Domain = "*";
            }

            var links = context.Links.AsQueryable();
            // if query.Domain == "*", then all domains are valid, so skip this filter
            if (query.Domain != "*")
            {
                links = links.Where(l => l.Domain == query.Domain);
            }

            // if query.Subdomain != "*", filter the subset by query value
            if (query.SubDomain != "*")
            {
                links = links.Where(l => l.Subdomain == query.SubDomain);
            }

            toReturn.Links = await links.Select( l => new LinkStatsDTO
            {
                Link = l.Link,
                ShortLink = l.ShortLink,
                Domain = l.Domain,
                Subdomain = l.Subdomain,
                SubmissionCount = l.SubmissionCount,
                AccessCount = l.AccessCount
            }).ToListAsync();

            toReturn.MatchingLinksCount = await links.CountAsync();
            toReturn.TotalAccessCount = await links.SumAsync(l => l.AccessCount);
            toReturn.TotalSubmissionCount = await links.SumAsync(l => l.SubmissionCount);
            return toReturn;
        }
    }
}