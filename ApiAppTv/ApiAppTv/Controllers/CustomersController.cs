﻿using ApiAppTv.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAppTv.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customer.ToListAsync();
        }


        //------------------------------this-------------------------------
        [HttpGet("{serial}")]
        public async Task<ActionResult<Customer>> GetCustomer(string serial)
        {
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Serial == serial);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { serial = customer.Serial }, customer);
        }

        [HttpPut("{serial}")]
        public async Task<IActionResult> PutCustomer(string serial, Customer customer)
        {
            if (serial != customer.Serial)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(serial))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{serial}")]
        public async Task<IActionResult> DeleteCustomer(string serial)
        {
            var customer = await _context.Customer.FindAsync(serial);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(string serial)
        {
            return _context.Customer.Any(e => e.Serial == serial);
        }

    }
}
