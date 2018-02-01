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
        public string Name { get; set; }
        public string Remark { get; set; }
        public bool IsDel { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
