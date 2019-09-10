using lp_api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace lp_api_test.Controllers
{
    public class ValuesControllerTest
    {
        private readonly HttpClient client;

        public ValuesControllerTest()
        {
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            this.client = server.CreateClient();
        }

        [Fact]
        public void GetNumber()
        {
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "/api/values");
            HttpResponseMessage response = this.client.SendAsync(request).Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
