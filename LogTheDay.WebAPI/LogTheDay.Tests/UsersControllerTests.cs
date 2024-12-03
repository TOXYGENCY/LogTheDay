using LogTheDay.LogTheDay.WebAPI.Controllers;
using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LogTheDay.Tests
{
    //[TestClass]
    //public class UsersControllerTests // Пока для UsersCRUDController
    //{
    //    [TestMethod]
    //    public async Task GetAllAsync_Success()
    //    {
    //        //Arrange
    //        var ienumUser = new List<User>{
    //        new User
    //        {
    //            Id = Guid.NewGuid(),
    //            Name = "User1"
    //        }
    //    };
    //        //var serviceMock = new Mock<IUsersService>();
    //        //serviceMock.Setup(x => x.GetAllAsync(It.IsAny<>());
    //        var loggerMock = new Mock<ILogger<UsersCRUDController>>(); // TODO: Впоследствии убрать CRUD
    //        var usersRepositoryMock = new Mock<IUsersRepository>();
    //        usersRepositoryMock.Setup(u => u.GetAllAsync()).ReturnsAsync(ienumUser);

    //        //Act
    //        var controller = new UsersCRUDController(usersRepositoryMock.Object, loggerMock.Object); // TODO: Впоследствии убрать CRUD
    //        var result = await controller.GetAllAsync();

    //        //Assert
    //        Assert.IsInstanceOfType<OkObjectResult>(result);
    //    }

    //    [TestMethod]
    //    public async Task GetAllAsync_Status500()
    //    {
    //        //Arrange
    //        //var serviceMock = new Mock<IUsersService>();
    //        //serviceMock.Setup(x => x.GetAllAsync(It.IsAny<>());
    //        var loggerMock = new Mock<ILogger<UsersCRUDController>>(); // TODO: Впоследствии убрать CRUD
    //        var usersRepositoryMock = new Mock<IUsersRepository>();
    //        usersRepositoryMock.Setup(u => u.GetAllAsync()).ThrowsAsync(new Exception());

    //        //Act
    //        var controller = new UsersCRUDController(usersRepositoryMock.Object, loggerMock.Object); // TODO: Впоследствии убрать CRUD
    //        var result = await controller.GetAllAsync();

    //        //Assert
    //        Assert.IsInstanceOfType<ObjectResult>(result);
    //        Assert.AreEqual(StatusCodes.Status500InternalServerError, (result as ObjectResult)?.StatusCode);
    //    }
    //}
}