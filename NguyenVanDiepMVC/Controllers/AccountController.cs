using System.Security.Claims;
using BusinessObjects;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NguyenVanDiepMVC.Models;
using Services;

namespace NguyenVanDiepMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISystemAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(ISystemAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Account");
                if (User.IsInRole("Staff"))
                    return RedirectToAction("Index", "NewsArticle");
                return RedirectToAction("Index", "News");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var adminEmail = _configuration["AdminAccount:Email"] ?? "admin@FUNewsManagementSystem.org";
            var adminPassword = _configuration["AdminAccount:Password"] ?? "@@abc123@@";

            // 1. Check Admin account from appsettings.json
            if (string.Equals(model.Email.Trim(), adminEmail.Trim(), StringComparison.OrdinalIgnoreCase) &&
                model.Password == adminPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "0"),
                    new Claim(ClaimTypes.Name, "System Administrator"),
                    new Claim(ClaimTypes.Email, adminEmail),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties { IsPersistent = model.RememberMe });

                HttpContext.Session.SetInt32("AccountId", 0);
                HttpContext.Session.SetString("AccountName", "System Administrator");
                HttpContext.Session.SetString("Role", "Admin");

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Account");
            }

            // 2. Check Database account (Staff or Lecturer)
            var account = _accountService.Authenticate(model.Email, model.Password);
            if (account != null)
            {
                string roleName = account.AccountRole == 1 ? "Staff" : (account.AccountRole == 2 ? "Lecturer" : "Guest");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                    new Claim(ClaimTypes.Name, account.AccountName ?? "User"),
                    new Claim(ClaimTypes.Email, account.AccountEmail ?? ""),
                    new Claim(ClaimTypes.Role, roleName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties { IsPersistent = model.RememberMe });

                HttpContext.Session.SetInt32("AccountId", account.AccountId);
                HttpContext.Session.SetString("AccountName", account.AccountName ?? "");
                HttpContext.Session.SetString("Role", roleName);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                if (roleName == "Staff")
                    return RedirectToAction("Index", "NewsArticle");
                else
                    return RedirectToAction("Index", "News");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password. Please try again.");
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }

        #region Admin Account Management

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Index(string? search)
        {
            ViewBag.SearchKeyword = search;
            var accounts = _accountService.SearchAccounts(search);
            return View(accounts);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAccount(short id)
        {
            var account = _accountService.GetAccountById(id);
            if (account == null)
                return NotFound();

            return Json(new
            {
                accountId = account.AccountId,
                accountName = account.AccountName,
                accountEmail = account.AccountEmail,
                accountRole = account.AccountRole,
                accountPassword = account.AccountPassword
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SystemAccount model)
        {
            if (string.IsNullOrWhiteSpace(model.AccountName) ||
                string.IsNullOrWhiteSpace(model.AccountEmail) ||
                string.IsNullOrWhiteSpace(model.AccountPassword) ||
                !model.AccountRole.HasValue)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                return RedirectToAction(nameof(Index));
            }

            var existing = _accountService.GetAccountByEmail(model.AccountEmail);
            if (existing != null)
            {
                TempData["ErrorMessage"] = "An account with this email already exists.";
                return RedirectToAction(nameof(Index));
            }

            if (model.AccountId <= 0)
            {
                model.AccountId = _accountService.GetNextAccountId();
            }

            _accountService.CreateAccount(model);
            TempData["SuccessMessage"] = "Account created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SystemAccount model)
        {
            if (model.AccountId <= 0 || string.IsNullOrWhiteSpace(model.AccountName) || string.IsNullOrWhiteSpace(model.AccountEmail))
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                return RedirectToAction(nameof(Index));
            }

            var existingEmail = _accountService.GetAccountByEmail(model.AccountEmail);
            if (existingEmail != null && existingEmail.AccountId != model.AccountId)
            {
                TempData["ErrorMessage"] = "Email address is already in use by another account.";
                return RedirectToAction(nameof(Index));
            }

            _accountService.UpdateAccount(model);
            TempData["SuccessMessage"] = "Account updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(short id)
        {
            var success = _accountService.DeleteAccount(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Account deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to delete account.";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Profile Management

        [Authorize(Roles = "Staff,Lecturer")]
        [HttpGet]
        public IActionResult Profile()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!short.TryParse(userIdStr, out short accountId))
            {
                return RedirectToAction("Login");
            }

            var account = _accountService.GetAccountById(accountId);
            if (account == null)
            {
                return NotFound();
            }

            var vm = new ProfileViewModel
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName ?? "",
                AccountEmail = account.AccountEmail ?? "",
                RoleName = account.AccountRole == 1 ? "Staff" : (account.AccountRole == 2 ? "Lecturer" : "User")
            };

            return View(vm);
        }

        [Authorize(Roles = "Staff,Lecturer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(ProfileViewModel model)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!short.TryParse(userIdStr, out short accountId) || accountId != model.AccountId)
            {
                return Forbid();
            }

            var account = _accountService.GetAccountById(accountId);
            if (account == null)
            {
                return NotFound();
            }

            model.AccountEmail = account.AccountEmail ?? "";
            model.RoleName = account.AccountRole == 1 ? "Staff" : "Lecturer";

            if (string.IsNullOrWhiteSpace(model.AccountName))
            {
                ModelState.AddModelError("AccountName", "Account Name cannot be empty.");
                return View(model);
            }

            // If user wants to change password
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword) || model.CurrentPassword != account.AccountPassword)
                {
                    ModelState.AddModelError("CurrentPassword", "Current password does not match.");
                    return View(model);
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Password confirmation does not match.");
                    return View(model);
                }

                account.AccountPassword = model.NewPassword;
            }

            account.AccountName = model.AccountName;
            _accountService.UpdateAccount(account);

            HttpContext.Session.SetString("AccountName", account.AccountName);
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction(nameof(Profile));
        }

        #endregion
    }
}
