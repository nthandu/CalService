using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NUnit.Framework;

namespace Services.Integration.Tests
{
    public class CalculatorServicesTests
    {
        private ServicesWebApplicationFactory _factory;
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new ServicesWebApplicationFactory();
            _httpClient = _factory.CreateClient();
        }

        [Test]
        public async Task WhenEmptyInputGivenShouldReturnBadRequest()
        {   
            var result = await _httpClient.GetAsync("calculator");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("calculator?input=test")]
        [TestCase("calculator?input=test123")]
        [TestCase("calculator?input=test*123")]
        public async Task WhenInvalidInputGivenShouldReturnBadRequest(string endpoint)
        {
            var result = await _httpClient.GetAsync(endpoint);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("1*2*3", "6")]
        [TestCase("4+5*2", "14")]
        [TestCase("4+5/2", "6.5")]
        [TestCase("4+5/2-1", "5.5")]
        public async Task WhenValidInputGivenShouldReturnsResultOk(string queryString, 
            string expectedResult)
        {
            var endpoint = "calculator?input=" + System.Net.WebUtility.UrlEncode(queryString);
            var result = await _httpClient.GetAsync(endpoint);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var resultContent = await result.Content.ReadAsStringAsync();
            Assert.AreEqual(expectedResult, resultContent);
        }

    }
}