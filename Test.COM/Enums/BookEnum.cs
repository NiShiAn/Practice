using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.COM.Enums
{
    public class BookEnum
    {
        public enum State
        {
            [Description("预售")]
            PreSale = 1,
            [Description("在售")]
            OnSale = 2,
            [Description("售完")]
            SaleOut = 3
        }
    }
}
