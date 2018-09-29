using System;
using SqlSugar;

namespace Test.COM.Entity
{
    [SugarTable("xblade2")]
    public class Xblade2
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Idx { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string Name { get; set; }
        public string Role { get; set; }
        public string Partner { get; set; }
        public string Fetter { get; set; }
        public string Target { get; set; }
    }
}
