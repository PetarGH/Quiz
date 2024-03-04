using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Tests.Fakers
{
    public class QuestionRepoFaker : IQuestionRepository
    {
        private readonly List<IpQuestion> _questions;

        public QuestionRepoFaker()
        {
            _questions = new List<IpQuestion>();
        }

        public List<IpQuestion> GetAllQuestions(int quizid)
        {
            return _questions;
        }
        public async Task<bool> Delete(int id)
        {
            IpQuestion question = new IpQuestion(1, "Test", 5);
            _questions.Add(question);
            if (_questions.Count > 0)
            {
                _questions.Remove(question);
                return true;
            }
            else return false;
        }

        public bool Add(IpQuestion question)
        {
            _questions.Add(question);
            if (_questions.Count > 0)
            {
                return true;
            }
            else
                return false;
        }

        public IpQuestion GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
