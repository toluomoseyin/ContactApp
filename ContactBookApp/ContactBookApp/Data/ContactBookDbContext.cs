using ContactBookApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookApp.Data
{
    public class ContactBookDbContext : IdentityDbContext<IdentityUser>
    {
        public ContactBookDbContext(DbContextOptions<ContactBookDbContext> dbContextOptions): base(dbContextOptions)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
