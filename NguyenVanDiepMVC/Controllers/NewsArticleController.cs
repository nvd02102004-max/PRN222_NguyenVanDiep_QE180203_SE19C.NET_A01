using System.Security.Claims;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace NguyenVanDiepMVC.Controllers
{
    [Authorize(Roles = "Staff")]
    public class NewsArticleController : Controller
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public NewsArticleController(
            INewsArticleService newsService,
            ICategoryService categoryService,
            ITagService tagService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        [HttpGet]
        public IActionResult Index(string? search, short? categoryId, bool? status)
        {
            ViewBag.SearchKeyword = search;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedStatus = status;

            var articles = _newsService.SearchNews(search, categoryId, status);
            ViewBag.Categories = _categoryService.GetAllCategories();
            ViewBag.Tags = _tagService.GetAllTags();

            return View(articles);
        }

        [HttpGet]
        public IActionResult GetArticle(string id)
        {
            var article = _newsService.GetNewsById(id);
            if (article == null)
                return NotFound();

            var tagIds = article.NewsTags.Select(nt => nt.TagId).ToList();

            return Json(new
            {
                newsArticleId = article.NewsArticleId,
                newsTitle = article.NewsTitle,
                headline = article.Headline,
                newsContent = article.NewsContent,
                newsSource = article.NewsSource,
                categoryId = article.CategoryId,
                newsStatus = article.NewsStatus,
                selectedTagIds = tagIds
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NewsArticle article, List<int>? selectedTags)
        {
            if (string.IsNullOrWhiteSpace(article.NewsArticleId) ||
                string.IsNullOrWhiteSpace(article.NewsTitle) ||
                string.IsNullOrWhiteSpace(article.Headline) ||
                !article.CategoryId.HasValue)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields (Article ID, Title, Headline, Category).";
                return RedirectToAction(nameof(Index));
            }

            var existing = _newsService.GetNewsById(article.NewsArticleId.Trim());
            if (existing != null)
            {
                TempData["ErrorMessage"] = "An article with this ID already exists. Please choose a different ID.";
                return RedirectToAction(nameof(Index));
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (short.TryParse(userIdStr, out short authorId))
            {
                article.CreatedById = authorId;
            }

            article.CreatedDate = DateTime.Now;
            article.ModifiedDate = DateTime.Now;

            try
            {
                _newsService.CreateNews(article, selectedTags);
                TempData["SuccessMessage"] = "News article created successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error creating article: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NewsArticle article, List<int>? selectedTags)
        {
            if (string.IsNullOrWhiteSpace(article.NewsArticleId) ||
                string.IsNullOrWhiteSpace(article.NewsTitle) ||
                string.IsNullOrWhiteSpace(article.Headline) ||
                !article.CategoryId.HasValue)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                return RedirectToAction(nameof(Index));
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (short.TryParse(userIdStr, out short currentUserId))
            {
                article.UpdatedById = currentUserId;
            }

            article.ModifiedDate = DateTime.Now;

            try
            {
                _newsService.UpdateNews(article, selectedTags);
                TempData["SuccessMessage"] = "News article updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating article: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            try
            {
                var success = _newsService.DeleteNews(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "News article deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Article not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting article: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult History()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!short.TryParse(userIdStr, out short authorId))
            {
                return RedirectToAction("Login", "Account");
            }

            var myArticles = _newsService.GetNewsByAuthor(authorId);
            return View(myArticles);
        }
    }
}
