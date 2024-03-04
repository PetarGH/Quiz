using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IRepositories;
using Application.IManagers;
using Infrastructure.Models;

namespace Application.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository userRepository;

        public UserManager(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IpUser GetUserByID(int id)
        {
            IpUser user = userRepository.GetById(id);
            return user;
        }
        public List<IpUser> GetAllUsers()
        {
            List<IpUser> users = userRepository.GetAll();
            return users;
        }
        public async Task<bool> FreezeUser(int id)
        {
            bool result = await userRepository.FreezeAccount(id);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            bool result = await userRepository.DeleteUser(id);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IpUser> LoginAsync(string email, string password)
        {
            IpUser tmp = await userRepository.LoginAsync(email, password);
            if (tmp != null)
            {
                if (tmp.IsFrozen == true)
                {
                    return null;
                }
                return tmp;
            }
            else return null;
        }

        public bool RegisterUser(RegisterModel registerUser)
        {
            IpUser user = new IpUser(registerUser.Name, registerUser.Age, registerUser.Email, registerUser.Password, registerUser.Address, false, false);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.Name.Length != 0 && user.Password.Length >= 3)
            {
                try
                {
                    bool result = userRepository.Add(user);
                    if (result)
                    {
                        return true;
                    }
                    return false;

                }
                catch (Exception ex)
                {

                }
            }
            return false;
        }

        public void UpdateUser(IpUser user)
        {
            userRepository.Update(user);
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            return await userRepository.IsEmailTakenAsync(email);
        }
    }
}
