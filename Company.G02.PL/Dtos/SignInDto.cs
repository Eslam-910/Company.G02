using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class SignInDto
    {
        [EmailAddress]
        [Required(ErrorMessage ="The Email Is Required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="The Password Is Required")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
