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
        public async Task<IActionResult> Index(string SearchInput)
        {
            IEnumerable<Employee>employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees= await _unitOfWork.employeeRepository.GetAllAsync();
            }
            else
            {
               employees= await _unitOfWork.employeeRepository.GetByNameAsync(SearchInput);
            }
            
            
            return View(employees);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.departmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
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
                    var count = await _unitOfWork.employeeRepository.AddAsync(employee);

                    
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
        public async Task<IActionResult> Details(int?id,string viewname="Details")
        {
            if (id == null) return BadRequest("Invalid Id");
            var employees= await _unitOfWork.employeeRepository.GetAsync(id.Value);
            if (employees == null) return NotFound(new { StatusCode = 404, message = $"Employee With Id: {id} Is Not found" });
          
            //var dto = _mapper.Map<CreateEmployeeDto>(employees);
            return View(viewname, employees);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var departments = await _unitOfWork.departmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            if (id == null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.employeeRepository.GetAsync(id.Value);
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
        
        public IActionResult Edit([FromRoute]int id,CreateEmployeeDto model)
        {
            if(ModelState.IsValid)
            {
                if(model.ImageName is not null&&model.Image is not null)
                {
                    DecumentSettings.FileDelete(model.ImageName, "Images");
                }
                if (model.Image is not null)
                {
                   model.ImageName= DecumentSettings.UploadFile(model.Image, "Images");
                }

               var employee= _mapper.Map<Employee>(model);
                employee.Id = id;
                var count= _unitOfWork.employeeRepository.Update(employee);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete([FromRoute] int id, Employee model)
        {
            if (ModelState.IsValid)
            {
               
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                var count = _unitOfWork.employeeRepository.Delete(employee);
                if (count > 0)
                {
                    if(model.ImageName is not null)
                    {
                        DecumentSettings.FileDelete(model.ImageName, "Images");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
