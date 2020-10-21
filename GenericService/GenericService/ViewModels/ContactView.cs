using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericService.ViewModels
{
    public class ContactView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<EmailView> Emails { get; set; }
    }
}
