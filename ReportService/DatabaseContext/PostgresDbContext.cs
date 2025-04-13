using Microsoft.EntityFrameworkCore;
using ReportService.Models;

namespace ReportService.DatabaseContext
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportDetail> ReportDetails { get; set; }
    }
}
