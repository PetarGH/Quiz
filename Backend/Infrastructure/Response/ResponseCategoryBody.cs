using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Response
{
    public class ResponseCategoryBody
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
