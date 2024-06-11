using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter User Name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter password")]
        public string Password { get; set; }
    }
}
