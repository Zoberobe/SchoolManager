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
        public DbSet<ExtraCourse> ExtraCourses { get; set; }
    }
}
