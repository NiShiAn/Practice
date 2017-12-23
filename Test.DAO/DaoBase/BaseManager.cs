using System;
using SqlSugarRepository;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Test.DAO.DaoBase
{
    public class BaseManager<T> : IBaseManager<T> where T : class, new ()
    {
        #region
        private static readonly string mySqlConnetStr = "server=192.168.199.243;Database=test;Uid=root;Pwd=1qazXSW@";
        private readonly ISqlSugarClient _db = DbRepository.GetInstance(DbType.MySql, mySqlConnetStr);
        #endregion

        #region
        /// <summary>
        /// 查询全部数据
        /// </summary>
        public List<T> GetAll()
        {
            return _db.Queryable<T>().ToList() ?? new List<T>();
        }
        /// <summary>
        /// 获得单个实体
        /// </summary>
        public T Find(Expression<Func<T, bool>> expression)
        {
            return _db.Queryable<T>().Where(expression).FirstOrDefault() ?? new T();
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        public bool Insert(T t)
        {
            return (string) _db.Insert(t) == "true";
        }
        /// <summary>
        /// 更新数据
        /// .Update(new { B = "BB" }, n => n.A == "LAS-P");
        /// </summary>
        public bool Update(object rowObj, Expression<Func<T, bool>> expression)
        {
            return _db.Update(rowObj, expression);
        }
        #endregion
    }
}
