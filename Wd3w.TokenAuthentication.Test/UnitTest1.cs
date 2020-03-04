using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Wd3w.TokenAuthentication.Sample;
using Xunit;

namespace Wd3w.TokenAuthentication.Test
{
    public class UnitTest1j : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public UnitTest1j(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test1()
        {
            var httpClient = _factory.CreateClient();

            var response = await httpClient.GetAsync("api/sample/public-api");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test2()
        {
            var httpClient = _factory.CreateClient();

            var response = await httpClient.GetAsync("api/sample/my-email");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Test3()
        {
            var httpClient = _factory.CreateClient();

            var response = await httpClient.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Headers =
                {
                    {"Authorization", "Bearer abcdefghijk"}
                },
                RequestUri = new Uri(httpClient.BaseAddress, "api/sample/my-email")
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}