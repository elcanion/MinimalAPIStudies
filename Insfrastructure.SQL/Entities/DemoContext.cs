using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insfrastructure.SQL.Entities
{
    public class DemoContext : DbContext
    {
        public DemoContext(DbContextOptions options) : base(options) { }
        public DbSet<CountryEntity> Countries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<CountryEntity>();
            //builder.ToTable("Countries", "dbo");
            builder.HasIndex(p => p.Name).IsUnique(true);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(200).IsRequired();
            builder.Property(p => p.FlagUri).IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
