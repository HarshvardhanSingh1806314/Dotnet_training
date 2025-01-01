using Assignment_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_1.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactContext _contactContext;

        public ContactRepository()
        {
            _contactContext = new ContactContext();
        }

        public async Task<List<Contact>> GetAllAsync()
        {
            return _contactContext.Contacts.ToList();
        }

        public async Task<Contact> GetByIdAsync(int Id)
        {
            return await _contactContext.Contacts.FindAsync(Id);
        }

        public async Task<Tuple<bool, string>> CreateAsync(Contact contact)
        {
            string message;
            try
            {
                Contact contactExist = _contactContext.Contacts.FirstOrDefault(c => c.Email == contact.Email);
                if(contactExist != null)
                {
                    throw new Exception("Contact Already Exists");
                }
                Contact newContact = _contactContext.Contacts.Add(contact);
                if(newContact == null)
                {
                    throw new Exception("Not Able To Add New Contact");
                }
                if(await _contactContext.SaveChangesAsync() > 0)
                {
                    message = "Contact Created Successfully";
                    return new Tuple<bool, string>(true, message);
                }
                else
                {
                    throw new Exception("Not Able To Create New Contact");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                message = ex.Message;
            }

            return new Tuple<bool, string>(false, message);
        }

        public async Task<Tuple<bool, string>> UpdateAsync(Contact contact)
        {
            string message;
            try
            {
                Contact contactExist = await _contactContext.Contacts.FindAsync(contact.Id);
                if(contactExist == null)
                {
                    throw new Exception("Contact Does Not Exist");
                }
                contactExist.FirstName = contact.FirstName;
                contactExist.LastName = contact.LastName;
                contactExist.Email = contact.Email;

                if(await _contactContext.SaveChangesAsync() > 0)
                {
                    message = "Contact Updated Successfully";
                    return new Tuple<bool, string>(true, message);
                }
                else
                {
                    throw new Exception("Contact Updation Failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                message = ex.Message;
            }

            return new Tuple<bool, string>(false, message);
        }

        public async Task<Tuple<bool, string>> DeleteAsync(int Id)
        {
            string message;
            try
            {
                Contact contact = await _contactContext.Contacts.FindAsync(Id);
                if(contact == null)
                {
                    throw new Exception("Contact You Want To Delete Does Not Exist");
                }
                _contactContext.Contacts.Remove(contact);
                if(await _contactContext.SaveChangesAsync() > 0)
                {
                    message = "Contact Deleted Successfully";
                    return new Tuple<bool, string>(true, message);
                }
                else
                {
                    throw new Exception("Contact Deletion Failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                message = ex.Message;
            }

            return new Tuple<bool, string>(false, message);
        }
    }
}