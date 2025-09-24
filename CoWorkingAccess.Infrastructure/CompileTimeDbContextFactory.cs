using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoWorkingAccess.Infrastructure;

public class CompileTimeDbContextFactory : IDesignTimeDbContextFactory<CoWorkingAccessDbContext>
{
    public CoWorkingAccessDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CoWorkingAccessDbContext>();
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=atark;Username=myuser;Password=mypassword;");

        return new CoWorkingAccessDbContext(optionsBuilder.Options);
    }
}
