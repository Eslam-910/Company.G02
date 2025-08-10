using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employee;

        public EmployeeController(IEmployeeRepository employee)
        {
            _employee = employee;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var employees= _employee.GetAll();
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if(ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Email = model.Email,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    HirringDate = model.HirringDate,
                    CreateAt = model.CreateAt,
                };
                var count=_employee.Add(employee);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int?id,string viewname="Details")
        {
            if (id == null) return BadRequest("Invalid Id");
            var employees= _employee.Get(id.Value);
            if (employees == null) return NotFound(new { StatusCode = 404, message = $"Employee With Id: {id} Is Not found" });
            return View(viewname,employees);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
           return Details(id,"Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Edit([FromRoute]int id,Employee employee)
        {
            if(ModelState.IsValid)
            {
                if (id != employee.Id) return BadRequest();
                var count=_employee.Update(employee);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete([FromRoute] int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id != employee.Id) return BadRequest();
                var count = _employee.Delete(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employee);
        }
    }
}
