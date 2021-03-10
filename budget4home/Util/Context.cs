using System;
using budget4home.App.Expenses;
using budget4home.App.Groups;
using budget4home.App.Labels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace budget4home.Util
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<ExpenseModel> Expenses { get; set; }
        public DbSet<LabelModel> Labels { get; set; }
        public DbSet<GroupModel> Groups { get; set; }
        public DbSet<GroupUserModel> GroupUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // define 2 foreign key as primary key
            builder
                .Entity<GroupUserModel>()
                .HasKey(x => new { x.GroupId, x.UserId });

            builder.UseIdentityColumns();
        }
    }
}