using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Test.COM.Entity;
using Test.DAO.DaoBase;

namespace Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //StrIntercept();
            //TimeSerch();
            //LocalTime();
            FactoryFun();
        }
        /// <summary>
        /// 最大宽度换行
        /// </summary>
        protected static void StrIntercept()
        {
            var str = "中国,bBb,cCcCcc,ddDdDdDd,ee,fFff,ggGggGg,hhhhhhhhhhhhhhhh,iiiiIIIIIiii";
            if (str.Length < 25)
            {
                System.Console.WriteLine(str);
            }
            var n = "";
            foreach (var s in str.Split(','))
            {
                var k = n == "" ? n + s : n + "," + s;
                if (k.Length > 25)
                {
                    System.Console.WriteLine(n);
                    n = s;
                }
                else
                {
                    n = k;
                }
            }
            if (!string.IsNullOrEmpty(n))
            {
                System.Console.WriteLine(n);
            }
            System.Console.ReadKey(true);
        }
        /// <summary>
        /// 12制时间转换
        /// </summary>
        protected static void TimeSerch()
        {
            var list = new List<string>()
            {
                "8:00 AM",
                "9:10 AM",
                "png",
                "",
                "4:10 pm",
                "8:20",
            };
            var sd = new List<TimeSpan>();
            var ss = ("8:11 am").ToUpper();
            var d1 = Convert.ToDateTime("8:11 AM").TimeOfDay;
            var d2 = Convert.ToDateTime("7:25 PM").TimeOfDay;
            var d3 = Convert.ToDateTime("12:25 PM").TimeOfDay;
            var d4 = Convert.ToDateTime("7:10 am").TimeOfDay;
            var d5 = Convert.ToDateTime("6:0 pm").TimeOfDay;
            foreach (var info in list)
            {
                if (Regex.IsMatch(info, @"^([1-9]|1[0-2]|0[1-9]):([0-5]\d|\d)(|\s)(AM|PM|am|pm|)$"))
                {
                    sd.Add(Convert.ToDateTime(info).TimeOfDay);
                }

                //if (Regex.IsMatch(info, @"^([1-9]|1[0-2]|0[1-9]):([0-5]\d|\d)(|\s)(AM|PM|am|pm|)$"))
                //{
                //    if (info.Contains("pm"))
                //    {
                //        sd.Add(Convert.ToDateTime(info.Replace("pm", "")).AddHours(12).TimeOfDay);
                //    }
                //    else
                //    {
                //        sd.Add(Convert.ToDateTime(info.Replace("am", "").Trim()).TimeOfDay);
                //    }
                //}
            }
            foreach (var timeSpan in sd.OrderBy(n => n))
            {
                System.Console.WriteLine(timeSpan.ToString());
            }
            System.Console.ReadKey(true);
        }

        protected static void LocalTime()
        {
            var uis = DateTime.UtcNow;
            System.Console.WriteLine("世界时间：" + uis);
            System.Console.WriteLine("服务器时区时间：" + uis.ToLocalTime());
            System.Console.WriteLine("北京时间：" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(uis, "China Standard Time"));

            System.Console.ReadKey(true);
        }

        protected static void FactoryFun()
        {
            var manager = new BaseManager<Sheet1>();

            //var list = manager.GetAll();
            //var model = manager.Find(n => n.A == "M9S");
            //var s = manager.Insert(new Sheet1()
            //{
            //    A = "A1",
            //    B = "B1",
            //    C = "C1",
            //    D = "D1"
            //});
            var ss = manager.Update(new { B = "BB" }, n => n.A == "LAS-P");
        }
    }
}
