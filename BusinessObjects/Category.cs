using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            InverseParentCategory = new HashSet<Category>();
            NewsArticles = new HashSet<NewsArticle>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters")]
        public string CategoryName { get; set; } = null!;

        [Required(ErrorMessage = "Category Description is required")]
        [StringLength(250, ErrorMessage = "Category Description cannot exceed 250 characters")]
        public string CategoryDesciption { get; set; } = null!;

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; } = true;

        [ForeignKey("ParentCategoryId")]
        [InverseProperty("InverseParentCategory")]
        public virtual Category? ParentCategory { get; set; }

        [InverseProperty("ParentCategory")]
        public virtual ICollection<Category> InverseParentCategory { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<NewsArticle> NewsArticles { get; set; }
    }
}
