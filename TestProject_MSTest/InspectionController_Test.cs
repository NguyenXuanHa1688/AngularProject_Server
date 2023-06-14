using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApp.Controllers;
using WebApp.Data;
using WebApp.Models;

namespace TestProject_MSTest
{
    [TestClass]
    public class InspectionController_Test
    {
        private InspectionsController ?_controller;
        private Mock<DataContext> ?_contextMock;

        [TestInitialize]
        public void Setup()
        {
            _contextMock = new Mock<DataContext>();
            _controller = new InspectionsController(_contextMock.Object);
        }

        [TestMethod]
        public async Task GetInspections_ReturnsOkResult_WhenInspectionsExist()
        {
            // Arrange
            var inspections = new Inspection { Id = 1, Status = "inspection1" };

            _contextMock.Setup(x => x.Inspections).Returns(inspections);

            // Act
            var result = await _controller.GetInspections();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeOfType<Inspection>();
            var value = okResult.Value as Inspection;

            value.Id.Should().Be(inspections.Id);
            value.Status.Should().Be(inspections.Status);

        }

        [TestMethod]
        public async Task GetInspections_ReturnsNotFound_WhenInspectionsDoNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var controller = new InspectionsController(context);
            // Act
            var result = await controller.GetInspections();

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void InspectionExists_ReturnsTrue_WhenInspectionExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var inspection = new Inspection { Id = 1 };
            context.Inspections.Add(inspection);
            context.SaveChanges();
            var controller = new InspectionsController(context);

            // Act
            var result = controller.InspectionExists(inspection.Id);

            // Assert
            result.Should().BeTrue();
        }

    }
}
