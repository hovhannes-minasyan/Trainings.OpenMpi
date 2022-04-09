using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainings.OpenMpi.Dal.Entities;

namespace Trainings.OpenMpi.Dal
{
    public class TrainingMpiDbContext : DbContext
    {
        public TrainingMpiDbContext(DbContextOptions<TrainingMpiDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(u => u.Id);
            });
        }
    }
}
