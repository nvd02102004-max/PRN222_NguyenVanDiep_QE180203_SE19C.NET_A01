using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NguyenVanDiepMVC.Models;
using Services;

namespace NguyenVanDiepMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;

        public AdminController(INewsArticleService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Report(DateTime? startDate, DateTime? endDate)
        {
            // Default to last 30 days if not specified, or show all
            var articles = _newsService.GetReport(startDate, endDate);

            var vm = new ReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Articles = articles,
                TotalArticles = articles.Count,
                ActiveArticles = articles.Count(a => a.NewsStatus == true),
                InactiveArticles = articles.Count(a => a.NewsStatus != true)
            };

            // Category breakdown
            var categories = _categoryService.GetAllCategories();
            foreach (var cat in categories)
            {
                int count = articles.Count(a => a.CategoryId == cat.CategoryId);
                if (count > 0)
                {
                    vm.CategoryStats[cat.CategoryName] = count;
                }
            }

            return View(vm);
        }
    }
}
