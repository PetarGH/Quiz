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
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionManager _questionManager;
        public QuestionController(IQuestionManager questionManager)
        {
            _questionManager = questionManager;
        }

        [HttpDelete("DeleteQuestion/{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var result = await _questionManager.Delete(id);

            if (result)
            {
                return Ok("Question deleted successfully.");
            }
            else
            {
                return BadRequest("Something went wrong.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestionById(int id)
        {
            var question = _questionManager.GetQuestionById(id);
            if (question == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Question not found",
                    Code = 404
                });
            }

            var questionDto = new ResponseQuestionBody
            {
                Id = question.Id,
                Text = question.Text,
                QuizId = question.QuizId,
            };

            return Ok(questionDto);
        }

        [HttpGet("GetAllQuestions/{quizId}")]
        public async Task<ActionResult<List<IpQuestion>>> GetAllQuestions(int quizId)
        {
            List<IpQuestion> questions = _questionManager.GetAllQuestions(quizId);
            if (questions.Count == 0)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Questions for this quiz not found",
                    Code = 404
                });
            }

            List<ResponseQuestionBody> questionDtos = questions.Select(question => new ResponseQuestionBody
            {
                Id = question.Id,
                Text = question.Text,
                QuizId = question.QuizId,

            }).ToList();
            return Ok(questionDtos);
        }

        [HttpPost]
        [Route("AddQuestion")]
        public IActionResult AddQuestion([FromBody] AddQuestionModel questionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool result = _questionManager.AddQuestion(questionModel);
            if (result)
            {
                return Ok("Question added successfully.");
            }
            else return BadRequest("Something went wrong!");
        }

    }
}
