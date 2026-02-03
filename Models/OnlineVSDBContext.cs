using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using ONLINEVS_Project.Models;

namespace ONLINEVS_Project.Models
{
    public class OnlineVSDBContext : DbContext
    {
        public OnlineVSDBContext(DbContextOptions<OnlineVSDBContext> options)
            : base(options)
        {
        }

        // Add DbSet properties for your tables, e.g., Student
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Voter> Voters { get; set; }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Voting> Votings { get; set; }
    }
}