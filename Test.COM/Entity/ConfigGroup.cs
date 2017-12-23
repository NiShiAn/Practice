using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.COM.Entity
{
    public class Common_Config_Group
    {
        public int IDX { get; set; }
        public string ConfigurationKey { get; set; }
        public string ConfigurationName { get; set; }
        public string ConfigurationValue { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Sequence { get; set; }
        public int ParentID { get; set; }
        public int ConfigTypeID { get; set; }
        public string ConfigTypeName { get; set; }
        public string GroupId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
