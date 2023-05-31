using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParserXml.Model.EntitiesDbContext;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ParserXml.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public DbSet<Element> Elements { get; set; }
        public DbSet<Model.EntitiesDbContext.File> Files { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Logging> Loggings { get; set; }
    }
}
