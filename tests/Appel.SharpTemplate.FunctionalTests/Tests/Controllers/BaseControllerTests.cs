using Appel.SharpTemplate.FunctionalTests.Infrastructure;
using Xunit;

namespace Appel.SharpTemplate.FunctionalTests.Tests.Controllers;

public class BaseControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;

    public BaseControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        Client = factory.CreateClient();
    }
}

