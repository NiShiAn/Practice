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
        List<T> Query(Expression<Func<T, bool>> expression = null);
        T Find(Expression<Func<T, bool>> expression);
        int Insert(T t);
        bool InsertList(List<T> list);
        bool Update(object rowObj, Expression<Func<T, bool>> expression);
        List<T> ExecutList(string sql, object whereObj = null);
        int Execut(string sql, object whereObj = null);
    }
}
