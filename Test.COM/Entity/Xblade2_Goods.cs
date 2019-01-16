using System;
using SqlSugar;

namespace Test.COM.Entity
{
    [SugarTable("xblade2_goods")]
    public class Xblade2_Goods
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Idx { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public int ShopId { get; set; }
        public string Name { get; set; }
        public string Effect { get; set; }
        public string Material { get; set; }
        public string Desc { get; set; }
    }
}
