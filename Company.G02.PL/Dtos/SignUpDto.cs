using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class SignUpDto
    {
        [Required (ErrorMessage= "UseName is Required ")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "FirstName is Required ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required ")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is Required ")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm is Required ")]
        [DataType(DataType.Password)]
        [Compare(nameof( Password),ErrorMessage = "ConfirmPassword Dosn't Match The Password ")]
        public string ConfirmPassword { get; set; }
        public bool Igree { get; set; }

    }
}
