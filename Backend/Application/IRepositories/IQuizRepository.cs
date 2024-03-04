using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Data;
using Domain.Entities;
using Infrastructure.Models;
using Infrastructure.Response;

namespace Application.IRepositories
{
    public interface IQuizRepository
    {
        List<IpQuiz> GetAllQuizzes();
        Task<bool> Delete(int id);
        bool Add(IpQuiz quiz);
        bool Update(IpQuiz quiz);
        List<IpQuiz> GetUserQuiz(int userid);
        List<IpQuiz> GetAllQuizzesWithQuestionsAndAnswers();
        IpQuiz GetQuizWithQuestionsAndAnswers(int quizId);
        IpQuiz GetNewestQuiz();
    }
}
