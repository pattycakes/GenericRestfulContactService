using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericService.Repositories
{
    public class BaseRepository
    {
        public ContactDbContext Context { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">owned by this repository, and will be disposed of</param>
        protected BaseRepository(ContactDbContext context)
        {
            Context = context;
        }
    }
}
