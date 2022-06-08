using Belong.SelfTours.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belong.SelfTours.Infra
{
    public class SelfTourDbContext : DbContext
    {
        public SelfTourDbContext(DbContextOptions<SelfTourDbContext> options) : base(options) { }

        public DbSet<SelfTour> SelfTours { get; set; }
        public DbSet<Home> Homes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Home>().HasKey(b => b.Id);
            modelBuilder.Entity<SelfTour>().HasKey(b => b.Id);

            modelBuilder.Entity<SelfTour>()
                    .HasOne(b => b.Home)
                    .WithMany(a => a.SelfTours)
                    .HasForeignKey(p => p.HomeId);
        }
    }
}
