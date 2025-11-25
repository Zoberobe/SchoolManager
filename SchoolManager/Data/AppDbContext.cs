using Microsoft.EntityFrameworkCore;
using SchoolManager.Models;

namespace SchoolManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudyGroup> StudyGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudyGroup>(entity =>
            {
                entity.HasMany(x => x.Students)
                      .WithOne(x => x.StudyGroup)
                      .HasForeignKey(x => x.StudyGroupId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Teacher)
                      .WithMany(x => x.StudyGroups)
                      .HasForeignKey(x => x.TeacherId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
