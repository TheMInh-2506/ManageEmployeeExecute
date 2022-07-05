using System.ComponentModel.DataAnnotations;

namespace ManageEmployee.Models
{
    public class User
    {
           [Key] public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

     
    }
}
