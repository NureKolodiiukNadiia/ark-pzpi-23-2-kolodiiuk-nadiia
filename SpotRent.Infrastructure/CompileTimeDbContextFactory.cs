using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SpotRent.Infrastructure;

public class CompileTimeDbContextFactory : IDesignTimeDbContextFactory<SpotRentDbContext>
{
    public SpotRentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SpotRentDbContext>();
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=atark;Username=myuser;Password=mypassword;");

        return new SpotRentDbContext(optionsBuilder.Options);
    }
}
