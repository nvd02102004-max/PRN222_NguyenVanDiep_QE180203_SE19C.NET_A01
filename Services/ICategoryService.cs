using BusinessObjects;

namespace Services
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
        List<Category> GetActiveCategories();
        Category? GetCategoryById(short id);
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        bool DeleteCategory(short id);
        bool HasNewsArticles(short id);
        List<Category> SearchCategories(string? keyword);
    }
}
