using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Response;

namespace Infrastructure.Response
{
    public class ResponseQuestionBody
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int QuizId { get; set; }
        public List<ResponseAnswerBody> Answers { get; set; }
    }
}
