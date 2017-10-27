using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Test.COM;
using Test.COM.Entity;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private IExportManager exportManager = new ExportManager();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Box()
        {
            return View();
        }

        public ActionResult Table()
        {
            return View();
        }

        public ActionResult Verify()
        {
            return View();
        }

        public ActionResult Export()
        {

            return View(GetSpotList());
        }

        public ActionResult ExportTable(int type)
        {
            try
            {
                var list = GetSpotList();

                var colums1 = new Dictionary<string, string>()
                {
                    { "团队ID", "TourNo"},
                    { "团队名称", "TourName"},
                    { "景点名称", "SpotName"},
                    { "票种", "TicketName"},
                    { "时间", "TourDate"},
                    { "单价", "UnitPrice"},
                    { "数量", "SpotQuantity"},
                    { "减免", "FreeQuantity"},
                    { "总金额", "TotalAmount"}
                };
                var colums2 = new Dictionary<string, string>()
                {
                    { "团队ID", "TourNo"},
                    { "团队名称", "TourName"},
                    { "景点名称", "SpotName"},
                    { "票种", "TicketName"},
                    { "时间", "TourDate"},
                    { "单价", "UnitPrice"},
                    { "数量", "SpotQuantity"},
                    { "减免", "FreeQuantity"},
                };
                MemoryStream stream;
                switch (type)
                {
                    case 1:
                        stream = exportManager.ExportExcelForList(
                            new Tuple<string, Dictionary<string, string>, List<SpotInfo>>("麦兜好可爱", colums1, list), true);
                        break;
                    case 2:
                        var tupleList1 = new List<Tuple<string, Dictionary<string, string>, DataTable>>()
                        {
                            new Tuple<string, Dictionary<string, string>, DataTable>("Life's A Struggle", colums2, ToDataTable(list)),
                        };
                        stream = exportManager.ExportExcelBatchDataTable(tupleList1, true);
                        break;
                    default:
                        var tupleList2 = new List<Tuple<string, Dictionary<string, string>, DataTable>>()
                        {
                            new Tuple<string, Dictionary<string, string>, DataTable>("精神可嘉", colums1, ToDataTable(list)),
                            new Tuple<string, Dictionary<string, string>, DataTable>("不错de", colums2, ToDataTable(list)),
                        };
                        stream = exportManager.ExportExcelBatchDataTable(tupleList2, true);
                        break;
                }

                return File(stream, "application/vnd.ms-excel", "景点资源(" + DateTime.Now.ToString("yyyy-MM-dd") + ").xlsx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<SpotInfo> GetSpotList()
        {
            var list = new List<SpotInfo>();

            for (var i = 0; i < 6; i++)
            {
                list.Add(new SpotInfo()
                {
                    TourNo = "TMS20170802S00" + i,
                    TourId = "170805100" + i,
                    TourName = "HDA-U_0805_02030" + i,
                    SpotName = "千岛湖好运岛",
                    SpotId = 2005,
                    TicketName = "成人票",
                    TicketId =1,
                    TourDate = Convert.ToDateTime("2017-08-02").AddDays(i),
                    UnitPrice = 20,
                    SpotQuantity =14,
                    FreeQuantity =1,
                    TotalAmount =260
                });
            }

            return list;
        }

        #region List转成DataTable
        private DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                var t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                return Nullable.GetUnderlyingType(t);
            }
            return t;
        }
        #endregion
    }
}