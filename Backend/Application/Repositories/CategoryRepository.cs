using Domain.Data;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IRepositories;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Dbi477163Context _context;

        public CategoryRepository(Dbi477163Context context)
        {
            _context = context;
        }

        public List<IpCategory> GetAllCategories()
        {
            return _context.IpCategories.ToList();
        }
        public async Task<bool> Delete(int id)
        {
            var category = await _context.IpCategories.FindAsync(id);
            if (category != null)
            {
                _context.IpCategories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool Add(IpCategory category)
        {
            if (category == null)
            {
                return false;
            }

            _context.IpCategories.Add(category);
            _context.SaveChanges();
            return true;
        }
        public IpCategory GetCategoryById(int id)
        {
            return _context.IpCategories.Find(id);
        }
        public void Update(IpCategory category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            _context.IpCategories.Update(category);
            _context.SaveChanges();
        }

    }
}
