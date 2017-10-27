using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.COM.Entity
{
    public class SpotInfo
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
        /// 景点名称
        /// </summary>
        public string SpotName { get; set; }
        /// <summary>
        /// 景点id
        /// </summary>
        public int SpotId { get; set; }
        /// <summary>
        /// 票种名称
        /// </summary>
        public string TicketName { get; set; }
        /// <summary>
        /// 票种id
        /// </summary>
        public int TicketId { get; set; }
        /// <summary>
        /// 景点日期
        /// </summary>
        public DateTime TourDate { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public float UnitPrice { get; set; }
        /// <summary>
        /// 景点使用数量
        /// </summary>
        public float SpotQuantity { get; set; }
        /// <summary>
        /// 减免数量
        /// </summary>
        public float FreeQuantity { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public float TotalAmount { get; set; }
    }
}
