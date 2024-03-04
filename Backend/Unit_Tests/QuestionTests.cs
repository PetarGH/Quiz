using Application.IManagers;
using Application.Managers;
using Backend.Controllers;
using Domain.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Unit_Tests.Fakers;

namespace Unit_Tests
{
    [TestClass]
    public class QuestionTests
    {
        IQuestionManager questionManager = new QuestionManager(new QuestionRepoFaker());

        [TestMethod]
        public void TestConstructor()
        {
            //Arrange & Act
            IpQuestion question = new IpQuestion("Test", 5);
            //Assert
            Assert.AreEqual("Test", question.Text);
            Assert.AreEqual(5, question.QuizId);
        }

        [TestMethod]
        public async Task DeleteQuestion_ExistingQuestion_Deleted()
        {
            //Arrange
            IpQuestion question = new IpQuestion(1, "Test", 5);
            //Act
            bool result = await questionManager.Delete(question.Id);
            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void AddQuestion_Added()
        {
            //Arrange
            AddQuestionModel questionModel = new AddQuestionModel
            {
                Text = "Test",
                QuizId = 5
            };
            //Act
            bool result = questionManager.AddQuestion(questionModel);
            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void AddQuesting_WithNoText_NotAdded()
        {
            //Arrange
            AddQuestionModel questionModel = new AddQuestionModel
            {
                Text = "",
                QuizId = 5
            };

            //Act
            bool result = questionManager.AddQuestion(questionModel);
            //Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestGetQuestion_ByID_Returned()
        {
            //Arrange
            var question = new IpQuestion
            {
                Id = 5,
                Text = "Test",
                QuizId = 5
            };
            //Act
            var questionManager = new Mock<IQuestionManager>();
            questionManager.Setup(x => x.GetQuestionById(It.IsAny<int>())).Returns(question);
            var controller = new QuestionController(questionManager.Object);
            var getQuestionById = controller.GetQuestionById(5);
            //Assert
            Assert.IsNotNull(getQuestionById);
        }

        [TestMethod]
        public async Task AddQuestion_ValidQuestion_ReturnsQuestion()
        {
            //Arrange
            AddQuestionModel questionModel = new AddQuestionModel
            {
                Text = "Test",
                QuizId = 5
            };

            //Act
            var questionManager = new Mock<IQuestionManager>();
            questionManager.Setup(x => x.AddQuestion(It.IsAny<AddQuestionModel>())).Returns(true);
            var controller = new QuestionController(questionManager.Object);
            var result = controller.AddQuestion(questionModel);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            OkObjectResult okResult = (OkObjectResult)result;
            Assert.AreEqual("Question added successfully.", okResult.Value);
        }

        [TestMethod]
        public async Task DeleteQuestion_ValidId_ReturnsOk()
        {
            // Arrange
            int questionId = 123;
            var questionManager = new Mock<IQuestionManager>();
            questionManager.Setup(x => x.Delete(questionId)).ReturnsAsync(true); // Mock to return true for a successful delete
            var controller = new QuestionController(questionManager.Object);

            // Act
            var result = await controller.DeleteQuestion(questionId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Question deleted successfully.", okResult.Value);
        }
    }
}
