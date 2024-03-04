using Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using Application.IRepositories;

namespace Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Dbi477163Context _context;

        public UserRepository(Dbi477163Context context)
        {
            _context = context;
        }

        public List<IpUser> GetAll()
        {
            return _context.IpUsers.ToList();
        }

        public IpUser GetById(int id)
        {
            return _context.IpUsers.Find(id);
        }

        public async Task<bool> FreezeAccount(int id)
        {
            var user = await _context.IpUsers.FindAsync(id);

            if (user != null)
            {
                if (user.IsFrozen == true)
                {
                    user.IsFrozen = false;
                }
                else
                {
                    user.IsFrozen = true;
                }
                await _context.SaveChangesAsync();
                return true; // Return true to indicate success
            }

            return false;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.IpUsers.FindAsync(id);

            if (user != null)
            {
                _context.IpUsers.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool Add(IpUser user)
        {

            //password hashing
            string randomSalt = BCrypt.Net.BCrypt.GenerateSalt();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, randomSalt);

            user.Password = hashedPassword;
            user.Salt = randomSalt;

            _context.IpUsers.Add(user);
            _context.SaveChanges();
            return true;
        }

        public void Update(IpUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<IpUser> LoginAsync(string email, string password)
        {
            IpUser user = await _context.IpUsers.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return user;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            var userWithSameEmail = await _context.IpUsers.FirstOrDefaultAsync(u => u.Email == email);

            return userWithSameEmail != null;
        }

    }
}
