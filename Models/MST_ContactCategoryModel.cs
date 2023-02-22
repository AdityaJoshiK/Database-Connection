using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Database_Connection.Models
{
    public class MST_ContactCategoryModel
    {
        public int? ContactCategoryID { get; set; }
        public string? ContactCategoryName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public class MST_ContactCategoryDropDownModel
    {
        public int ContactCategoryID { get; set; }
        [Required]
        public string? ContactCategoryName { get; set; }
    }
}
