using ContractMonthlyClaimsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ContractMonthlyClaimsSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<ClaimReview> ClaimReviews { get; set; }
        public DbSet<SupportingDocument> SupportingDocuments { get; set; }
    }

}

