using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.DAO.DaoBase
{
    public interface IBaseManager<T>
    {
        List<T> GetAll();
        T Find(Expression<Func<T, bool>> expression);
        bool Insert(T t);
        bool Update(object rowObj, Expression<Func<T, bool>> expression);
    }
}
