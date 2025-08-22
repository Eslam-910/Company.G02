using AutoMapper;
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
        private readonly IDepartmentRepository _department;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employee,IDepartmentRepository department,IMapper mapper)
        {
            _employee = employee;
            _department = department;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index(string SearchInput)
        {
            IEnumerable<Employee>employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees= _employee.GetAll();
            }
            else
            {
               employees= _employee.GetByName(SearchInput);
            }
            
            
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var departments = _department.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Manual Mapping
                    //var employee = new Employee()
                    //{
                    //    Name = model.Name,
                    //    Age = model.Age,
                    //    Address = model.Address,
                    //    IsActive = model.IsActive,
                    //    IsDeleted = model.IsDeleted,
                    //    Email = model.Email,
                    //    Phone = model.Phone,
                    //    Salary = model.Salary,
                    //    HirringDate = model.HirringDate,
                    //    CreateAt = model.CreateAt,
                    //    DepartmentId=model.DepartmentId
                    //};
                    var employee = _mapper.Map<Employee>(model);
                    var count = _employee.Add(employee);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
               
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("",ex.Message);
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
            var departments = _department.GetAll();
            ViewData["departments"] = departments;
            if (id == null) return BadRequest("Invalid Id");
            var employee = _employee.Get(id.Value);
            if (employee == null) return NotFound(new { StatusCode = 404, message = $"Employee With Id: {id} Is Not found" });
            var dto=_mapper.Map<CreateEmployeeDto>(employee);
            return View(dto);

           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Edit([FromRoute]int id,Employee model)
        {
            if(ModelState.IsValid)
            {
                  if(id!=model.Id)return BadRequest();
                
                var count=_employee.Update(model);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee=new Employee()
                {
                    Id = id,
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address,
                    IsActive = model.IsActive,
                    IsDeleted=model.IsDeleted,
                    Email = model.Email,
                    Phone = model.Phone,
                    Salary = model.Salary,
                };
                if (id != employee.Id) return BadRequest();
                var count = _employee.Delete(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
