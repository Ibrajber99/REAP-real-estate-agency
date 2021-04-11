using Real_Estate_Project.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.DataAccess.Factories
{
    public class CustomerRepoFactory
    {
        public static Func<ICustomerRepo> CreateCustomerRepo;

        public static ICustomerRepo GetCustomerRepo()
        {
            return CreateCustomerRepo();
        }
    }
}