using Microsoft.EntityFrameworkCore;

namespace budget4home.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<LabelModel> Labels { get; set; }
    }
}