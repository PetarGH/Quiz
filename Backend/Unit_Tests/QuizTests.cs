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
    public class QuizTests
    {
        IQuizManager quizManager = new QuizManager(new QuizRepoFaker());

        [TestMethod]
        public void TestConstructor()
        {
            //Arrange & Act
            IpQuiz quiz = new IpQuiz("Title" , "Short description", 18, 10);
            //Assert
            Assert.AreEqual("Title", quiz.Title);
            Assert.AreEqual(18, quiz.CreatedBy);
            Assert.AreEqual("Short description", quiz.Description);
            Assert.AreEqual(10, quiz.Categoryid);
        }

        [TestMethod]
        public async Task DeleteQuiz_ExistingQuiz_Deleted()
        {
            //Arrange
            IpQuiz quiz = new IpQuiz(1, "Title", "Short description", 18, 10);
            //Act
            bool result = await quizManager.DeleteQuiz(quiz.Id);
            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void AddQuiz_Added()
        {
            //Arrange
            var quiz = new AddQuizModel
            {
                Title = "Valid Title",
                Description = "Valid Description",
                CreatedBy = 1,
                CategoryId = 2,
                Questions = new List<AddQuestionModel>
            {
                new AddQuestionModel
                {
                    Text = "Question 1",
                    Answers = new List<AddAnswerModel>
                    {
                        new AddAnswerModel { Text = "Answer 1", IsCorrect = true },
                        new AddAnswerModel { Text = "Answer 2", IsCorrect = false }
                    }
                }
            }
            };
            //Act
            bool result = quizManager.CreateQuiz(quiz);
            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void AddQuiz_WithNoTitle_NotAdded()
        {
            //Arrange
            var quiz = new AddQuizModel
            {
                Title = "",
                Description = "Short description",
                CreatedBy = 18,
                CategoryId = 10,
            };

            //Act
            bool result = quizManager.CreateQuiz(quiz);
            //Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestGetQuuiz_ByID_Returned()
        {
            //Arrange
            IpQuiz quiz = new IpQuiz(5, "Title", "Short description", 18, 10);
            //Act
            var quizManager = new Mock<IQuizManager>();
            quizManager.Setup(x => x.GetQuizBodyById(It.IsAny<int>())).Returns(quiz);
            var controller = new QuizController(quizManager.Object);
            var getQuizById = controller.GetQuizById(5);
            //Assert
            Assert.IsNotNull(getQuizById);
        }

        [TestMethod]
        public async Task AddQuiz_ValidQuiz_ReturnsQuiz()
        {
            //Arrange
            var quiz = new AddQuizModel
            {
                Title = "",
                Description = "Short description",
                CreatedBy = 18,
                CategoryId = 10,
            };

            //Act
            var quizManager = new Mock<IQuizManager>();
            quizManager.Setup(x => x.CreateQuiz(It.IsAny<AddQuizModel>())).Returns(true);
            var controller = new QuizController(quizManager.Object);
            var result = controller.Create(quiz);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            OkObjectResult okResult = (OkObjectResult)result;
            Assert.AreEqual("Quiz created successfully.", okResult.Value);
        }

        [TestMethod]
        public async Task DeleteQuiz_ValidId_ReturnsOk()
        {
            // Arrange
            int quizId = 123;
            var quizManager = new Mock<IQuizManager>();
            quizManager.Setup(x => x.DeleteQuiz(quizId)).ReturnsAsync(true); // Mock to return true for a successful delete
            var controller = new QuizController(quizManager.Object);

            // Act
            var result = await controller.DeleteQuiz(quizId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Quiz deleted successfully.", okResult.Value);
        }
    }
}
