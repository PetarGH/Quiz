using Application.IRepositories;
using Domain.Data;
using Domain.Entities;
using Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly Dbi477163Context _context;

        public AnswerRepository(Dbi477163Context context)
        {
            _context = context;
        }

        public List<ResponseAnswerBody> GetAllRightQuizAnswers(int quizId)
        {
            var correctAnswers = _context.IpQuizzes
             .Where(quiz => quiz.Id == quizId)
             .SelectMany(quiz => quiz.IpQuestions)
             .SelectMany(question => question.IpAnswers)
             .Where(answer => answer.IsCorrect)
             .Select(answer => new ResponseAnswerBody
             {
                 Id = answer.Id,
                 Text = answer.Text,
                 IsCorrect = answer.IsCorrect
             })
             .ToList();

            return correctAnswers;
        }

        public List<IpAnswer> GetAllAnswers(int questionId)
        {
            return _context.IpAnswers.Where(answer => answer.QuestionId == questionId).ToList();
        }

        public async Task<bool> Delete(int id)
        {
            var answer = await _context.IpAnswers.FindAsync(id);
            if (answer != null)
            {
                _context.IpAnswers.Remove(answer);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool Add(IpAnswer answer)
        {
            if (answer == null)
            {
                throw new ArgumentNullException(nameof(answer));
            }

            _context.IpAnswers.Add(answer);
            _context.SaveChanges();
            return true;
        }

        public IpAnswer GetById(int id)
        {
            return _context.IpAnswers.Find(id);
        }
    }
}
