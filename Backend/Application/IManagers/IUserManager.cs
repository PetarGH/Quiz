using Domain.Entities;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IManagers
{
    public interface IUserManager
    {
        public IpUser GetUserByID(int id);
        public List<IpUser> GetAllUsers();
        public Task<bool> FreezeUser(int id);
        public Task<bool> DeleteUser(int id);
        public Task<IpUser> LoginAsync(string email, string password);
        public bool RegisterUser(RegisterModel registerUser);
        public void UpdateUser(IpUser user);
        public Task<bool> IsEmailTaken(string email);
    }
}
