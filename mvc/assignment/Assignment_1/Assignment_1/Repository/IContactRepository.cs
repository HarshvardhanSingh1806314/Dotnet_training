using Assignment_1.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment_1.Repository
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetAllAsync();

        Task<Contact> GetByIdAsync(int Id);

        Task<Tuple<bool, string>> CreateAsync(Contact contact);

        Task<Tuple<bool, string>> UpdateAsync(Contact contact);

        Task<Tuple<bool, string>> DeleteAsync(int Id);
    }
}
