using Application.IManagers;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Models;
using Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers
{
    public class AnswerManager : IAnswerManager
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerManager(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public IpAnswer GetAnswerById(int id)
        {
            IpAnswer answer = _answerRepository.GetById(id);
            return answer;
        }
        public List<ResponseAnswerBody> GetAllRightQuizAnswers(int quizId)
        {
            if (quizId > 0)
            {
                List<ResponseAnswerBody> answers = _answerRepository.GetAllRightQuizAnswers(quizId);
                return answers;
            }
            else return null;
        }

        public List<IpAnswer> GetAllAnswers(int questionId)
        {
            if (questionId > 0)
            {
                List<IpAnswer> answers = _answerRepository.GetAllAnswers(questionId);
                return answers;
            }
            else return null;
        }

        public async Task<bool> Delete(int id)
        {
            bool result = await _answerRepository.Delete(id);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddAnswer(AddAnswerModel answerModel)
        {
            IpAnswer answer = new IpAnswer(answerModel.Text, answerModel.IsCorrect, answerModel.QuestionId);

            if (answer == null)
            {
                throw new ArgumentNullException(nameof(answer));
            }
            if (answer.Text.Length != 0 && answer.QuestionId > 0)
            {
                try
                {
                    bool result = _answerRepository.Add(answer);
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
