using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.COM.Entity
{
    public class TourInfo
    {
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 团队Id
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 团队名称
        /// </summary>
        public string TourName { get; set; }
        /// <summary>
        /// 团队日期
        /// </summary>
        public DateTime TourDate { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNumber { get; set; }
    }
}
