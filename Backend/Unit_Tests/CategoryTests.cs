using Application.IManagers;
using Application.Managers;
using Backend.Controllers;
using Domain.Entities;
using Infrastructure.Models;
using Infrastructure.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit_Tests.Fakers;

namespace Unit_Tests
{
    [TestClass]
    public class CategoryTests
    {
        ICategoryManager categoryManager = new CategoryManager(new CategoryRepoFaker());

        [TestMethod]
        public void TestConstructor()
        {
            //Arrange & Act
            IpCategory category = new IpCategory("Fun");
            //Assert
            Assert.AreEqual("Fun", category.Name);
        }

        [TestMethod]
        public async Task DeleteCategory_ExistingCategory_Deleted()
        {
            //Arrange
            IpCategory category = new IpCategory("Food", 1);
            //Act
            bool result = await categoryManager.DeleteCategory(category.Id);
            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public async Task AddCategory()
        {
            //Arrange
            AddCategoryModel categoryModel = new AddCategoryModel
            {
                Name = "John",
            };
            //Act
            bool result = await categoryManager.AddCategory(categoryModel);
            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public async Task AddCategory_WithNoName_NotAdded()
        {
            //Arrange
            AddCategoryModel categoryModel = new AddCategoryModel
            {
                Name = "",
            };

            //Act
            bool result = await categoryManager.AddCategory(categoryModel);
            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task AddSubCategory_Added()
        {
            //Arrange
            AddCategoryModel categoryModel = new AddCategoryModel
            {
                Name = "Dog",
                ParentId = 7
            };
            //Act
            bool result = await categoryManager.AddCategory(categoryModel);
            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void CheckIfListIsEmpty_AfterDeletingCategory_Empty()
        {
            //Arrange
            IpCategory category = new IpCategory(1, "Fun");
            //Act
            categoryManager.DeleteCategory(category.Id);
            List<IpCategory> categories = categoryManager.GetAllCategories();
            //Assert
            CollectionAssert.DoesNotContain(categories, category);
            Assert.IsFalse(categories.Count > 0);
        }


        /// <summary>
        /// Controller tests
        /// </summary>

        [TestMethod]
        public void TestGetCategory_ByID_Returned()
        {
            //Arrange
            var category = new IpCategory
            {
                Id = 1,
                Name = "Fun",
            };
            //Act
            var categoryManager = new Mock<ICategoryManager>();
            categoryManager.Setup(x => x.GetCategoryById(It.IsAny<int>())).Returns(category);
            var controller = new CategoryController(categoryManager.Object);
            var getCategoryById = controller.GetCategoryById(1);
            //Assert
            Assert.IsNotNull(getCategoryById);
            Assert.IsInstanceOfType(getCategoryById, typeof(OkObjectResult));

            // Extract the value from OkObjectResult
            var okObjectResult = (OkObjectResult)getCategoryById;
            var actualCategory = okObjectResult.Value as ResponseCategoryBody;

            // Compare relevant properties
            Assert.AreEqual(category.Id, actualCategory.Id);
            Assert.AreEqual(category.Name, actualCategory.Name);
        }

        [TestMethod]
        public async Task TestGetCategories_ReturnListOfCategories()
        {
            //Arrange
            List<IpCategory> categories = new List<IpCategory>
            {

                new IpCategory(1, "Fun"),
                new IpCategory(2, "Business"),
                new IpCategory(3, "Food")
            };
            //Act
            var categoryManager = new Mock<ICategoryManager>();
            categoryManager.Setup(x => x.GetAllCategories()).Returns(categories);
            var controller = new CategoryController(categoryManager.Object);
            var result = await controller.GetAllCategories();
            //Assert
            var objectResult = result.Result as ObjectResult;
            var allCategories = objectResult.Value as List<ResponseCategoryBody>;

            Assert.IsNotNull(allCategories);
            Assert.AreEqual(3, allCategories.Count);
        }

        [TestMethod]
        public async Task Add_ValidCategory_ReturnsCategory()
        {
            //Arrange
            AddCategoryModel categoryModel = new AddCategoryModel
            {
                Name = "Test",
            };

            //Act
            var categoryManager = new Mock<ICategoryManager>();
            categoryManager.Setup(x => x.AddCategory(It.IsAny<AddCategoryModel>())).ReturnsAsync(true);
            var controller = new CategoryController(categoryManager.Object);
            var result = await controller.CreateCategory(categoryModel);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            OkObjectResult okResult = (OkObjectResult)result;
            Assert.AreEqual("Category created successfully", okResult.Value);
        }

        [TestMethod]
        public async Task DeleteCategory_ValidId_ReturnsOk()
        {
            // Arrange
            int categoryId = 123;
            var categoryManager = new Mock<ICategoryManager>();
            categoryManager.Setup(x => x.DeleteCategory(categoryId)).ReturnsAsync(true); // Mock to return true for a successful delete
            var controller = new CategoryController(categoryManager.Object);

            // Act
            var result = await controller.DeleteCategory(categoryId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Category deleted successfully", okResult.Value);
        }

        [TestMethod]
        public async Task DeleteCategory_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            int categoryId = 456;
            var categoryManager = new Mock<ICategoryManager>();
            categoryManager.Setup(x => x.DeleteCategory(categoryId)).ReturnsAsync(false); // Mock to return false for an unsuccessful delete
            var controller = new CategoryController(categoryManager.Object);

            // Act
            var result = await controller.DeleteCategory(categoryId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Something went wrong.", badRequestResult.Value);
        }
    }
}
