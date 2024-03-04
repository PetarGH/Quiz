using Application.IManagers;
using Application.Managers;
using Unit_Tests.Fakers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Infrastructure.Response;
using static System.Net.Mime.MediaTypeNames;

namespace Unit_Tests
{
    [TestClass]
    public class UserTests
    {
        /// <summary>
        /// Manager tests
        /// </summary>

        IUserManager userManager = new UserManager(new UserRepoFaker());

        [TestMethod]
        public void TestConstructor()
        {
            //Arrange & Act
            IpUser user = new IpUser("John", 23, "John@email.com", "1234", "Somewhere", false, false);
            //Assert
            Assert.AreEqual("John", user.Name);
            Assert.AreEqual(23, user.Age);
            Assert.AreEqual("1234", user.Password);
        }

        [TestMethod]
        public async Task DeleteUser_ExistingUser_Deleted()
        {
            //Arrange
            IpUser user = new IpUser(3, "Ivan", 21, "Ivan@email.com", "1234", "Center", false, false);
            //Act
            bool result = await userManager.DeleteUser(user.Id);
            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void RegisterUser()
        {
            //Arrange
            RegisterModel registerUser = new RegisterModel
            {
                Name = "John",
                Age = 31,
                Email = "Jhon@email.com",
                Password = "123",
                Address = "Center"
            };
            //Act
            bool result = userManager.RegisterUser(registerUser);
            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void RegisterUser_WithNoName_NotRegistered()
        {
            //Arrange
            RegisterModel registerUser = new RegisterModel
            {
                Name = "",
                Age = 31,
                Email = "Jhon@email.com",
                Password = "123",
                Address = "Center"
            };

            //Act
            bool result = userManager.RegisterUser(registerUser);
            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RegisterUser_WithPasswordLengthLessThan3_NotRegistered()
        {
            //Arrange
            RegisterModel registerUser = new RegisterModel
            {
                Name = "John",
                Age = 31,
                Email = "John@email.com",
                Password = "12",
                Address = "Center"
            };
            //Act
            bool result = userManager.RegisterUser(registerUser);
            //Assert
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void CheckIfListIsEmpty_AfterDeletingUser_Empty()
        {
            //Arrange
            IpUser user = new IpUser(3 ,"Ivan", 21, "ivan@email.com", "1234", "Center", false, false);
            //Act
            userManager.DeleteUser(user.Id);
            List<IpUser> users = userManager.GetAllUsers();
            //Assert
            CollectionAssert.DoesNotContain(users, user);
            Assert.IsFalse(users.Count > 0);
        }

        /// <summary>
        /// Controller tests
        /// </summary>

        [TestMethod]
        public void TestGetUser_ByID_Returned()
        {
            //Arrange
            var user = new IpUser
            {
                Id = 1,
                Name = "John",
                Age = 22,
                Email = "John34@gmail.com",
                Address = "Center",
                Password = "123",
                UserType = false,
                IsFrozen = false,
            };
            //Act
            var userManager = new Mock<IUserManager>();
            userManager.Setup(x => x.GetUserByID(It.IsAny<int>())).Returns(user);
            var controller = new UserController(null, userManager.Object, null);
            var getUserById = controller.GetUser(1);
            //Assert
            Assert.IsNotNull(getUserById);
        }

        [TestMethod]
        public async Task TestGetUsers_ReturnListOfUsers()
        {
            //Arrange
            List<IpUser> users = new List<IpUser>
            {

                new IpUser(1, "John", 22, "John22@gmail.com", "123", "Center", false, true),
                new IpUser(2, "John2", 23, "John23@gmail.com", "123", "Center", false, false),
                new IpUser(3, "John3", 24, "John24@gmail.com", "123", "Center", false, false)
            };
            //Act
            var userManager = new Mock<IUserManager>();
            userManager.Setup(x => x.GetAllUsers()).Returns(users);
            var controller = new UserController(null, userManager.Object, null);
            var result = await controller.GetAllUsers();
            //Assert
            var objectResult = result.Result as ObjectResult;
            var allUsers = objectResult.Value as List<ResponseUserBody>;

            Assert.IsNotNull(allUsers);
            Assert.AreEqual(3, allUsers.Count);
        }

        [TestMethod]
        public async Task Register_ValidUser_ReturnsUser()
        {
            //Arrange
            RegisterModel registerUser = new RegisterModel
            {
                Name = "Test",
                Age = 31,
                Email = "Jhon@email.com",
                Password = "123",
                Address = "Center"
            };

            //Act
            var userManager = new Mock<IUserManager>();
            userManager.Setup(x => x.RegisterUser(It.IsAny<RegisterModel>())).Returns(true);
            var controller = new UserController(null, userManager.Object, null);
            var result = await controller.RegisterUser(registerUser);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            OkObjectResult okResult = (OkObjectResult)result;
            Assert.AreEqual("Registered successfully.", okResult.Value);
        }

        [TestMethod]
        public async Task DeleteUser_ValidId_ReturnsOk()
        {
            // Arrange
            int userId = 123;
            var userManager = new Mock<IUserManager>();
            userManager.Setup(x => x.DeleteUser(userId)).ReturnsAsync(true); // Mock to return true for a successful delete
            var controller = new UserController(null, userManager.Object, null);

            // Act
            var result = await controller.DeleteUser(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Your account is deleted!", okResult.Value);
        }

        [TestMethod]
        public async Task DeleteUser_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            int userId = 456;
            var userManager = new Mock<IUserManager>();
            userManager.Setup(x => x.DeleteUser(userId)).ReturnsAsync(false); // Mock to return false for an unsuccessful delete
            var controller = new UserController(null, userManager.Object, null);

            // Act
            var result = await controller.DeleteUser(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Something went wrong.", badRequestResult.Value);
        }



    }

}

