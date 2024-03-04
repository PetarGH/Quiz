using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class UpdateQuestionModel
    {
        public string Text { get; set; }

        public List<UpdateAnswerModel> Answers { get; set; }

    }
}
