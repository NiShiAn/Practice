using System;
using SqlSugar;

namespace Test.COM.Entity
{
    [SugarTable("xblade2_shop")]
    public class Xblade2_Shop
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Idx { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string City { get; set; }
        public string Category { get; set; }
        public string Property { get; set; }
    }
}
