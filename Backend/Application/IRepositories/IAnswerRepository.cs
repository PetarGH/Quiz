using Domain.Entities;
using Infrastructure.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IAnswerRepository
    {
        public List<ResponseAnswerBody> GetAllRightQuizAnswers(int quizId);
        List<IpAnswer> GetAllAnswers(int questionId);
        public Task<bool> Delete(int id);
        public IpAnswer GetById(int id);
        bool Add(IpAnswer answer);
    }
}
