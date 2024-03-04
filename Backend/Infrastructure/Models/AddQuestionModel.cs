using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class AddQuestionModel
    {
        public string Text { get; set; } 
        public int QuizId { get; set; }
        public List<AddAnswerModel> Answers { get; set; }
    }
}
