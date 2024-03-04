using Domain.Entities;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IManagers
{
    public interface ICategoryManager
    {
        IpCategory GetCategoryById(int id);
        List<IpCategory> GetAllCategories();
        Task<bool> AddCategory(AddCategoryModel categoryModel);
        Task<bool> DeleteCategory(int id);
        void UpdateCategory(IpCategory category);
    }
}
