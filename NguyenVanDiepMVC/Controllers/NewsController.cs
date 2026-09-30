using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace NguyenVanDiepMVC.Controllers
{
    [AllowAnonymous]
    public class NewsController : Controller
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;

        public NewsController(INewsArticleService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index(string? search, short? categoryId)
        {
            ViewBag.SearchKeyword = search;
            ViewBag.SelectedCategoryId = categoryId;

            // Only active news articles are shown for public / lecturer
            var articles = _newsService.SearchNews(search, categoryId, activeOnly: true);
            ViewBag.Categories = _categoryService.GetActiveCategories();

            return View(articles);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var article = _newsService.GetNewsById(id);
            if (article == null)
            {
                return NotFound();
            }

            // If public or lecturer and status is inactive, return forbidden or not found
            if ((article.NewsStatus != true) && (!User.IsInRole("Admin") && !User.IsInRole("Staff")))
            {
                return NotFound();
            }

            return View(article);
        }
    }
}
