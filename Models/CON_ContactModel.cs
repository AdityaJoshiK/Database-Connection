using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Database_Connection.Models
{
    public class CON_ContactModel
    {
        public int? ContactID { get; set; }
        public int? ContactCategoryID { get; set; }
        public int? CountryID { get; set; }
        public int? StateID { get; set; }
        public int? CityID { get; set; }
        [Required]
        public string? ContactName { get; set; }
        [Required]
        public string? Address { get; set; }
        public string? PinCode { get; set; }
        [Required]
        public string? Mobile { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Gender { get; set; }
        public string? LinkedIn { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public IFormFile File { get; set; }
        public string PhotoPath { get; set; }
    }

    public class CON_ContactDropDownModel
    {
        public int ContactID { get; set; }
        public string? ContactName { get; set; }
    }
}
