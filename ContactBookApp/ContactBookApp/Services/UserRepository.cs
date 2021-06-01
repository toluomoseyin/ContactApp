using ContactBookApp.Data;
using ContactBookApp.Models;
using ContactBookApp.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Services
{
    public class UserRepository:IUserRepository
    {
        private readonly UserManager<IdentityUser> userManager;

        private IConfiguration Configuration;
        private readonly ContactBookDbContext db;

        public UserRepository(UserManager<IdentityUser> userManager, IConfiguration configuration, ContactBookDbContext db)
        {
            this.userManager = userManager;
            Configuration = configuration;
            this.db = db;
        }

       

        public async Task<ManagerResponse> RegisterUserAsync(User model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Register model is null");
            }
            if (model.Password != model.ConfirmPassword)
            {
                return new ManagerResponse
                {
                    Message = "Password Mismatch: Confirm Password doesn't Match Password",
                    IsSuccess = false,
                };
            }
            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await userManager.CreateAsync(identityUser,model.Password);
            var role = await userManager.AddToRoleAsync(identityUser, "Regular");
           
            if (result.Succeeded && role.Succeeded)
            {
                var cloudPic = new CloudinaryUpload(Configuration);
                var para = cloudPic.UploadMyPic(model.PhotoFile);
                var contact = new Contact
                {
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    City = model.City,
                    State = model.State,
                    PhotoUrl= para[0],
                };
                db.Contacts.Add(contact);
                db.SaveChanges();
               
                return new ManagerResponse
                {
                    Message = "User created succesfully!",
                    IsSuccess = true,
                    
                };
            }
            return new ManagerResponse
            {
                Message = "Not able to register user",
                IsSuccess = false,
            };


        }


        // the user logs in using this method
        public async Task<ManagerResponse> LoginUserAsync(Login model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var roles = await userManager.GetRolesAsync(user);
            if (user == null)
            {
                return new ManagerResponse
                {
                    Message = $"There is no user with this email {model.Email} address",
                    IsSuccess = false,
                };
            }
            var result = await userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                return new ManagerResponse
                {
                    Message = $"{model.Password} is an Invalid Password",
                    IsSuccess = false,
                };
            }

            var Claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id),

               
            };
            foreach (var role in roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSetting:Key"]));
            var token = new JwtSecurityToken
            (
                
                 claims: Claims,
                 expires: DateTime.Now.AddDays(30),
                 signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)

            );
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            return new ManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpiryDate = token.ValidFrom

            };
        }
    }
}
