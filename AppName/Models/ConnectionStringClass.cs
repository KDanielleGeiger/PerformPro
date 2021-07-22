using Microsoft.EntityFrameworkCore;

namespace PerformPro.Models
{
    public class ConnectionStringClass : DbContext
    {

        public ConnectionStringClass(DbContextOptions<ConnectionStringClass> options) : base(options) { }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Supervisor> Supervisor { get; set; }

        public DbSet<Form> Form { get; set; }
    }
}