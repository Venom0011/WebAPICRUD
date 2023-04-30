using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using WebAPICrudApplication.Models;

namespace WebAPICrudApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public readonly CustomerContext _dbcontext;

        public CustomerController(CustomerContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            if (_dbcontext.Customers == null)
            {
                return NotFound();
            }
            return await _dbcontext.Customers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
            if (_dbcontext.Customers == null)
            {
                return NotFound();
            }
            var customer= await _dbcontext.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _dbcontext.Customers.Add(customer);
            await _dbcontext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomer),new { id = customer.Id },customer);
        }

        [HttpPut]
        public async Task<ActionResult<Customer>> PutCustomer(int id, Customer customer)
        {
            if (id!=customer.Id)
            {
                return BadRequest();
            }

            _dbcontext.Entry(customer).State = EntityState.Modified;
            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }
        private bool CustomerAvailable(int id)
        {
            return (_dbcontext.Customers?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            if (_dbcontext.Customers == null)
            {
                return NotFound();
            }
            var customer = await _dbcontext.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            _dbcontext.Customers.Remove(customer);
            await _dbcontext.SaveChangesAsync();
            return Ok();
        }
    }
}
