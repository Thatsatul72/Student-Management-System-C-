using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem
{
    public class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        private readonly string _dbPath;

        public StudentContext()
        {
            // Database file will be created in the application folder
            _dbPath = "students.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.RollNumber)
                .IsUnique(false); // If you want roll unique, set true
        }
    }
}
