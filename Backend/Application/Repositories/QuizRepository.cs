using Domain.Data;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IRepositories;
using Infrastructure.Response;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;

namespace Application.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly Dbi477163Context _context;

        public QuizRepository(Dbi477163Context context)
        {
            _context = context;
        }

        public List<IpQuiz> GetAllQuizzes()
        {
            return _context.IpQuizzes.ToList();
        }

        public IpQuiz GetQuizWithQuestionsAndAnswers(int quizId)
        {
            var quiz = _context.IpQuizzes.Include(quiz => quiz.IpQuestions).ThenInclude(question => question.IpAnswers).FirstOrDefault(quiz => quiz.Id == quizId);

            if (quiz == null)
            {
                return null;
            }
            return quiz;
        }

        public IpQuiz GetNewestQuiz()
        {
            var quiz = _context.IpQuizzes.OrderByDescending(quiz => quiz.Id).FirstOrDefault();
            if (quiz == null)
            {
                return null;
            }
            return quiz;
        }

        public List<IpQuiz> GetAllQuizzesWithQuestionsAndAnswers()
        {
            var quizzes = _context.IpQuizzes.Include(quiz => quiz.IpQuestions).ThenInclude(question => question.IpAnswers).ToList();
            return quizzes;
        }

        public List<IpQuiz> GetUserQuiz(int userid) 
        {
            return _context.IpQuizzes.Where(quiz => quiz.CreatedBy == userid).ToList();
        }

        public async Task<bool> Delete(int id)
        {
            var quiz = await _context.IpQuizzes.FindAsync(id);
            if (quiz != null)
            {
                _context.IpQuizzes.Remove(quiz);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool Add(IpQuiz quiz)
        {
            if (quiz == null)
            {
                throw new ArgumentNullException(nameof(quiz));
            }

            _context.IpQuizzes.Add(quiz);
            _context.SaveChanges();
            return true;
        }

        public bool Update(IpQuiz quiz)
        {
            if (quiz == null)
            {
                throw new ArgumentNullException(nameof(quiz));
            }

            try
            {
                // Check if the entity is already tracked
                var existingEntity = _context.Set<IpQuiz>().Local.FirstOrDefault(e => e.Id == quiz.Id);

                if (existingEntity == null)
                {
                    // If not tracked, attach the entity
                    _context.Attach(quiz);
                    _context.Entry(quiz).State = EntityState.Modified;
                }
                else
                {
                    // If tracked, update the tracked entity
                    _context.Entry(existingEntity).CurrentValues.SetValues(quiz);
                    existingEntity.IpQuestions = quiz.IpQuestions;
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
