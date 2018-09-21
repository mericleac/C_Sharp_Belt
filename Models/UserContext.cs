using Microsoft.EntityFrameworkCore;
namespace BeltTemplate.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Activity> Activities {get;set;}
        public DbSet<Participant> Participants {get;set;}
    }
}