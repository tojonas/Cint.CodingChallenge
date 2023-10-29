using Cint.CodingChallenge.Model;
using Microsoft.EntityFrameworkCore;

namespace Cint.CodingChallenge.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
