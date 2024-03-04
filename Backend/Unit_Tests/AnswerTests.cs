using Application.IManagers;
using Application.Managers;
using Backend.Controllers;
using Domain.Entities;
using Infrastructure.Models;
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
    public class AnswerTests
    {
        IAnswerManager answerManager = new AnswerManager(new AnswerRepoFaker());


        [TestMethod]
        public void TestConstructor()
        {
            //Arrange & Act
            IpAnswer answer = new IpAnswer("Yes", true, 5);
            //Assert
            Assert.AreEqual("Yes", answer.Text);
            Assert.AreEqual(5, answer.QuestionId);
            Assert.AreEqual(true, answer.IsCorrect);
        }

        [TestMethod]
        public async Task DeleteAnswer_ExistingAnswer_Deleted()
        {
            //Arrange
            IpAnswer answer = new IpAnswer(5, "Yes", true, 5);
            //Act
            bool result = await answerManager.Delete(answer.Id);
            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void AddAnswer_Added()
        {
            //Arrange
            AddAnswerModel answerModel = new AddAnswerModel
            {
                Text = "Yes",
                IsCorrect = true,
                QuestionId = 5,
            };
            //Act
            bool result = answerManager.AddAnswer(answerModel);
            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void AddAnswer_WithNoText_NotAdded()
        {
            //Arrange
            AddAnswerModel answerModel = new AddAnswerModel
            {
                Text = "",
                IsCorrect = true,
                QuestionId = 5,
            };

            //Act
            bool result = answerManager.AddAnswer(answerModel);
            //Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestGetAnswer_ByID_Returned()
        {
            //Arrange
            var answer = new IpAnswer
            {
                Id = 5,
                Text = "Test",
                IsCorrect = true,
                QuestionId = 5,
            };
            //Act
            var answerManager = new Mock<IAnswerManager>();
            answerManager.Setup(x => x.GetAnswerById(It.IsAny<int>())).Returns(answer);
            var controller = new AnswerController(answerManager.Object);
            var getAnswerById = controller.GetAnswerById(5);
            //Assert
            Assert.IsNotNull(getAnswerById);
        }

        [TestMethod]
        public async Task AddAnswer_ValidAnswer_ReturnsAnswer()
        {
            //Arrange
            AddAnswerModel answerModel = new AddAnswerModel
            {
                Text = "Yes",
                IsCorrect = true,
                QuestionId = 5,
            };

            //Act
            var answerManager = new Mock<IAnswerManager>();
            answerManager.Setup(x => x.AddAnswer(It.IsAny<AddAnswerModel>())).Returns(true);
            var controller = new AnswerController(answerManager.Object);
            var result = controller.AddAnswer(answerModel);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            OkObjectResult okResult = (OkObjectResult)result;
            Assert.AreEqual("Answer added successfully.", okResult.Value);
        }

        [TestMethod]
        public async Task DeleteAnswer_ValidId_ReturnsOk()
        {
            // Arrange
            int answerId = 123;
            var answerManager = new Mock<IAnswerManager>();
            answerManager.Setup(x => x.Delete(answerId)).ReturnsAsync(true); // Mock to return true for a successful delete
            var controller = new AnswerController(answerManager.Object);

            // Act
            var result = await controller.DeleteAnswer(answerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Answer deleted successfully.", okResult.Value);
        }
    }
}
