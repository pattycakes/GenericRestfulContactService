using GenericService.Models;
using GenericService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericService.Repositories
{
    public class EmailRepository : BaseRepository, IEmailRepository
    {
        public EmailRepository(ContactDbContext context) : base(context)   {    }

        public async Task<Email> AddEmail(Email email)
        {
            Email oldEmail = await Context.Emails.FindAsync(email.Id);
            if (oldEmail == null)
            {
                Context.DetachAllEntities();  
                Context.Emails.Add(email);
                await Context.SaveChangesAsync();
            }
            return email;
        }

        public async Task<bool> DeleteEmail(long emailId)
        {
            Email email = Context.Emails.SingleOrDefault(e => e.Id == emailId);
            if (email == null)
            {
                throw new KeyNotFoundException();
            }

            email.IsDeleted = true;
            email.IsPrimary = false;
            return await UpdateEmail(email);
        }

        public async Task<IEnumerable<Email>> GetAllEmails()
        {
            return await Context.Emails.ToListAsync();
        }

        public async Task<Email> GetEmail(long emailId)
        {
            Email email = await Context.Emails.SingleOrDefaultAsync(e => e.Id == emailId);
            if (email == null)
            {
                throw new KeyNotFoundException();
            }

            return email;
        }

        public async Task<bool> UpdateEmail(Email email)
        {
            Email oldEmail = await GetEmail(email.Id);
            if (!email.Equals(oldEmail))
            {
                DateTime? originalDateCreated = oldEmail.Created;
                email.Created = originalDateCreated;

                if (ReferenceEquals(email, oldEmail))
                {
                    Context.Update(email);
                }
                else
                {
                    Context.Entry(oldEmail).CurrentValues.SetValues(email);
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
