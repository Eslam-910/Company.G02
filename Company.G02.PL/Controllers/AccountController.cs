using Company.G02.BLL;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _user;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> user,SignInManager<AppUser> signInManager) 
        {
            _user = user;
            _signInManager = signInManager;
        }
        #region Sign Up
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {

            var usercheck=await _user.FindByNameAsync(model.UserName);
           
              if (ModelState.IsValid)
              {
                if (usercheck is null)
                {
                    usercheck = await _user.FindByEmailAsync(model.Email);
                    if (usercheck is null)
                    {
                        var user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.Igree,
                        };
                        var adduser = await _user.CreateAsync(user, model.Password);//PaSs@12

                        if (adduser.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        foreach (var Error in adduser.Errors)
                        {
                            ModelState.AddModelError("", Error.Description);
                        }
                    }
                } 
                ModelState.AddModelError("", " Invalid SignUp !!");
              }
                     
            
            return View();
        }
        #endregion

        #region Sign In
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
               var User=await _user.FindByEmailAsync(model.Email);
                if(User is not null)
                {
                  var flag=await _user.CheckPasswordAsync(User, model.Password);//PaSs@12
                    if (flag)
                    {
                        var result=await _signInManager.PasswordSignInAsync(User,model.Password,model.RememberMe,false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                        //Signin
                    }
                }
                ModelState.AddModelError("", "Invalid Login");
            }
            return View(model);
        }
        #endregion

        #region Sign Out

        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion
    }
}
