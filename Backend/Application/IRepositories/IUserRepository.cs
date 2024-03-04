using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IUserRepository
    {
        public List<IpUser> GetAll();
        public IpUser GetById(int id);
        public Task<bool> FreezeAccount(int id);
        public Task<bool> DeleteUser(int id);
        public bool Add(IpUser user);
        public void Update(IpUser user);
        public Task<IpUser> LoginAsync(string email, string password);
        public Task<bool> IsEmailTakenAsync(string email);
    }
}
