using Application.IRepositories;
using Domain.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Unit_Tests.Fakers
{
    public class UserRepoFaker : IUserRepository
    {
        private readonly List<IpUser> _users;

        public UserRepoFaker()
        {
            _users = new List<IpUser>();
        }

        public List<IpUser> GetAll()
        {
            return _users.ToList();
        }

        public IpUser GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> FreezeAccount(int id)
        {
            //get user by the id ( create a fake user )
            IpUser user = new IpUser("Test", 22, "test@gmail.com", "123", "Center", false, false);
            user.IsFrozen = true;
            if (user.IsFrozen)
            {
                return true;
            }
            else return false;

        }

        public async Task<bool> DeleteUser(int id)
        {
            //get user by the id ( create a fake user )
            IpUser user = new IpUser("Ivan", 21, "ivan@email.com", "1234", "Center", false, false);
            _users.Add(user);
            if (_users.Count > 0)
            {
                _users.Remove(user);
                return true;
            }
            else return false;
        }

        public bool Add(IpUser user)
        {
            _users.Add(user);
            if (_users.Count > 0)
            {
                return true;
            }
            else
                return false;
        }

        public void Update(IpUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IpUser> LoginAsync(string email, string password)
        {
            _users.Add(await LoginAsync(email, password));
            return _users[0];
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            throw new NotImplementedException();
        }

    }
}
