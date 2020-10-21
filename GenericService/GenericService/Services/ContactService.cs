using GenericService.Models;
using GenericService.Repositories.Interfaces;
using GenericService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericService.Services
{
    public class ContactService : IContactService
    {

        private readonly IContactRepository _contactRepo;
        private readonly IEmailRepository _emailRepo;

        public async Task<Contact> AddContact(Contact contact)
        {
            return await _contactRepo.AddContact(contact);
        }

        public async Task<bool> DeleteContact(long contactId)
        {
            return await _contactRepo.DeleteContact(contactId);
        }

        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
            return await _contactRepo.GetAllContacts();
        }

        public async Task<Contact> GetContact(long contactId)
        {
            return await _contactRepo.GetContact(contactId);
        }

        public async Task<bool> UpdateContact(Contact contact)
        {
            return await _contactRepo.UpdateContact(contact);
        }
    }
}
