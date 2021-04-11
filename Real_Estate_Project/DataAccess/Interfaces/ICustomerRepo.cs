using Real_Estate_Project.DataAccess.Sql_Classes;
using Real_Estate_Project.Models.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Project.DataAccess.Interfaces
{
    public interface ICustomerRepo : ICrudRepo<Customer>,IDisposable
    {

    }
}
