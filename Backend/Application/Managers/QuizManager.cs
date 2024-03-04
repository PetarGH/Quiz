using Application.IManagers;
using Application.IRepositories;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Models;
using Infrastructure.Response;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers
{
    public class QuizManager : IQuizManager
    {
        private readonly IQuizRepository _quizRepository;

        public QuizManager(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public List<IpQuiz> GetAllQuizzes()
        {
            List<IpQuiz> quizzes = _quizRepository.GetAllQuizzes();
            return quizzes;
        }

        public List<ResponseQuizBody> GetQuizzesWithQandA()
        {
            List<IpQuiz> quizzes = _quizRepository.GetAllQuizzesWithQuestionsAndAnswers();

            var quizDtos = quizzes.Select(quiz => new ResponseQuizBody
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                CreatedBy = quiz.CreatedBy,
                CategoryId = quiz.Categoryid,
                Questions = quiz.IpQuestions.Select(question => new ResponseQuestionBody
                {
                    Id = question.Id,
                    Text = question.Text,
                    QuizId = question.QuizId,
                    Answers = question.IpAnswers.Select(answer => new ResponseAnswerBody
                    {
                        Id = answer.Id,
                        Text = answer.Text,
                        IsCorrect = answer.IsCorrect,
                        QuestionId = answer.QuestionId,
                    }).ToList()
                }).ToList()
            }).ToList();

            return quizDtos;
        }

        public IpQuiz GetQuizBodyById(int id)
        {
            IpQuiz quiz = _quizRepository.GetQuizWithQuestionsAndAnswers(id);


            var quizDto = new IpQuiz()
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                CreatedBy = quiz.CreatedBy,
                Categoryid = quiz.Categoryid,
                IpQuestions = quiz.IpQuestions.Select(question => new IpQuestion
                {
                    Id = question.Id,
                    Text = question.Text,
                    QuizId = question.QuizId,
                    IpAnswers = question.IpAnswers.Select(answer => new IpAnswer
                    {
                        Id = answer.Id,
                        Text = answer.Text,
                        IsCorrect = answer.IsCorrect,
                    }).ToList()
                }).ToList()
            };

            return quizDto;
        }

        public List<IpQuiz> GetUserQuizzes(int userid)
        {
            List<IpQuiz> quizzes = _quizRepository.GetUserQuiz(userid);
            return quizzes;
        }

        public IpQuiz GetNewestQuiz()
        {
            IpQuiz newestQuiz = _quizRepository.GetNewestQuiz();
            return newestQuiz;
        }

        public async Task<bool> DeleteQuiz(int id)
        {
            bool result = await _quizRepository.Delete(id);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CreateQuiz(AddQuizModel createModel)
        {
            if (createModel.Title.Length != 0 && createModel.CreatedBy != 0 && createModel.CategoryId != 0 && createModel.Description.Length > 0)
            {
                var quiz = new IpQuiz
                {
                    Title = createModel.Title,
                    Description = createModel.Description,
                    CreatedBy = createModel.CreatedBy,
                    Categoryid = createModel.CategoryId,
                    // Add the questions
                    IpQuestions = createModel.Questions.Select(q => new IpQuestion
                    {
                        Text = q.Text,
                        //Add the answers
                        IpAnswers = q.Answers.Select(a => new IpAnswer
                        {
                            Text = a.Text,
                            IsCorrect = a.IsCorrect
                        }).ToList()
                    }).ToList()
                };

                _quizRepository.Add(quiz);
                return true;
            }

            return false;
        }

        public bool UpdateQuiz(IpQuiz quiz)
        {

            if (_quizRepository.Update(quiz))
            {
                return true;
            }
            return false;
        }
    }
}
