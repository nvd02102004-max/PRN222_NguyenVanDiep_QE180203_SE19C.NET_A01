using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("Tag")]
    public partial class Tag
    {
        public Tag()
        {
            NewsTags = new HashSet<NewsTag>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TagId { get; set; }

        [StringLength(50, ErrorMessage = "Tag Name cannot exceed 50 characters")]
        public string? TagName { get; set; }

        [StringLength(400, ErrorMessage = "Note cannot exceed 400 characters")]
        public string? Note { get; set; }

        [InverseProperty("Tag")]
        public virtual ICollection<NewsTag> NewsTags { get; set; }
    }
}
