using Domain.Entities;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IManagers
{
    public interface IQuestionManager
    {
        public IpQuestion GetQuestionById(int id);
        public List<IpQuestion> GetAllQuestions(int quizid);
        public Task<bool> Delete(int id);
        bool AddQuestion(AddQuestionModel questionModel);
    }
}
