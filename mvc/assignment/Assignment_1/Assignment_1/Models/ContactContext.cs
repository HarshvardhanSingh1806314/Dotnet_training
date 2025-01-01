using System.Data.Entity;

namespace Assignment_1.Models
{
    public class ContactContext : DbContext
    {
        public ContactContext() : base("name = dbcs")
        {

        }

        public DbSet<Contact> Contacts { get; set; }
    }
}