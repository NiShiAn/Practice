using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.COM.Entity
{
    public class TourPlan
    {
        /// <summary>
        /// 天数
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 游玩项目
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public decimal Mins { get; set; }
    }
}
