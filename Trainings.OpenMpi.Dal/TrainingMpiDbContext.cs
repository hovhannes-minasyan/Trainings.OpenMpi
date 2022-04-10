using Microsoft.EntityFrameworkCore;
using Trainings.OpenMpi.Dal.Entities;

namespace Trainings.OpenMpi.Dal
{
    public class TrainingMpiDbContext : DbContext
    {
        public TrainingMpiDbContext(DbContextOptions<TrainingMpiDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        public DbSet<ConcurrencyGame> ConcurrencyGames { get; set; }

        public DbSet<ConcurrencyGameRound> ConcurrencyGameRounds { get; set; }

        public DbSet<Game> Games { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("citext");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).HasColumnType("citext");
            });

            modelBuilder.Entity<ConcurrencyGameRound>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ConcurrencyGame).WithMany(e => e.ConcurrencyGameRounds).HasForeignKey(e => e.ConcurrencyGameId);
                entity.HasIndex(e => e.ConcurrencyGameId);
            });

            modelBuilder.Entity<ConcurrencyGame>(entity =>
            {
                entity.HasKey(u => u.GameId);
                entity.Property(u => u.GameId).ValueGeneratedNever();
            });

            modelBuilder.Entity<QuizQuestion>(entity =>
            {
                entity.HasKey(u => u.Id);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ConcurrencyGame).WithOne(e => e.Game).HasForeignKey<ConcurrencyGame>(e => e.GameId);
            });
        }
    }
}
