using Application.IManagers;
using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Tests.Fakers
{
    public class CategoryRepoFaker : ICategoryRepository
    {
        private readonly List<IpCategory> _categories;

        public CategoryRepoFaker()
        {
            _categories = new List<IpCategory>();
        }

        public List<IpCategory> GetAllCategories()
        {
            return _categories.ToList();
        }
        public async Task<bool> Delete(int id)
        {
            IpCategory category = new IpCategory("Fun");
            _categories.Add(category);
            if (_categories.Count > 0)
            {
                _categories.Remove(category);
                return true;
            }
            else return false;
        }

        public bool Add(IpCategory category)
        {
            _categories.Add(category);
            if (_categories.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
        public IpCategory GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }
        public void Update(IpCategory category)
        {
            throw new NotImplementedException();
        }
    }
}
