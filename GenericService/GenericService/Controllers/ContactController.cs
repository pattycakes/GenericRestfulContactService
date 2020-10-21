using AutoMapper;
using GenericService.Models;
using GenericService.Services.Interfaces;
using GenericService.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GenericService.Controllers
{
    /// <summary>
    /// REST endpoints for <see cref="Contact"/>
    /// </summary>
    [Route("v1/[controller]")]
    public class ContactController : ControllerBase
    {
        #region mapper stuff

        private readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(CreateMapper, LazyThreadSafetyMode.ExecutionAndPublication);

        private static IMapper CreateMapper()
        {
            MapperConfiguration config = new MapperConfiguration(Startup.InitializeMapperConfig);

            return config.CreateMapper();
        }

        protected IMapper Mapper => _mapper.Value;

        #endregion

        private IContactService ContactService { get; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="contactContactService"></param>
        public ContactController(IContactService contactContactService )
        {
            ContactService = contactContactService;
        }

        // GET v1/Contacts/4
        /// <summary>
        /// Get Contact by internal id
        /// </summary>
        /// <param name="id">A valid contact id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ContactView))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                return Ok(await ContactService.GetContact(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        /// <summary>
        /// Pushing a new contact into db (bypasses the review queue).
        /// </summary>
        /// <param name="contactView">A contact object to be created</param>     
        // POST v1/Contacts 
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ContactView))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Post([FromBody] ContactView contactView)
        {
            try
            {
                var contact = await ContactService.AddContact(Mapper.Map<Contact>(contactView));
                return CreatedAtAction(nameof(Get), new { id = contact.Id }, contact);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Pushing changes to contacts directly.
        /// </summary>
        /// <param name="id">A valid contact id</param>
        /// <param name="ContactView">The contact to be changed</param>
        // PUT v1/Contacts/5 
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> Put(long id, [FromBody] ContactView ContactView)
        {
            try
            {
                await ContactService.GetContact(id);

                Contact updatedDbContact = Mapper.Map<Contact>(ContactView);
                updatedDbContact.Id = id;
                return Ok(await ContactService.UpdateContact(updatedDbContact));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // DELETE v1/Contacts/5 
        /// <summary>
        /// removes a contact from the db
        /// </summary>
        /// <param name="id">A valid contact id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                bool result = (await ContactService.DeleteContact(id));
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
