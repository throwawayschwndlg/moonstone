using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.services
{
    public interface ICategoryService
    {
        Category CreateCategory(Category category);

        IEnumerable<Category> GetCategoriesForGroup(Guid groupId);

        IEnumerable<Category> GetCategoriesForUser(Guid userId);

        Category GetCategoryById(Guid id);

        Category GetDefaultCategoryForUser(Guid userId);
    }
}