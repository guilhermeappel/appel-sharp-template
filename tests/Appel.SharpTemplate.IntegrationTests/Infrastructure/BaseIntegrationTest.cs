using Appel.SharpTemplate.Infrastructure.Data;
using Appel.SharpTemplate.Infrastructure.Data.Repositories;
using Appel.SharpTemplate.TestsCommon.Data;
using Microsoft.EntityFrameworkCore;

namespace Appel.SharpTemplate.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTest
{
    protected readonly UnitOfWork UnitOfWork;

    protected BaseIntegrationTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        var userRepository = new UserRepository(context);
        UnitOfWork = new UnitOfWork(context, userRepository);

        DatabaseSetup.SeedData(context);
    }
}
