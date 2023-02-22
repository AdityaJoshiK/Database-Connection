//using Microsoft.Build.Framework;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Database_Connection.Models
{
    public class LOC_CountryModel
    {
        public int? CountryID { get; set; }
        [Required(ErrorMessage = "Please enter Country name"), MaxLength(50)]
        [DataType(DataType.Text)]
        [DisplayName("Country Name")]
        [StringLength(20, MinimumLength = 3)]
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }

    public class LOC_CountryDropDownModel
    { 
        public int CountryID { get; set; }
        public string? CountryName { get; set;}
    }
}
