using System.ComponentModel.DataAnnotations;
using BusinessObjects;

namespace NguyenVanDiepMVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public class ProfileViewModel
    {
        public short AccountId { get; set; }

        [Required(ErrorMessage = "Account Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string AccountName { get; set; } = string.Empty;

        public string AccountEmail { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(70, MinimumLength = 1, ErrorMessage = "Password must be at least 1 character")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password confirmation does not match")]
        public string? ConfirmPassword { get; set; }
    }

    public class ReportViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public int TotalArticles { get; set; }
        public int ActiveArticles { get; set; }
        public int InactiveArticles { get; set; }

        public List<NewsArticle> Articles { get; set; } = new();
        public Dictionary<string, int> CategoryStats { get; set; } = new();
    }
}
