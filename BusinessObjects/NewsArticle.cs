using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("NewsArticle")]
    public partial class NewsArticle
    {
        public NewsArticle()
        {
            NewsTags = new HashSet<NewsTag>();
        }

        [Key]
        [StringLength(20, ErrorMessage = "Article ID cannot exceed 20 characters")]
        [Required(ErrorMessage = "News Article ID is required")]
        public string NewsArticleId { get; set; } = null!;

        [StringLength(400, ErrorMessage = "News Title cannot exceed 400 characters")]
        [Required(ErrorMessage = "News Title is required")]
        public string? NewsTitle { get; set; }

        [Required(ErrorMessage = "Headline is required")]
        [StringLength(150, ErrorMessage = "Headline cannot exceed 150 characters")]
        public string Headline { get; set; } = null!;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [StringLength(4000, ErrorMessage = "News Content cannot exceed 4000 characters")]
        public string? NewsContent { get; set; }

        [StringLength(400, ErrorMessage = "News Source cannot exceed 400 characters")]
        public string? NewsSource { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; } = true;

        public short? CreatedById { get; set; }

        public short? UpdatedById { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("NewsArticles")]
        public virtual Category? Category { get; set; }

        [ForeignKey("CreatedById")]
        [InverseProperty("NewsArticles")]
        public virtual SystemAccount? CreatedBy { get; set; }

        [InverseProperty("NewsArticle")]
        public virtual ICollection<NewsTag> NewsTags { get; set; }
    }
}
