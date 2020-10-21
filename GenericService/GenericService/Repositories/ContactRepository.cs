using GenericService.Models;
using GenericService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericService.Repositories
{
    public class ContactRepository : BaseRepository, IContactRepository
    {       

        public ContactRepository(ContactDbContext context) : base(context)
        {
        }

        public async Task<Contact> AddContact(Contact contact)
        {
            Contact oldContact = await Context.Contacts.FindAsync(contact.Id);
            if (oldContact == null)
            {
                contact = Context.Contacts.Add(contact).Entity;
                await Context.SaveChangesAsync();
            }
            return contact;
        }

        public async Task<bool> DeleteContact(long contactId)
        {
            Contact contact = await Context.Contacts.FindAsync(contactId);
            if (contact == null)
            {
                throw new KeyNotFoundException();
            }

            Context.Contacts.Remove(contact);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
            List<Contact> contacts = await Context.Contacts
                  .Include(c => c.Emails)
                  .ToListAsync();
            return contacts;
        }

        public async Task<Contact> GetContact(long contactId)
        {
            Contact contact = await Context.Contacts
               .Include(c => c.Emails)
               .SingleOrDefaultAsync(c => c.Id == contactId);
            if (contact == null)
            {
                throw new KeyNotFoundException();
            }
            return contact;
        }

        public async Task<bool> UpdateContact(Contact contact)
        {
            Contact originalContact = await GetContact(contact.Id);
            if (!contact.Equals(originalContact))
            {
                DateTime? originalDateCreated = originalContact.Created;
                contact.Created = originalDateCreated;

                if (ReferenceEquals(contact, originalContact))
                {
                    Context.Update(contact);
                }
                else
                {
                    Context.Entry(originalContact).CurrentValues.SetValues(contact);
                }

                return await Context.SaveChangesAsync() > 0;
            }
            else
            {
                return true;
            }
        }
    }
}
