using Microsoft.EntityFrameworkCore;

namespace appointmentLookupModel;
public class AppDbContext:DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }
    public DbSet<LookupResult>? LookupResults { get; set; }  
}