using GenericService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericService.Repositories.Interfaces
{
    public interface IEmailRepository
    {
        Task<Email> GetEmail(long emailId);
        Task<IEnumerable<Email>> GetAllEmails();
        Task<Email> AddEmail(Email email);
        Task<bool> UpdateEmail(Email email);
        Task<bool> DeleteEmail(long emailId);
    }
}
