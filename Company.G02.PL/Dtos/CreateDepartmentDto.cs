using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage ="The Code is Reqiured ")]
        public string Code { get; set; }
        [Required(ErrorMessage = "The Name is Reqiured ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The CreatAt is Reqiured ")]
        public DateTime CreateAt { get; set; }

    }
}
