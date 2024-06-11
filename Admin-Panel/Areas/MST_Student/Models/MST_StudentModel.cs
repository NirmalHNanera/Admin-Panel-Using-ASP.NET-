using System;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Areas.MST_Student.Models
{
    public class MST_StudentModel
    {

        public int? StudentID { get; set; }

        [Required(ErrorMessage = "Select Branch Name")]
        public int BranchID { get; set; }

        [Required(ErrorMessage = "Select City Name")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "Enter Student Name")]
        public string StudentName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Enter Student Mobile Number")]
        public string MobileNoStudent { get; set; } = string.Empty;


        [Required(ErrorMessage = "Enter Student Email")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Enter Student Father Mobile Number")]
        public string MobileNoFather { get; set; } = string.Empty;


        [Required(ErrorMessage = "Enter Student Address")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter Student BirthDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Enter Student Age")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Enter Student Is Active")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Enter Student Gender")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter Student Password")]
        [StringLength(maximumLength: 20, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public IFormFile File { get; set; }
        public string Photopath { get; set; }
    }

}
