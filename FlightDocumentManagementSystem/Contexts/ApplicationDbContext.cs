using FlightDocumentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        #region DbSet
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Display> Displays { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Signature> Signatures { get; set; }
        public DbSet<TokenManager> TokenManagers { get; set; }
        public DbSet<User> Users { get; set; }
        #endregion
    }
}
