﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Test.COM;
using Test.COM.Comparer;
using Test.COM.Entity;
using Test.COM.Enums;
using Test.COM.Tool;
using Test.DAO;
using Test.DAO.Book;
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
            //FactoryFun();
            //StrSort();
            //ListJoin();
            //new linq().Where();
            //StrDateTime();
            //NumWeeks(new DateTime(2018, 7, 18));
            //Expect();
            DateCount();
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
        /// <summary>
        /// 时区转换
        /// </summary>
        protected static void LocalTime()
        {
            var now = DateTime.Now;
            var date = System.Console.ReadLine();
            var old = Convert.ToDateTime(date);

            if (old < now || (old - now).TotalDays <= 180)
            {
                System.Console.WriteLine("不在有效期内");
            }
            else
            {
                System.Console.WriteLine("满足条件了");
            }
            System.Console.ReadKey();



            //var uis = DateTime.UtcNow;
            //System.Console.WriteLine("世界时间：" + uis);
            //System.Console.WriteLine("服务器时区时间：" + uis.ToLocalTime());
            //System.Console.WriteLine("北京时间：" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(uis, "China Standard Time"));

            //System.Console.ReadKey(true);
        }
        /// <summary>
        /// 指定顺序排序
        /// </summary>
        protected static void StrSort()
        {
            //var ary = ("H,D,A,K,D,C").Split(',').ToList(); 
            //var neg = new [] { "B", "A", "D", "C" };

            //ary = ary.Where(n => neg.Contains(n)).Distinct().ToList();

            //foreach (var s in neg.OrderBy(n => ary.IndexOf(n) < 0 ? ary.Count + 1 : ary.IndexOf(n)))
            //{
            //    System.Console.WriteLine(s);
            //}
            //System.Console.WriteLine(string.Join(",", neg.OrderBy(n => n)));
            //System.Console.ReadKey(true);

            //string[] arys = { "1", "45B", "45A", "3", "7H", "LA3", "LA1", "LB5", "15" };
            //foreach (var s in arys.OrderBy(n => n, new StrNumberComparer()))
            //{
            //    System.Console.WriteLine(s);
            //}
            //var s = new TourInfo()
            //{
            //    PeopleNumber = 4,
            //    TourDate = DateTime.Now,
            //    TourId = "123456",
            //    TourName = "厚积薄发",
            //    TourNo = "DW_111"
            //};
            //var ss = CopyObject<TourInfo, TourInfo>.Trans(s);
            System.Console.ReadKey(true);
        }

        protected static void ListJoin()
        {
            var l1 = new BaseManager<Book>().Query(n => n.Id > 3);
            var l2 = new List<Sheet1>()
            {
                new Sheet1() {A = "9", B = "SFB"}
            };

            //var l3 = (from a in l1
            //    join b in l2 on new {d1 = a.Id, d2 = a.Name} equals new {d1 = b.A.ToInt(), d2 = b.B}
            //    select a).ToList();

        }

        protected static void FactoryFun()
        {
            //var str =
            //    "VC|MA7|Y4|MB9|YZ8|YM8|YW8|YS7|YL9|AW7|AC5|ANM|BGC|BVC|BAC|BLV|BVA|BGA|GCN|VGC|VAC|LV|AGC|AVC|AAC|ALV|ASF|SFA|SFB|LT3|US|CT|DL|DA|SD|SW|LG|WW|PS";
            //var sortList = str.Split('|').ToList();

            //var list = new BaseManager<Book>().Query(n => n.Id > 3);

            //list = list.OrderBy(n => sortList.IndexOf(n.Name) < 0 ? sortList.Count + 1 : sortList.IndexOf(n.Name)).
            //    ThenBy(g => g.Remark, new StrNumberComparer()).ToList();
            //list = list.OrderBy(n => n.Remark, new StrNumberComparer()).ToList();

            //var ctx = new Container();

            //ctx.Register<Book>();
            var list = Common.Enum2List<BookEnum.State>("1");
            System.Console.ReadKey(true);
        }

        protected static void StrDateTime()
        {
            //var strAry = new[] { "20180201", "10:20:19" };
            //var sDate = "";
            //foreach(var str in strAry)
            //{
            //    if (str.Contains(":"))
            //    {
            //        sDate += $" {str}";
            //    }
            //    else
            //    {
            //        var s1 = str.Substring(0, 4);
            //        var s2 = str.Substring(4, 2);
            //        var s3 = str.Substring(6, 2);
            //        sDate += $"{s1}/{s2}/{s3}";
            //    }
            //}
            //var date = Convert.ToDateTime(sDate);
            //System.Console.ReadKey();

        }
        /// <summary>
        /// 获得本月有几周
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static void NumWeeks(DateTime dt)
        {
            //年
            int year = dt.Year;
            //月
            int month = dt.Month;
            //当前月第一天
            DateTime weekStart = new DateTime(year, month, 1);
            //该月的最后一天
            DateTime monEnd = weekStart.AddMonths(1).AddDays(-1);
            int i = 1;
            //当前月第一天是星期几
            int dayOfWeek = Convert.ToInt32(weekStart.DayOfWeek.ToString("d"));
            //该月第一周结束日期
            DateTime weekEnd = dayOfWeek == 0 ? weekStart : weekStart.AddDays(7 - dayOfWeek);

            System.Console.WriteLine("第" + i + "周起始日期： " + weekStart.ToShortDateString() + "   结束日期： " + weekEnd.ToShortDateString());

            //当日期小于或等于该月的最后一天
            while (weekEnd.AddDays(1) <= monEnd)
            {
                i++;
                //该周的开始时间
                weekStart = weekEnd.AddDays(1);
                //该周结束时间
                weekEnd = weekEnd.AddDays(7) > monEnd ? monEnd : weekEnd.AddDays(7);

                System.Console.WriteLine("第" + i + "周起始日期： " + weekStart.ToShortDateString() + "   结束日期： " + weekEnd.ToShortDateString());
            }
            System.Console.WriteLine(year + "年" + month + "月共有" + i + "周");
            System.Console.ReadKey();
        }

        public static void Expect()
        {
            var ary1 = new List<int>(){ 1, 2, 7, 8, 6, 4 };
            var ary2 = new List<int>(){ 4, 3, 8, 9 };
            var ary3 = new List<int>(){ 7, 2, 8, 1, 5 };
            //1,2,3,4,7,9

            var t1 = ary1.Except(ary3).Count();
            var t2 = ary1.All(n => ary3.Contains(n));

            //var ary3 = ary2.Except(ary1);
            //var ary4 = ary1.Except(ary2);

            //var ary5 = ary3.Union(ary4).OrderBy(n => n);

            var dicList = new Dictionary<int, List<int>>()
            {
                {1, ary1 },
                {2, ary2 },
                {3, ary3 },
            };

            var rows = dicList.Max(n => n.Value.Count);
        }

        public static void DateCount()
        {
            var date = DateTime.Now.AddMonths(-1*10);

            var sd = (DateTime.Now - date).Days <= 180;
        }
    }
}
