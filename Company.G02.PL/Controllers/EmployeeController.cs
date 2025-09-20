using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index(string SearchInput)
        {
            IEnumerable<Employee>employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees= _unitOfWork.employeeRepository.GetAll();
            }
            else
            {
               employees= _unitOfWork.employeeRepository.GetByName(SearchInput);
            }
            
            
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var departments = _unitOfWork.departmentRepository.GetAll();
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
                    #region Manual Mapping
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
                    //    DepartmentId = model.DepartmentId
                    //};
                    //if (model is not null)
                    //{
                    //      model.ImageName= DecumentSettings.UploadFile(model.Image, "Images");
                    //}

                    #endregion

                    if (model.Image != null)
                    {
                        model.ImageName = DecumentSettings.UploadFile(model.Image, "Images");
                    }
                    //if (model.Image != null)
                    //{
                    //    Console.WriteLine($"File: {model.Image.FileName}, Size: {model.Image.Length}");
                    //}
                    var employee = _mapper.Map<Employee>(model);
                    var count = _unitOfWork.employeeRepository.Add(employee);

                    
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
            var employees= _unitOfWork.employeeRepository.Get(id.Value);
            if (employees == null) return NotFound(new { StatusCode = 404, message = $"Employee With Id: {id} Is Not found" });
            return View(viewname,employees);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departments = _unitOfWork.departmentRepository.GetAll();
            ViewData["departments"] = departments;
            if (id == null) return BadRequest("Invalid Id");
            var employee = _unitOfWork.employeeRepository.Get(id.Value);
            if (employee == null) return NotFound(new { StatusCode = 404, message = $"Employee With Id: {id} Is Not found" });
            //var dto=_mapper.Map<CreateEmployeeDto>(employee);
            var employeedto = new CreateEmployeeDto()
            {

                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                Email = employee.Email,
                Phone = employee.Phone,
                Salary = employee.Salary,
                HirringDate = employee.HirringDate,
                CreateAt = employee.CreateAt,
            };
            return View(employeedto);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Edit([FromRoute]int id,Employee model)
        {
            if(ModelState.IsValid)
            {
              
                  if(id!=model.Id)return BadRequest();
                
                var count= _unitOfWork.employeeRepository.Update(model);
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

        public IActionResult Delete([FromRoute] int id, Employee model)
        {
            if (ModelState.IsValid)
            {
                //var employee = new Employee()
                //{
                //    Id = id,
                //    Name = model.Name,
                //    Age = model.Age,
                //    Address = model.Address,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    Email = model.Email,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //};
                if (id != model.Id) return BadRequest();
                var count = _unitOfWork.employeeRepository.Delete(model);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
