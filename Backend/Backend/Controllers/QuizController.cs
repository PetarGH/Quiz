using Application.IManagers;
using Application.Managers;
using Domain.Entities;
using Infrastructure.ErrorResponse;
using Infrastructure.Models;
using Infrastructure.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {

        private readonly IQuizManager _quizManager;
        public QuizController(IQuizManager quizManager)
        {
            _quizManager = quizManager;
        }

        [HttpDelete("DeleteQuiz/{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var result = await _quizManager.DeleteQuiz(id);

            if (result)
            {
                return Ok("Quiz deleted successfully.");
            }
            else
            {
                return BadRequest("Something went wrong.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<IpQuiz>>> GetAllQuizzes()
        {
            List<IpQuiz> quizzes = _quizManager.GetAllQuizzes();
            if (quizzes == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Quizzes not found",
                    Code = 404
                });
            }

            List<ResponseQuizBody> quizDtos = quizzes.Select(quiz => new ResponseQuizBody
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                CreatedBy = quiz.CreatedBy,
                CategoryId = quiz.Categoryid,

            }).ToList();
            return Ok(quizDtos);
        }

        [HttpGet("GetUserQuizzes/{userId}")]
        public async Task<ActionResult<List<IpQuiz>>> GetUserQuizzes(int userId)
        {
            List<IpQuiz> quizzes = _quizManager.GetUserQuizzes(userId);
            if (quizzes.Count == 0)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Quizzes not found",
                    Code = 404
                });
            }

            List<ResponseQuizBody> quizDtos = quizzes.Select(quiz => new ResponseQuizBody
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                CreatedBy = quiz.CreatedBy,
                CategoryId = quiz.Categoryid,

            }).ToList();
            return Ok(quizDtos);
        }

        [HttpGet("GetNewestQuiz")]
        public async Task<ActionResult<IpQuiz>> GetNewestQuiz()
        {
            IpQuiz newestQuiz = _quizManager.GetNewestQuiz();
            if (newestQuiz == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Quiz not found",
                    Code = 404
                });
            }

            var quiz = new ResponseQuizBody
            {
                Id = newestQuiz.Id,
                Title = newestQuiz.Title,
                Description = newestQuiz.Description,
                CategoryId = newestQuiz.Categoryid,
                CreatedBy = newestQuiz.CreatedBy,
            };

            return Ok(quiz);
        }

        [HttpGet("GetQuizzesQandA")]
        public async Task<ActionResult<List<ResponseQuizBody>>> GetQuizzesWithQandA()
        {
            List<ResponseQuizBody> quizzes = _quizManager.GetQuizzesWithQandA();
            if (quizzes.Count == 0)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Quizzes not found",
                    Code = 404
                });
            }
            return Ok(quizzes);
        }

        [HttpGet("GetQuizQandA/{id}")]
        public async Task<ActionResult<IpQuiz>> GetQuizById(int id)
        {
            IpQuiz quiz = _quizManager.GetQuizBodyById(id);
            if (quiz == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Quizzes not found",
                    Code = 404
                });
            }
            return Ok(quiz);
        }


        [HttpPut("Update/{id}")]
        public IActionResult UpdateQuiz(int id, [FromBody] UpdateQuizModel quizModel)
        {
            var existingQuiz = _quizManager.GetQuizBodyById(id);
            if (existingQuiz == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Quiz not found",
                    Code = 404
                });
            }
            try
            {
                // Update existingQuiz properties
                existingQuiz.Title = quizModel.Title;
                existingQuiz.Description = quizModel.Description;
                existingQuiz.Categoryid = quizModel.CategoryId;

                // Update or add questions
                for (int i = 0; i < quizModel.Questions.Count; i++)
                {
                    var updatedQuestion = quizModel.Questions[i];
                    var existingQuestion = existingQuiz.IpQuestions.ElementAtOrDefault(i);

                    if (existingQuestion != null)
                    {
                        // Update existing question
                        existingQuestion.Text = updatedQuestion.Text;

                        // Update or add answers for the question
                        for (int j = 0; j < updatedQuestion.Answers.Count; j++)
                        {
                            var updatedAnswer = updatedQuestion.Answers[j];
                            var existingAnswer = existingQuestion.IpAnswers.ElementAtOrDefault(j);

                            if (existingAnswer != null)
                            {
                                // Update existing answer
                                existingAnswer.Text = updatedAnswer.Text;
                                existingAnswer.IsCorrect = updatedAnswer.IsCorrect;
                            }
                            else
                            {
                                // Add new answer
                                existingQuestion.IpAnswers.Add(new IpAnswer
                                {
                                    Text = updatedAnswer.Text,
                                    IsCorrect = updatedAnswer.IsCorrect,
                                });
                            }
                        }
                    }
                    else
                    {
                        // Add new question
                        existingQuiz.IpQuestions.Add(new IpQuestion
                        {
                            Text = updatedQuestion.Text,
                            // Add or update answers for the new question
                            IpAnswers = updatedQuestion.Answers.Select(updatedAnswer => new IpAnswer
                            {
                                Text = updatedAnswer.Text,
                                IsCorrect = updatedAnswer.IsCorrect,
                            }).ToList(),
                        });
                    }
                }

                _quizManager.UpdateQuiz(existingQuiz);
                return Ok("Quiz is updated successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message,
                    Code = 4005
                });
            }

        }

        [HttpPost]
        [Route("CreateQuiz")]
        public IActionResult Create([FromBody] AddQuizModel createModel)
        {
            if (_quizManager.CreateQuiz(createModel))
            {
                return Ok("Quiz created successfully.");
            }

            return BadRequest("Failed to create quiz");
        }
    }
}
