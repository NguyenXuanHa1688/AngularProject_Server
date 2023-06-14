using Moq;
using WebApp.Controllers;
using WebApp.Data;
using WebApp.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace TestProject_MSTest
{
    [TestClass]
    public class UserController_Test
    {
        private UsersController _controller;
        private Mock<DataContext> _contextMock;

        [TestInitialize]
        public void Setup()
        {
            _contextMock = new Mock<DataContext>();
            _controller = new UsersController(_contextMock.Object);
        }

        [TestMethod]
        public async Task GetUser_ReturnsOkResult_WhenUserExists()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, UserName = "user1" },
                new User { Id = 2, UserName = "user2" },
                new User { Id = 3, UserName = "user3" }
            };
            var userDbSetMock = users.AsQueryable().BuildMockDbSet();
            _contextMock.Setup(x => x.User).Returns(userDbSetMock.Object);

            // Act
            var result = await _controller.GetUser();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<User>>>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeOfType<List<User>>();
            var value = okResult.Value as List<User>;
            value.Count.Should().Be(users.Count);
            value[0].Id.Should().Be(users[0].Id);
            value[0].UserName.Should().Be(users[0].UserName);
            value[1].Id.Should().Be(users[1].Id);
            value[1].UserName.Should().Be(users[1].UserName);
            value[2].Id.Should().Be(users[2].Id);
            value[2].UserName.Should().Be(users[2].UserName);
        }

        [TestMethod]
        public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userDbSetMock = new List<User>().AsQueryable().BuildMockDbSet();
            _contextMock.Setup(x => x.User).Returns(userDbSetMock.Object);

            // Act
            var result = await _controller.GetUser();

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}