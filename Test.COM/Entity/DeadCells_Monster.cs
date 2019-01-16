using System;
using SqlSugar;

namespace Test.COM.Entity
{
    [SugarTable("deadcells_monster")]
    public class DeadCells_Monster
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Idx { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }
        public string Skill { get; set; }
        public string Map { get; set; }
        public string Note { get; set; }
    }
}
