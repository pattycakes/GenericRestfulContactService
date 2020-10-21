using GenericService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericService.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllContacts();
        Task<Contact> GetContact(long contactId);
        Task<Contact> AddContact(Contact contact);
        Task<bool> UpdateContact(Contact contact);
        Task<bool> DeleteContact(long contactId);
    }
}
