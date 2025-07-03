using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    //Mvc Controller
    public class DepartmentController : Controller
    {
       private readonly IDepartmentRepository _departmenRepository;
        public DepartmentController(IDepartmentRepository departmenRepository)
        {
            _departmenRepository = departmenRepository;
        }
        [HttpGet]

        public IActionResult Index()
        {
            var department= _departmenRepository.GetAll();
            return View(department);
        }
    }
}
