using Application.IManagers;
using Domain.Entities;
using Infrastructure.ErrorResponse;
using Infrastructure.Models;
using Infrastructure.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerManager _answerManager;
        public AnswerController(IAnswerManager answerManager)
        {
            _answerManager = answerManager;
        }

        [HttpDelete("DeleteAnswer/{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var result = await _answerManager.Delete(id);

            if (result)
            {
                return Ok("Answer deleted successfully.");
            }
            else
            {
                return BadRequest("Something went wrong.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetAnswerById(int id)
        {
            var answer = _answerManager.GetAnswerById(id);
            if (answer == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Answer not found",
                    Code = 404
                });
            }

            var answerDto = new ResponseAnswerBody
            {
                Id = answer.Id,
                Text = answer.Text,
                IsCorrect = answer.IsCorrect,
                QuestionId = answer.QuestionId,
            };

            return Ok(answerDto);
        }

        [HttpGet("GetAllRightQuizAnswers/{quizId}")]
        public async Task<ActionResult<List<ResponseAnswerBody>>> GetAllRightQuizAnswers(int quizId)
        {
            List<ResponseAnswerBody> answers = _answerManager.GetAllRightQuizAnswers(quizId);
            if (answers.Count == 0)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Answers for this quiz were not found",
                    Code = 404
                });
            }

            List<ResponseAnswerBody> answerDtos = answers.Select(question => new ResponseAnswerBody
            {
                Id = question.Id,
                Text = question.Text,
                IsCorrect = question.IsCorrect,
                QuestionId = question.QuestionId,

            }).ToList();
            return Ok(answerDtos);
        }

        [HttpPost]
        [Route("AddAnswer")]
        public IActionResult AddAnswer([FromBody] AddAnswerModel answerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool result = _answerManager.AddAnswer(answerModel);
            if (result)
            {
                return Ok("Answer added successfully.");
            }
            else return BadRequest("Something went wrong!");
        }
    }
}
