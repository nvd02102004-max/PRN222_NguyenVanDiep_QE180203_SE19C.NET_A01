using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("SystemAccount")]
    public partial class SystemAccount
    {
        public SystemAccount()
        {
            NewsArticles = new HashSet<NewsArticle>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short AccountId { get; set; }

        [Required(ErrorMessage = "Account Name is required")]
        [StringLength(100, ErrorMessage = "Account Name cannot exceed 100 characters")]
        public string? AccountName { get; set; }

        [Required(ErrorMessage = "Account Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email address format")]
        [StringLength(70, ErrorMessage = "Email cannot exceed 70 characters")]
        public string? AccountEmail { get; set; }

        [Required(ErrorMessage = "Account Role is required")]
        public int? AccountRole { get; set; } // 1: Staff, 2: Lecturer

        [Required(ErrorMessage = "Password is required")]
        [StringLength(70, MinimumLength = 1, ErrorMessage = "Password cannot exceed 70 characters")]
        public string? AccountPassword { get; set; }

        [InverseProperty("CreatedBy")]
        public virtual ICollection<NewsArticle> NewsArticles { get; set; }
    }
}
