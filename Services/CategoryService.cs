using BusinessObjects;
using Repositories;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public CategoryService()
        {
            _categoryRepo = new CategoryRepository();
        }

        public List<Category> GetAllCategories() => _categoryRepo.GetAll();

        public List<Category> GetActiveCategories() => _categoryRepo.GetActiveCategories();

        public Category? GetCategoryById(short id) => _categoryRepo.GetById(id);

        public void CreateCategory(Category category) => _categoryRepo.Add(category);

        public void UpdateCategory(Category category) => _categoryRepo.Update(category);

        public bool DeleteCategory(short id) => _categoryRepo.Delete(id);

        public bool HasNewsArticles(short id) => _categoryRepo.HasNewsArticles(id);

        public List<Category> SearchCategories(string? keyword) => _categoryRepo.Search(keyword);
    }
}
