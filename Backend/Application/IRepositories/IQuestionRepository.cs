using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IQuestionRepository
    {
        public List<IpQuestion> GetAllQuestions(int quizid);
        public Task<bool> Delete(int id);
        public IpQuestion GetById(int id);
        bool Add(IpQuestion question);
    }
}
