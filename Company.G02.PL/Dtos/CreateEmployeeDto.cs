using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class CreateEmployeeDto
    {
        
        [Required (ErrorMessage ="name is required")]
        public string Name { get; set; }
        [Range (20,65,ErrorMessage ="Age Must Be Between 20 to 65")]
        public int? Age { get; set; }
        [DataType(DataType.EmailAddress,ErrorMessage ="Email is'n Valid")]
        public string Email { get; set; }
        //[RegularExpression(@"[0,9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}$",
        //    ErrorMessage ="Address Must Be Like 123-street-city-countery")]
        public string Address { get; set; }
        [Phone]
        public string Phone { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [DisplayName ("HirringDate")]
        public DateTime HirringDate { get; set; }
        [DisplayName("Date Of Creation")]
        public DateTime CreateAt { get; set; }
        public int? DepartmentId { get; set; }
        public IFormFile Image { get; set; }
        public string? ImageName { get; set; }
    }
}
