using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
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
        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid)//Server Side Validation
            {
                var department = new Department()
                {
                    code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
               var count= _departmenRepository.Add(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(model);
        }

    }
}
