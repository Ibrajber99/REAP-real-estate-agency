using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Project.DataAccess.Sql_Classes
{
    public interface ICrudRepo<T>
    {
        Task<List<T>> GetAll();

        Task<T> Get(int id);

        Task<int> Create(T item);

        Task<int> Update(T item);

        Task<int> Delete(T item);

        Task<int> Delete(int id);
    }
}
