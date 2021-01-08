using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using linkmir.DbModels;

namespace linkmir.Models
{
    public class SubmissionRequestDTO : LinkDTO
    {
        private LinkmirLink _link;

        public async Task<int> AddOrUpdateLink(LinkmirDbContext context)
        {
            int toReturn = 0;
            _link = new LinkmirLink(Link);

            if(_link.IsValid())
            {
                LinkmirLink link = await context.Links.FindAsync(_link.ShortLink);
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

        // private bool LinkmirLinkExists(string id)
        // {
        //     return _context.Links.Any(e => e.ShortLink == id);
        // }
    }
}