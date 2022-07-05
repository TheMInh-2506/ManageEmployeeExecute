using System.ComponentModel.DataAnnotations;

namespace ManageEmployee.DTO
{
    public class UserDTO
    {
       // [Required(ErrorMessage = "User Name is required")]
   public string UserName { get ; set; }
       // [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
