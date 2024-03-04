using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface ICategoryRepository
    {
        List<IpCategory> GetAllCategories();
        Task<bool> Delete(int id);
        bool Add(IpCategory category);
        IpCategory GetCategoryById(int id);
        void Update(IpCategory category);
    }
}
