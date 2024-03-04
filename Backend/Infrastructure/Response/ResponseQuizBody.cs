using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Response;

namespace Infrastructure.Response
{
    public class ResponseQuizBody
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int CreatedBy { get; set; }
        public List<ResponseQuestionBody> Questions { get; set; }
    }
}
