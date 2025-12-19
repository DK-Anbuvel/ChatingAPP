using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbContext :DbContext// is the representation
                                         // of session Database.
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }

    }
}
