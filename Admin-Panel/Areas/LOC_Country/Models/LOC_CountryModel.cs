using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace AdminPanel.Areas.LOC_Country.Models
{
    public class LOC_CountryModel
    {
        public int? CountryID { get; set; }

        [Required(ErrorMessage = "Enter Country Name")]
        public string CountryName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter Country Code")]
        public string CountryCode { get; set; } = string.Empty;

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }


    public class LOC_CountryDropDownModel
    {
        public int? CountryID { get; set; }

        [Required]
        public string? CountryName { get; set; }
    }
}
