using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Response
{
    public class ResponseUserBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool UserType { get; set; }
        public int Age { get; set; }
        public bool IsFrozen { get; set; }
    }
}
