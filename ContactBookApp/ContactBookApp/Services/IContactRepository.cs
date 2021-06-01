using ContactBookApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookApp.Services
{
    public interface IContactRepository
    {
        List<Contact> GetAllContacts(UsersParameter usersParameter);
        Contact GetContactById(int Id);
        List<Contact> GetContactByEmail(string Email);
        void AddNewContact(ContactDto model);
        Contact UpdateContact( int Id,ContactDto model);
        bool Delete(int Id);
        IQueryable<Contact> Search(string name, string state, string city);
        Contact UpdateProfilePic(IFormFile formFile, int Id);




    }
}
