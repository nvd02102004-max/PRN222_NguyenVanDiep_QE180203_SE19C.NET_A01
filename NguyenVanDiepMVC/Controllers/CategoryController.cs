using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace NguyenVanDiepMVC.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index(string? search)
        {
            ViewBag.SearchKeyword = search;
            var categories = _categoryService.SearchCategories(search);
            ViewBag.AllCategories = _categoryService.GetAllCategories();
            return View(categories);
        }

        [HttpGet]
        public IActionResult GetCategory(short id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();

            return Json(new
            {
                categoryId = category.CategoryId,
                categoryName = category.CategoryName,
                categoryDesciption = category.CategoryDesciption,
                parentCategoryId = category.ParentCategoryId,
                isActive = category.IsActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName) || string.IsNullOrWhiteSpace(category.CategoryDesciption))
            {
                TempData["ErrorMessage"] = "Category Name and Description are required.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _categoryService.CreateCategory(category);
                TempData["SuccessMessage"] = "Category created successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error creating category: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (category.CategoryId <= 0 || string.IsNullOrWhiteSpace(category.CategoryName) || string.IsNullOrWhiteSpace(category.CategoryDesciption))
            {
                TempData["ErrorMessage"] = "Invalid data. Please fill in all required fields.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _categoryService.UpdateCategory(category);
                TempData["SuccessMessage"] = "Category updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating category: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(short id)
        {
            try
            {
                if (_categoryService.HasNewsArticles(id))
                {
                    TempData["ErrorMessage"] = "Cannot delete this category because it is already used in one or more news articles.";
                    return RedirectToAction(nameof(Index));
                }

                var success = _categoryService.DeleteCategory(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Category deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Category not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
