using ContactBookApp.Data;
using ContactBookApp.Models;
using ContactBookApp.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookApp.Services
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactBookDbContext db;
        private readonly IConfiguration configuration;

        public ContactRepository(ContactBookDbContext Db, IConfiguration configuration)
        {
            db = Db;
            this.configuration = configuration;
        }


        //This method adds a new contact 
        public void AddNewContact(ContactDto model)
        {
            var Upload = new CloudinaryUpload(configuration);
            var upload = Upload.UploadMyPic(model.PhotoFile);

            var contact = new Contact
            {
                State = model.State,
                City = model.City,
                Email = model.Email,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                PhotoUrl = upload[0]
            };
           
            db.Contacts.Add(contact);
            db.SaveChanges();
        }

        // Deletes a contact from the contacts
        public bool Delete(int Id)
        {
            var contact = db.Contacts.Find(Id);
            if (contact == null)
            {
                return false;
            }
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return true;
        }

        public List<Contact> GetContactByEmail(string Email)
        {
            var contact = db.Contacts.Where(s=>s.Email==Email).ToList();
            return contact;
        }

        public Contact GetContactById(int Id)
        {
            var contact = db.Contacts.Find(Id);
            return contact;
        }

        // return contacts in pages or number
        public List<Contact> GetAllContacts(UsersParameter usersParameter)
        {
            var contacts = db.Contacts.OrderBy(on => on.Name)
                    .Skip((usersParameter.PageNumber - 1) * usersParameter.PageSize)
                    .Take(usersParameter.PageSize)
                    .ToList();
            return contacts;

        }



        // updates one or more of the detail of the contact yes
        public Contact UpdateContact(int Id, ContactDto model)
        {
            
            var user = db.Contacts.Find(Id);
            if (!string.IsNullOrEmpty(model.Name))
            {
                user.Name = model.Name;
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                user.City = model.City;
            }
            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                user.PhoneNumber = model.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                user.State = model.State;
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                user.Email = model.Email;
            }
            if (model.PhotoFile!=null)
            {
                var pic = new CloudinaryUpload(configuration);
                var url = pic.UploadMyPic(model.PhotoFile);
                user.PhotoUrl = url[0];
            }  
            db.SaveChanges();
            return user;
        }


        // searches the contact by name or state or city
        public IQueryable<Contact> Search(string name, string state, string city)
        {
            IQueryable<Contact> query = db.Contacts;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(state))
            {
                query = query.Where(e => e.State.Contains(state));
            }
            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(e => e.City.Contains(city));
            }
            return query;
        }


        // this method is used to ypdate the profile picture of a contact
        public Contact UpdateProfilePic(IFormFile formFile, int Id)
        {        
            var user = db.Contacts.Find(Id);
            if (user != null)
            {
                var pic = new CloudinaryUpload(configuration);
                var Url = pic.UploadMyPic(formFile);
                user.PhotoUrl = Url[0];
                db.SaveChanges();
            }

            return user;
        }


    }
}
