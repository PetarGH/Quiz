using Application.IRepositories;
using Application.IManagers;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryManager(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }


        public IpCategory GetCategoryById(int id)
        {
            IpCategory category = categoryRepository.GetCategoryById(id);
            return category;
        }
        public List<IpCategory> GetAllCategories()
        {
            List<IpCategory> categories = categoryRepository.GetAllCategories();
            return categories;
        }

        public async Task<bool> AddCategory(AddCategoryModel categoryModel) 
        {
            if (categoryModel.ParentId == 0)
            {
                IpCategory category = new IpCategory(categoryModel.Name);

                if (category == null)
                {
                    throw new ArgumentNullException(nameof(category));
                }
                if (category.Name.Length != 0)
                {
                    try
                    {
                        bool result = categoryRepository.Add(category);
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
            else
            {
                IpCategory category = new IpCategory(categoryModel.Name, categoryModel.ParentId);

                if (category == null)
                {
                    throw new ArgumentNullException(nameof(category));
                }
                if (category.Name.Length != 0)
                {
                    try
                    {
                        bool result = categoryRepository.Add(category);
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
        }

        public async Task<bool> DeleteCategory(int id)
        {
            bool result = await categoryRepository.Delete(id);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateCategory(IpCategory category)
        {
            categoryRepository.Update(category);
        }

    }
}
