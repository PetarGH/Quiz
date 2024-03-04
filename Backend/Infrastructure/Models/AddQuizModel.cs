using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class AddQuizModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public int CategoryId { get; set; } 
        public List<AddQuestionModel> Questions { get; set; }
    }
}
