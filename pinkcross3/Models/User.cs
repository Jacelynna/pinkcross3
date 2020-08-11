using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace pinkcross3.Models
{
    public class User
    {
        [Required(ErrorMessage = "Please enter your Username ID")]
        public string Username_id { get; set; }
        [Required(ErrorMessage = "Please enter your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
