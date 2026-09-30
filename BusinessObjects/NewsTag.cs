using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("NewsTag")]
    public partial class NewsTag
    {
        public string NewsArticleId { get; set; } = null!;
        public int TagId { get; set; }

        [ForeignKey("NewsArticleId")]
        [InverseProperty("NewsTags")]
        public virtual NewsArticle NewsArticle { get; set; } = null!;

        [ForeignKey("TagId")]
        [InverseProperty("NewsTags")]
        public virtual Tag Tag { get; set; } = null!;
    }
}
