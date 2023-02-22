using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Database_Connection.Models
{
    public class LOC_CityModel
    {
        public int? CityID { get; set; }
        [Required]
        public string? CityName { get; set; }
        public int? CityCode { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public class LOC_CityDropDownModel
    {
        public int CityID { get; set; }
        public string? CityName { get; set; }
    }
}
