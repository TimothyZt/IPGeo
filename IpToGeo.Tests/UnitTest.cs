using IpToGeo.Controllers;
using IpToGeo.Services;
using IpToGeo.Utilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace IpToGeo.Tests
{
    public class UnitTest
    {
        [Theory]
        [InlineData("0.0.0.0", 0)]
        [InlineData("255.255.255.255", 4294967295u)]
        [InlineData("127.0.0.1", 2130706433u)]
        public void IntToIpv4(string result, uint input)
        {
            Assert.Equal(result, IpFormatter.IntToIpv4(input));
        }

        [Theory]
        [InlineData("0.0.0.0", 0)]
        [InlineData("255.255.255.255", 4294967295u)]
        [InlineData("127.0.0.1", 2130706433u)]
        public void Ipv4ToInt(string input, uint result)
        {
            Assert.Equal(result, IpFormatter.Ipv4ToNum(input));
        }

        [Fact]
        public void TestIpLocationDbSourceDownloading()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpClientMock = new Mock<HttpClient>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClientMock.Object);
            var service = new IpLocationDbSourceService(httpClientFactoryMock.Object);
            var result = service.GetDataSource();
            Assert.NotNull(result);
        }
    }
}