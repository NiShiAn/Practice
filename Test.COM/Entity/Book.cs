using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.COM.Entity
{
    public class Book
    {
        public int Id { get; set; }
        /// <summary>
        /// 书名
        /// </summary>
        public string BookName { get; set; }
        /// <summary>
        /// 作者id
        /// </summary>
        public int AutherId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 销量
        /// </summary>
        public int Sales { get; set; }
    }
    public class Auther
    {
        public int Id { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string AutherName { get; set; }
    }

    public class Library
    {
        public string Address { get; set; }
        /// <summary>
        /// 书本
        /// </summary>
        public List<Book> BookList { get; set; }
    }
}
