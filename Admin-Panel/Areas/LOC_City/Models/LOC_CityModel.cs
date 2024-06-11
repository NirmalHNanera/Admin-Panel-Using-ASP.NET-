using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Areas.LOC_City.Models
{
    public class LOC_CityModel
    {
        public int? CityID { get; set; }

        [Required(ErrorMessage = "Enter City Name")]
        public string CityName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Enter City Code")]
        public string Citycode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Select Country Name")]
        public int CountryID { get; set; }

        [Required(ErrorMessage = "Select State Name")]
        public int StateID { get; set; }


        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public class LOC_CityDropDownModel
    {
        public int? CityID { get; set; }

        [Required]
        public string? CityName { get; set; }
    }
}
