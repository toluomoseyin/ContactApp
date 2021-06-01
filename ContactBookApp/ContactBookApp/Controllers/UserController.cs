using ContactBookApp.Models;
using ContactBookApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContactBookApp.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IContactRepository contact;

        public UserController(IContactRepository contact)
        {
            this.contact = contact;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("add-new")]
        public IActionResult AddnewContact([FromForm]ContactDto model)
        {
            if (model.PhotoFile.Length <= 0)
            {
                return BadRequest("Invalid file size");
            }
            if (ModelState.IsValid)
            {
                 contact.AddNewContact(model); 
                return Created("~api/User/add-new", model);
            }
            return BadRequest();
        }
        
       [AllowAnonymous]
        [HttpGet("Get-by-Id/{id}")]    
        public IActionResult ContactById(int id)
        {
            var user = contact.GetContactById(id);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest("User not found!");
        }


        [AllowAnonymous]
        [HttpGet("Get-by-Email/{Email}")]
        public IActionResult ContactByEmail([FromQuery]string Email)
        {
            var user = contact.GetContactByEmail(Email);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest("User not found!");
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("Delete-user-by-id/{Id}")]
        public IActionResult DeleteContact([FromQuery]int Id)
        {
            var result = contact.Delete(Id);
            if (!result)
            {
                return BadRequest("User does not exist!");
            }
            return Ok("User successfully removed!");

        }



        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUserId")]
        public IActionResult UpdateContact(int Id, [FromForm] ContactDto model)
        {
           
            var user = contact.UpdateContact(Id, model);
            if (user == null)
            {
                return BadRequest("user does not exist!");
            }
            return Ok(user);

        }



        [Authorize(Roles = "Admin")]
        [HttpGet("all-users")]
        public IActionResult AllUsers([FromQuery] UsersParameter usersParameter)
        {
            var users = contact.GetAllContacts(usersParameter);
            if (users != null)
            {
                return Ok(users);
            }
            return BadRequest("No user is Resgistered!");

        }





        [Authorize(Roles = "Admin")]
        [HttpGet("SearchUser")]
        public IActionResult SearchUser(string name, string state, string city)
        {
            var user = contact.Search(name, state, city);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest("No user matched the search term provided!");
        }

        [AllowAnonymous]
        [HttpPatch("UpdatePicture")]
        public IActionResult Patch([FromForm]PhotoToAddDto request, int Id)
        {
            if (request.PhotoFile==null)
            {
                return BadRequest("File Upload is empty");
            }
            var user = contact.UpdateProfilePic(request.PhotoFile, Id);
            if (user == null)
            {
                return BadRequest("User does not exist!");
            }
            return Ok(user);
        }

    }

}
