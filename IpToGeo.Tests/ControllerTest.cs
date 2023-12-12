using IpToGeo.Controllers;
using IpToGeo.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IpToGeo.Tests
{
    public class ControllerTest
    {
        [Theory]
        [InlineData("0.0.0.0")]
        [InlineData("255.255.255.255")]
        [InlineData("127.0.0.1")]
        public async void TestIpToGeoCityController(string input)
        {
            // todo: implement test
            // https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-8.0
            // https://stackoverflow.com/questions/49449971/how-to-use-dbcontext-inside-xunit-test-project
            var mockService = new Mock<IIpGeoService>();
            // todo: setup mockService
            var controller = new IpToGeoCityController(mockService.Object);
            var result = await controller.GetAnyIp(input);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task TestUploadNewIpDatasController()
        {
            var mockService = new Mock<IIpGeoService>();
            // todo: setup mockService
           // throw new NotImplementedException();

            var controller = new UploadNewIpDatasController(mockService.Object);

            var result = await controller.TriggerIpGeoDatabaseUpdate();
            Assert.IsType<NoContentResult>(result);
        }
    }
}
