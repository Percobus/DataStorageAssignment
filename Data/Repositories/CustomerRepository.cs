using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Data.Repositories
{
    public class CustomerRepository
    {
        private readonly DataContext _context;

        public CustomerRepository(DataContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<bool> CreateAsync(CustomerEntity customer)
        {
            if (customer == null)
                return false;

            try
            {
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating customer entity :: {ex.Message}");
                return false;
            }
        }

        // READ
        public async Task<CustomerEntity?> GetByIdAsync(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CustomerEntity>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        // UPDATE
        public async Task<bool> UpdateAsync(CustomerEntity customer)
        {
            if (customer == null)
                return false;

            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);
            if (existingCustomer == null)
                return false;

            try
            {
                existingCustomer.CustomerName = customer.CustomerName;


                _context.Customers.Update(existingCustomer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating customer entity :: {ex.Message}");
                return false;
            }
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return false;

            try
            {
                _context.Customers.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting customer entity :: {ex.Message}");
                return false;
            }
        }
    }
}