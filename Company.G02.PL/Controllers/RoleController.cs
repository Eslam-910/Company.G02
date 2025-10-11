using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using Company.G02.PL.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.G02.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager)
        {
            _rolemanager = roleManager;
            _userManager = userManager;
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
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role = await _rolemanager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();
            }
            ViewData["RoleId"]= roleId;

            var usersinrole = new List<UsersInRoleViewModel>();
            var users=await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userinrole = new UsersInRoleViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userinrole.IsSelected = true;
                }
                else
                {
                    userinrole.IsSelected= false;
                }
                usersinrole.Add(userinrole);    
            }

            return View(usersinrole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _rolemanager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();
            }
           if(ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appuser = await _userManager.FindByIdAsync(user.Id);
                    if (appuser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appuser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appuser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appuser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appuser, role.Name);
                        }
                    }

                }
                return RedirectToAction(nameof(Edit),new {id=roleId});
            }
            
            return View(users);

        }

    }
}
