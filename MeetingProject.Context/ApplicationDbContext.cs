using MeetingProject.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingProject.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
