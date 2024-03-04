using Application.IManagers;
using Application.IRepositories;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers
{
    public class QuestionManager : IQuestionManager
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionManager(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public IpQuestion GetQuestionById(int id)
        {
            IpQuestion question = _questionRepository.GetById(id);
            return question;
        }
        public List<IpQuestion> GetAllQuestions(int quizid)
        {
            if (quizid > 0)
            {
                List<IpQuestion> questions = _questionRepository.GetAllQuestions(quizid);
                return questions;
            }
            else return null;
        }

        public async Task<bool> Delete(int id)
        {
            bool result = await _questionRepository.Delete(id);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddQuestion(AddQuestionModel questionModel)
        {
            IpQuestion question = new IpQuestion(questionModel.Text, questionModel.QuizId);

            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            if (question.Text.Length != 0 && question.QuizId > 0)
            {
                try
                {
                    bool result = _questionRepository.Add(question);
                    if (result)
                    {
                        return true;
                    }
                    return false;

                }
                catch (Exception ex)
                {

                }
            }
            return false;
        }

    }
}
