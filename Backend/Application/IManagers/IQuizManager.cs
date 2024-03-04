using Domain.Entities;
using Infrastructure.Models;
using Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IManagers
{
    public interface IQuizManager
    {
        public List<IpQuiz> GetAllQuizzes();
        public Task<bool> DeleteQuiz(int id);
        bool UpdateQuiz(IpQuiz quiz);
        List<IpQuiz> GetUserQuizzes(int userid);
        List<ResponseQuizBody> GetQuizzesWithQandA();
        bool CreateQuiz(AddQuizModel createModel);
        IpQuiz GetQuizBodyById(int id);
        IpQuiz GetNewestQuiz();
    }
}
