using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IHelpers
{
    public interface IJwtService
    {
        public string Generate(int id);
        public JwtSecurityToken Verfiy(string jwt);
    }
}
