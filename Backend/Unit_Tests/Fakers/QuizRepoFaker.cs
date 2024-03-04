using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IManagers;
using Application.IRepositories;
using Application.Repositories;
using Application.Managers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Unit_Tests.Fakers
{
    public class QuizRepoFaker : IQuizRepository
    {
        private readonly List<IpQuiz> _quizzes;

        public QuizRepoFaker() 
        {
            _quizzes = new List<IpQuiz>();
        }

        public List<IpQuiz> GetAllQuizzes()
        {
            return _quizzes;
        }
        
        public IpQuiz GetNewestQuiz()
        {
            throw new NotImplementedException();
        }

        public IpQuiz GetQuizWithQuestionsAndAnswers(int quizId)
        {
            throw new NotImplementedException();
        }

        public List<IpQuiz> GetAllQuizzesWithQuestionsAndAnswers()
        {
            throw new NotImplementedException();
        }

        public List<IpQuiz> GetUserQuiz(int userid)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            IpQuiz quiz = new IpQuiz(1, "Title", "Short description", 18, 10);
            _quizzes.Add(quiz);
            if (_quizzes.Count > 0)
            {
                _quizzes.Remove(quiz);
                return true;
            }
            else return false;
        }

        public bool Add(IpQuiz quiz)
        {
            _quizzes.Add(quiz);
            if (_quizzes.Count > 0)
            {
                return true;
            }
            else
                return false;
        }

        public bool Update(IpQuiz quiz)
        {
            throw new NotImplementedException();
        }
    }
}
