using ContactBookApp.Models;
using ContactBookApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly SignInManager<IdentityUser> signInManager;
       
        public AuthController(IUserRepository userRepository, SignInManager<IdentityUser> signInManager)
        {
            this.userRepository = userRepository;
            this.signInManager = signInManager;
        }



        //This is the controller that handles registration
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromForm]User model)
        {
            if (model.PhotoFile==null)
            {
                return BadRequest("Invalid file size");
            }
            var result = await userRepository.RegisterUserAsync(model);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest("not successful!");
        }



        // the user logs in using this controller
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm]Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                   
                    var result1 = await userRepository.LoginUserAsync(model);
                    if (result1.IsSuccess)
                    {
                        return Ok(result1);
                    }
                }
                return BadRequest("Invalid Login Cridendentials");
               
               
            }
            return BadRequest("Some properties are not valid");

        }

      




    }
}
