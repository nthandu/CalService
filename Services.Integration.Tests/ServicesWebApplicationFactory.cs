using Microsoft.AspNetCore.Mvc.Testing;

namespace Services.Integration.Tests
{
    /// <summary>
    /// Factory for bootstrapping an Services in memory for
    /// functional end to end tests.
    /// </summary>
    public class ServicesWebApplicationFactory : 
        WebApplicationFactory<CalculationServices.Startup>
    {
    }
}