using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _rolemanager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _rolemanager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string SearchInput)
        {
            IEnumerable<RoleToReturnDto> Roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                Roles = _rolemanager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,

                });
            }
            else
            {
                Roles = _rolemanager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,

                }).Where(U => U.Name.ToLower().Contains(SearchInput.ToLower()));
            }


            return View(Roles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                var role=await _rolemanager.FindByNameAsync(model.Name);
                if(role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name,
                    };

                    var result=await _rolemanager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewname = "Details")
        {
            if (id == null) return BadRequest("Invalid Id");
            var role = await _rolemanager.FindByIdAsync(id);
            if (role == null) return NotFound(new { StatusCode = 404, message = $"Employee With Id: {id} Is Not found" });

            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name= role.Name,
            };
            return View(viewname, dto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {

            return await Details(id, "Edit");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation");
                var role = await _rolemanager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalide Operation");
                var roleResult=await _rolemanager.FindByNameAsync(model.Name);
                if(roleResult is null)
                {
                    role.Name=model.Name;

                    var result = await _rolemanager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("", "Invalid Operation");
                
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {


                if (id != model.Id) return BadRequest("Invalid Operation");
                var role = await _rolemanager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalide Operation");
 

                    var result = await _rolemanager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                
                ModelState.AddModelError("", "Invalid Operation");


            }
            return View(model);
        }


    }
}
