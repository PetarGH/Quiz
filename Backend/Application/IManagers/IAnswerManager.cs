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
    public interface IAnswerManager
    {
        public IpAnswer GetAnswerById(int id);
        public List<ResponseAnswerBody> GetAllRightQuizAnswers(int quizId);
        List<IpAnswer> GetAllAnswers(int questionId);
        public Task<bool> Delete(int id);
        public bool AddAnswer(AddAnswerModel answerModel);
    }
}
