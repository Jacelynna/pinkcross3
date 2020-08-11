using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace pinkcross3.Models
{
    public class ForgetPassword
    {
        [DataType(DataType.Password)]
        [Required (ErrorMessage = "Cannot be empty!")]
        [Remote("VerifyNewPassword", "User")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Cannot be empty!")]
        [Compare("NewPassword", ErrorMessage = "Password not confirmed!")]
        public string ConfirmPassword { get; set; }
    }
}
