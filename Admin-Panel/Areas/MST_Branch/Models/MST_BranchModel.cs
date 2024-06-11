using System.ComponentModel.DataAnnotations;
namespace AdminPanel.Areas.MST_Branch.Models
{
    public class MST_BranchModel
    {
        public int? BranchID { get; set; }


        [Required(ErrorMessage = "Enter Branch Name")]
        public string BranchName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Enter Branch Code")]
        public string BranchCode { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }


    public class MST_BranchDropDownModel
    {
        public int? BranchID { get; set; }

        [Required]
        public string? BranchName { get; set; }
    }

}
