using AutoMapper;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        

        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        }
        [HttpGet]

        public IActionResult Index()
        {
            var department= _unitOfWork.departmentRepository.GetAll();
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
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt

                };
                //var department = _mapper.Map<Department>(model);
               var count= _unitOfWork.departmentRepository.Add(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Details(int? id) 
        {
            if (id is null) return BadRequest("Invalid Id");//400
            var department = _unitOfWork.departmentRepository.Get(id.Value);
            if (department == null) return NotFound(new { StatusCode = 404, message = $"Department With Id {id} is Not Found " });

           return View(department);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");//400
            var department = _unitOfWork.departmentRepository.Get(id.Value);
            if (department == null) return NotFound(new { StatusCode = 404, message = $"Department With Id {id} is Not Found " });
            var departmentdto = new CreateDepartmentDto()
            {

                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
            };
            //var departmentdto = _mapper.Map<CreateDepartmentDto>(department);
            return View(departmentdto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                // if (id != department.Id) BadRequest();//400
                var department = new Department()
                {
                    Id = id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
               // var department= _mapper.Map<Department>(model);
                var count = _unitOfWork.departmentRepository.Update(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        #region MyRegion
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department()
        //        {
        //            Id = id,
        //            Code = model.Code,
        //            Name = model.Name,
        //            CreateAt = model.CreateAt,
        //        };
        //        var count = _departmenRepository.Update(department);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(model);
        //}

        #endregion

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");//400
            var department = _unitOfWork.departmentRepository.Get(id.Value);
            if (department == null) return NotFound(new { StatusCode = 404, message = $"Department With Id {id} is Not Found " });
            var dto= _mapper.Map<Department>(department);
            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
                {
                    Id = id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
               
               
                var count = _unitOfWork.departmentRepository.Delete(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
