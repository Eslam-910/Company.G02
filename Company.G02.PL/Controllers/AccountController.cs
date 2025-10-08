using Company.G02.BLL;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Company.G02.PL.Controllers
{
    //PaSs@12
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

        #region ForgotPassword
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> ResetPasswordForUrl( ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user =await _user.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                   var token=await _user.GeneratePasswordResetTokenAsync(user);
                    //Create Url
                    var URL= Url.Action("ResetPassword", "Account", new {model.Email,token},Request.Scheme);
                    //Create Email
                    var email = new Helpers.Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = URL
                    };

                    //send Email
                    var flag = Helpers.EmailSettings.SendEmail(email);
                    if (flag)
                    {
                        //Check inbox
                        return Redirect("CheckYourInbox");
                    }


                }
                

            }
            ModelState.AddModelError("", "Invalid Reset Password !!");
            return View(nameof(SignIn));
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion

        #region Reset Password

        #endregion
    }
}
