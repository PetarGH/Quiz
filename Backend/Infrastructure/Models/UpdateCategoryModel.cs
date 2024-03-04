using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class UpdateCategoryModel
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
