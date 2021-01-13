using Microsoft.EntityFrameworkCore;

namespace linkmir.DbModels
{
    public class LinkmirDbContext : DbContext
    {
        public LinkmirDbContext(DbContextOptions<LinkmirDbContext> options)
            : base(options)
        {

        }

        public DbSet<LinkmirLinkDbItem> Links { get; set; }
    }
}