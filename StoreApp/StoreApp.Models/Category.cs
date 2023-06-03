using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StoreApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string? Name { get; set; }
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        [DisplayName("DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}
