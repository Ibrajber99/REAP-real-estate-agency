using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.Models.Dashboard_Models.ChartClient
{
    public static class BaseClientFactory<T>
    {
        public static Func<IBaseClient<T>> CreateBaseClient;

        public static IBaseClient<T> GetBaseClientRepo()
        {
            return CreateBaseClient();
        }
    }
}