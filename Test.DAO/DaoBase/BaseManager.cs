using System;
using SqlSugarRepository;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Test.DAO.DaoBase
{
    public class BaseManager<T> : IBaseManager<T> where T : class, new ()
    {
        #region
        //private static readonly string mySqlConnetStr = "server=192.168.199.243;Database=test;Uid=root;Pwd=1qazXSW@";
        private static readonly string mySqlConnetStr =
            "server=47.94.160.77;user id=ngy001;password=fz917758;persistsecurityinfo=True;port=3306;database=freshly;SslMode=none";
        private readonly ISqlSugarClient _db = DbRepository.GetInstance(DbType.MySql, mySqlConnetStr);
        #endregion

        #region 通用方法
        /// <summary>
        /// 查询List
        /// </summary>
        public List<T> Query(Expression<Func<T, bool>> expression = null)
        {
            return (expression == null
                ? _db.Queryable<T>().ToList()
                : _db.Queryable<T>().Where(expression).ToList()) ?? new List<T>();
        }
        /// <summary>
        /// 获得实体
        /// </summary>
        public T Find(Expression<Func<T, bool>> expression)
        {
            return _db.Queryable<T>().Where(expression).FirstOrDefault() ?? new T();
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        public int Insert(T t)
        {
            var obj = _db.Insert(t);
            return Convert.ToInt32(obj == null || obj.ToString() == "true" ? 0 : obj);
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        public bool InsertList(List<T> list)
        {
            var obj = _db.InsertRange(list);
            return obj != null && obj.Any(n => n.ToString() == "true" || Convert.ToInt32(n) > 0);
        }
        /// <summary>
        /// 更新数据
        /// .Update(new { B = "BB" }, n => n.A == "LAS-P");
        /// </summary>
        public bool Update(object rowObj, Expression<Func<T, bool>> expression)
        {
            return _db.Update(rowObj, expression);
        }
        /// <summary>
        /// 查询sql语句
        /// .ExecutList("SELECT * FROM book WHERE IsDel = @isDel;", new {isDel = 0})
        /// </summary>
        public List<T> ExecutList(string sql, object whereObj = null)
        {
            return _db.SqlQuery<T>(sql, whereObj).ToList();
        }
        /// <summary>
        /// 执行sql
        /// .Execut("INSERT INTO book (`Name`, Remark, IsDel, CreateBy, CreateDate) VALUES (@Name, @Remark, @IsDel, @CreateBy, NOW());",
        ///     new
        ///     {
        ///         Name = "初中语文",
        ///         Remark = "没意思",
        ///         IsDel = false,
        ///         CreateBy = "张学友"
        ///     });
        /// </summary>
        public int Execut(string sql, object whereObj = null)
        {
            return _db.ExecuteCommand(sql, whereObj);
        }
        #endregion
    }
}
