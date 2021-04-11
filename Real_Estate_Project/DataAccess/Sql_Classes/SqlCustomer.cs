
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.Models;
using Real_Estate_Project.Models.Domain_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Real_Estate_Project.DataAccess.Sql_Classes
{
    public class SqlCustomer : ICustomerRepo
    {
        private ApplicationDbContext _customerContext;

        public SqlCustomer()
        {
            _customerContext = new ApplicationDbContext();
        }

        public async Task<int> Create(Customer customer)
        {
           
            _customerContext.Customers.Add(customer);
            return await _customerContext.SaveChangesAsync();
        }


        public async Task<Customer> Get(int id)
        {
            var customer = _customerContext.Customers
                        .Include(u => u.UserUpdator)
                        .Include(u => u.UserCreator)
                        .Where(cus => cus.ID == id);

            return await customer.FirstOrDefaultAsync();
        }


        public async Task<List<Customer>> GetAll()
        {
            var customers = _customerContext.Customers
                .Where(cus => cus.IsActive == true);

            return await customers.AsNoTracking().ToListAsync();
        }


        public async Task<int> Update(Customer customer)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Customers.Attach(customer);

                context.Entry(customer.CustomerAddress).State 
                    = EntityState.Modified;

                context.Entry(customer).State = EntityState.Modified;

                return await context.SaveChangesAsync();
            }
        }


        public async Task<int> Delete(Customer customer)
        {
            _customerContext.Entry(customer).State
                = EntityState.Modified;

            return await _customerContext.SaveChangesAsync();
        }


        public async Task<int> Delete(int id)
        {
            var customer = _customerContext.Customers.
                            FirstOrDefault(x => x.ID == id);

            return await Delete(customer);
        }

        public void Dispose()
        {
            if(_customerContext != null)
            {
                _customerContext.Dispose();
            }
        }
    }
}