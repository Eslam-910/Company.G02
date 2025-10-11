using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "New Password is Required ")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm is Required ")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "ConfirmPassword Dosn't Match The Password ")]
        public string ConfirmPassword { get; set; }
    }
}
