
using FinancialApi.IntegrationTests.Confs;

namespace FinancialApi.IntegrationTests
{
    public class HealthCheckControllerTest
    {
        private readonly HttpClient _client;

        public HealthCheckControllerTest(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }
    }
}