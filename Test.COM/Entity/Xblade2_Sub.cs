using System;
using SqlSugar;

namespace Test.COM.Entity
{
    [SugarTable("xblade2_sub")]
    public class Xblade2_Sub
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Idx { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public int Xkey { get; set; }
        public string Equib { get; set; }
        public string Genrn { get; set; }
        public bool IsReach { get; set; }
    }
}
