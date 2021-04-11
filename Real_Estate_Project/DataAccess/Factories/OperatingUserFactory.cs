using Real_Estate_Project.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.DataAccess.Factories
{
    public class OperatingUserFactory
    {
        public static Func<IOperatingUserRepo> CreateOperaitngUserRepo;

        public static IOperatingUserRepo GetOperatingUserRepo()
        {
            return CreateOperaitngUserRepo();
        }
    }
}