using Application.IRepositories;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Tests.Fakers
{
    public class AnswerRepoFaker : IAnswerRepository
    {
        private readonly List<IpAnswer> _answers;

        public AnswerRepoFaker()
        {
            _answers = new List<IpAnswer>();
        }

        public List<IpAnswer> GetAllAnswers(int questionId)
        {
            if (questionId > 0)
            {
                return _answers;
            }
            else { return null; }
        }

        public List<ResponseAnswerBody> GetAllRightQuizAnswers(int quizId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            IpAnswer answer = new IpAnswer("Yes", true, 5);
            _answers.Add(answer);
            if (_answers.Count > 0)
            {
                _answers.Remove(answer);
                return true;
            }
            else return false;
        }

        public bool Add(IpAnswer answer)
        {
            _answers.Add(answer);
            if (_answers.Count > 0)
            {
                return true;
            }
            else
                return false;
        }

        public IpAnswer GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
