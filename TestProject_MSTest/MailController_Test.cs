using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using WebApp.Controllers;
using WebApp.Data;
using WebApp.Models;

namespace TestProject_MSTest
{
    [TestClass]
    public class MailController_Test
    {
        private EmailController _controller;
        private Mock<DataContext> _contextMock;
        private Mock<IConfiguration> _configurationMock;

        [TestInitialize]
        public void Setup()
        {
            _contextMock = new Mock<DataContext>();
            _configurationMock = new Mock<IConfiguration>();
            _controller = new EmailController(_configurationMock.Object, _contextMock.Object);
        }

        [TestMethod]
        public void SendEmail_ReturnsOkResult_WhenUserExists()
        {
            // Arrange
            var request = new Email { userName = "user1", email = "user1@example.com" };
            var userDtos = new List<UserDto>
            {
                new UserDto { Id = 1, UserName = "user1" },
                new UserDto { Id = 2, UserName = "user2" },
                new UserDto { Id = 3, UserName = "user3" }
            };
            _contextMock.Setup(x => x.UserDto).Returns((Delegate)userDtos.AsQueryable());

            // Act
            var result = _controller.SendEmail(request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void SendEmail_ReturnsBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new Email { userName = "user4", email = "user4@example.com" };
            var userDtos = new List<UserDto>
            {
                new UserDto { Id = 1, UserName = "user1", Password = "password1" },
                new UserDto { Id = 2, UserName = "user2", Password = "password2" },
                new UserDto { Id = 3, UserName = "user3", Password = "password3" }
            };
            _contextMock.Setup(x => x.UserDto).Returns((Delegate)userDtos.AsQueryable());

            // Act
            var result = _controller.SendEmail(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be("NO USER MATCH THE DATABASE");
        }
    }
}
