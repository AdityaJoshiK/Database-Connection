//using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Database_Connection.Models
{
    public class LOC_StateModel
    {
        public int? StateID { get; set; }
        [Required]
        public int? StateCode { get; set; }
        [Required]
        public string? StateName { get; set; }
        public int? CountryID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }

    public class LOC_StateDropDownModel
    {
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public string? StateName { get; set; }
    }
}
