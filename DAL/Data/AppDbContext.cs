using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DAL.Data
{
    public class AppDbContext : IdentityDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
             
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Category> Categories { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed Notes (Random Data)
            // Get the maximum existing Id from the table
            int maxId = Notes.Max(n => n.Id);


            // Seed Notes (Random Data)
            Random random = new Random();
            for (int i = maxId + 1; i <= maxId + 1000; i++) // Start from the maximum existing Id + 1
            {
                modelBuilder.Entity<Note>().HasData(
                    new Note
                    {
                        Id = i,
                        userId = 1, 
                        NoteTitle = $"Note {i}",
                        NoteDescription = $"Description for Note {i}",
                        DueDate = DateTime.Now.AddDays(random.Next(1, 30)), // Random due date within the next 30 days
                        categoryId = random.Next(1, 5), // Random category ID between 1 and 4
                        Status = "Ongoing", // Random status
                        Priority = random.Next(1, 11) // Random priority between 1 and 10
                    }
                );
            }
        }*/
    }
}
