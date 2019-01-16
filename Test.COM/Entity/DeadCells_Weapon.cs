using System;
using SqlSugar;

namespace Test.COM.Entity
{
    [SugarTable("deadcells_weapon")]
    public class DeadCells_Weapon
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Idx { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string Name { get; set; }
        public int TypeId { get; set; }
        public string Image { get; set; }
        public string Effect { get; set; }
        public string DesignPaper { get; set; }
        public string KillAbility { get; set; }
        public string BossAbility { get; set; }
        public string Evaluate { get; set; }
        public string BestAttribute { get; set; }
    }
}
