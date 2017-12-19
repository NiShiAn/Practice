using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Newtonsoft.Json;
using Test;
using Test.COM;
using Test.COM.Entity;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExportManager _exportManager;

        public HomeController(IExportManager exportManager)
        {
            _exportManager = exportManager;
        }
        #region 功能页面
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

        public ActionResult Row()
        {
            return View();
        }

        public ActionResult Import()
        {
            return View();
        }

        public ActionResult Column()
        {
            return View();
        }

        public ActionResult Carousel()
        {
            return View();
        }
        #endregion

        #region 页面回调
        /// <summary>
        /// 导出表格
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult ExportTable(int type)
        {
            try
            {
                var tour = GetTourList();
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
                var colums3 = new Dictionary<string, string>()
                {
                    { "团队ID", "TourNo"},
                    { "团队日期", "TourDate"},
                    { "团队人数", "PeopleNumber"}
                };
                MemoryStream stream;
                switch (type)
                {
                    case 1:
                        stream = AutofacConfig.GetContainer().Resolve<IExportManager>().ExportExcelForList(
                            new Tuple<string, Dictionary<string, string>, List<SpotInfo>>("麦兜好可爱", colums1, list), true);
                        break;
                    case 2:
                        var tupleList1 = new List<Tuple<string, Dictionary<string, string>, DataTable>>()
                        {
                            new Tuple<string, Dictionary<string, string>, DataTable>("Life's A Struggle", colums2, ToDataTable(list)),
                        };
                        stream = AutofacConfig.GetContainer().Resolve<IExportManager>().ExportExcelBatchDataTable(tupleList1, true);
                        break;
                    case 3:
                        var tupleList2 = new List<Tuple<string, Dictionary<string, string>, DataTable>>()
                        {
                            new Tuple<string, Dictionary<string, string>, DataTable>("精神可嘉", colums1, ToDataTable(list)),
                            new Tuple<string, Dictionary<string, string>, DataTable>("不错de", colums2, ToDataTable(list)),
                        };
                        stream = AutofacConfig.GetContainer().Resolve<IExportManager>().ExportExcelBatchDataTable(tupleList2, true);
                        break;
                    default:
                        var tupleList3 = new List<Tuple<string, Dictionary<string, string>, DataTable>>()
                        {
                            new Tuple<string, Dictionary<string, string>, DataTable>("团队信息", colums3, ToDataTable(tour)),
                            new Tuple<string, Dictionary<string, string>, DataTable>("景点信息", colums2, ToDataTable(list)),
                        };
                        stream = AutofacConfig.GetContainer().Resolve<IExportManager>().ExportExcelBlockData("颠倒日月", tupleList3, true);
                        break;
                }

                return File(stream, "application/vnd.ms-excel", "景点资源(" + DateTime.Now.ToString("yyyy-MM-dd") + ").xlsx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 导出文档
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult ExportWord(int type)
        {
            try
            {
                var colums1 = new Dictionary<string, string>()
                {
                    { "Name", "麦兜兜"},
                    { "Sex", "孩子不分性别"},
                    { "Nation", "猪猪"},
                    { "Address", "山东省青岛市李沧区"},
                    { "Phone", "不告诉你" }
                };
                var colums2 = new Dictionary<string, string>()
                {
                    { "TourName", "上海一日游"},
                    { "Number", "TMS20170802S001"},
                    { "Destination", "杭州西湖日月潭"},
                };
                var colums3 = new Dictionary<string, string>()
                {
                    { "TourName", "团队名称"},
                    { "SpotName", "景点名称"},
                    { "TicketName", "票种"},
                    { "TourDate", "时间"},
                    { "UnitPrice", "单价"},
                    { "SpotQuantity", "数量"},
                    { "FreeQuantity", "减免"},
                    { "TotalAmount", "总金额"}
                };
                var colums4 = new Dictionary<string, string>()
                {
                    { "TourNo", "团队ID"},
                    { "TourDate", "团队日期"},
                    { "PeopleNumber", "团队人数"}
                };
                var colums5 = new Dictionary<string, string>()
                {
                    { "CompanyName", "江南皮革厂" },
                    { "ContactTel", "1312121111" },
                    { "DepartureDate", "6月10" },
                    { "People", "10人" },
                    { "Car", "5号法拉利" },
                    { "Guid", "大护法" },
                };
                var colums6 = new[]
                {
                    "Day",
                    "Content"
                };
                //是否为表格
                var tupleList1 = new List<Tuple<string, Dictionary<string, string>, DataTable>>()
                {
                    new Tuple<string, Dictionary<string, string>, DataTable>("景点信息", colums3, ToDataTable(GetSpotList())),
                    new Tuple<string, Dictionary<string, string>, DataTable>("游客信息", colums4, ToDataTable(GetTourList())),
                };
                var tupleList2 = new List<Tuple<int, string[], DataTable>>()
                {
                    new Tuple<int, string[], DataTable>(1, colums6, ToDataTable(GetTourPlan())),
                };
                MemoryStream stream;
                switch (type)
                {
                    case 1:
                        stream = _exportManager.ExportWordTemplate(Server.MapPath("~/Temple/个人信息.docx"), colums1);
                        break;
                    case 2:
                        stream = _exportManager.ExportWordCreateTable(Server.MapPath("~/Temple/景点信息.docx"), colums2, tupleList1);
                        break;
                    default:
                        stream = _exportManager.ExportWordExtendTable(Server.MapPath("~/Temple/出游计划.docx"), colums5, tupleList2);
                        break;
                }

                return File(stream, "application/msword", "麦兜兜(" + DateTime.Now.ToString("yyyy-MM-dd") + ").docx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 导入采购任务下单数据
        /// RenYongQiang 2017/02/23
        /// </summary>
        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase myfile)
        {
            var errors = new List<string>();
            try
            {
                if (string.IsNullOrWhiteSpace(myfile?.FileName))
                {
                    errors.Add("失败,未获取到文件！");
                    return Json(new {success = false, data = errors}, JsonRequestBehavior.AllowGet);
                }
                var fileName = myfile.FileName;

                if (!fileName.EndsWith(".xls") && !fileName.EndsWith(".xlsx"))
                {
                    errors.Add("失败,仅能上传.xls或.xlsx文件的格式！");
                    return Json(new {success = false, data = errors}, JsonRequestBehavior.AllowGet);
                }

                if (myfile.ContentLength / (1024 * 1024) > 10) //大于10M
                {
                    errors.Add("失败,文件容量大于10M！");
                    return Json(new {success = false, data = errors}, JsonRequestBehavior.AllowGet);
                }

                var ds = _exportManager.ExcelToDataTableByStream(true, myfile.InputStream, fileName);

                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count == 0)
                {
                    errors.Add("失败,文件内没有内容！");
                    return Json(new {success = false, data = errors}, JsonRequestBehavior.AllowGet);
                }
                var dt = ds.Tables[0];
                //验证列是否存在
                var colums = "队员,号码,位置,年级,绰号";
                errors.AddRange(from msg in colums.Split(',') where dt.Columns[msg] == null select $"该导入的文件中没有\"{msg}\"这一列！");
                if (errors.Count > 0)
                {
                    return Json(new {success = false, data = errors}, JsonRequestBehavior.AllowGet);
                }
                //过滤空白行
                dt = dt.AsEnumerable().Where(x => !string.IsNullOrEmpty(x.Field<string>("队员"))).CopyToDataTable();

                return Json(new {success = true, data = JsonConvert.SerializeObject(dt)}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errors.Add("导入错误，" + ex.Message);
            }

            return Json(new {success = false, data = errors}, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 私有
        private List<TourInfo> GetTourList()
        {
            var list = new List<TourInfo>
            {
                new TourInfo()
                {
                    TourNo = "TMS20170802S001",
                    TourId = "1708051001",
                    TourName = "HDA-U_0805_020301",
                    TourDate = Convert.ToDateTime("2017-10-25"),
                    PeopleNumber = 5
                }
            };

            return list;
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

        private List<TourPlan> GetTourPlan()
        {
            var list = new List<TourPlan>();

            for (var i = 0; i < 3; i++)
            {
                list.Add(new TourPlan()
                {
                    Day = i,
                    Content = "吃饭睡觉打豆豆",
                    Mins = (decimal)0.5 + i,
                    Remark = "无所事事的活着"
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
        #endregion
    }
}