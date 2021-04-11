using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Project.Models.Dashboard_Models.ChartClient
{
    public interface IBaseClient<T> : IDisposable
    {
        Task<ClientResponseModel> GetChartResult(T item);
    }
}
