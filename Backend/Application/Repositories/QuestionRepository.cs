using Application.IRepositories;
using Domain.Data;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly Dbi477163Context _context;

        public QuestionRepository(Dbi477163Context context)
        {
            _context = context;
        }

        public List<IpQuestion> GetAllQuestions(int quizid)
        {
            return _context.IpQuestions.Where(question => question.QuizId == quizid).ToList();
        }
        public async Task<bool> Delete(int id)
        {
            var question = await _context.IpQuestions.FindAsync(id);
            if (question != null)
            {
                _context.IpQuestions.Remove(question);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool Add(IpQuestion question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _context.IpQuestions.Add(question);
            _context.SaveChanges();
            return true;
        }

        public IpQuestion GetById(int id)
        {
            return _context.IpQuestions.Find(id);
        }
    }
}
