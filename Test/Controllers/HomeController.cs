using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Test.COM.Entity;
using Test.COM.Export;
using Test.COM.Tool;
using Test.DAO.DaoBase;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExportManager _exportManager;
        private readonly ISpireManager _spireManager;
        public HomeController(IExportManager exportManager, ISpireManager spireManager)
        {
            _exportManager = exportManager;
            _spireManager = spireManager;
        }

        #region 功能页面
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Drag()
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

        public ActionResult Flex()
        {
            return View();
        }

        public ActionResult Bind()
        {
            return View();
        }

        public ActionResult FixTab()
        {
            return View();
        }
        #endregion

        #region 页面回调
        /// <summary>
        /// 导出表格
        /// </summary>
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
                        stream = AutofacConfig.GetManager<IExportManager>().ExportExcelForList(
                            new Tuple<string, Dictionary<string, string>, List<SpotInfo>>("麦兜好可爱", colums1, list), true);
                        break;
                    case 2:
                        var tupleList1 = new List<Tuple<string, Dictionary<string, string>, DataTable>>()
                        {
                            new Tuple<string, Dictionary<string, string>, DataTable>("Life's A Struggle", colums2, list.ToDataTable()),
                        };
                        stream = AutofacConfig.GetManager<IExportManager>().ExportExcelBatchDataTable(tupleList1, true);
                        break;
                    case 3:
                        var tupleList2 = new List<Tuple<string, Dictionary<string, string>, DataTable>>()
                        {
                            new Tuple<string, Dictionary<string, string>, DataTable>("精神可嘉", colums1, list.ToDataTable()),
                            new Tuple<string, Dictionary<string, string>, DataTable>("不错de", colums2, list.ToDataTable()),
                        };
                        stream = AutofacConfig.GetManager<IExportManager>().ExportExcelBatchDataTable(tupleList2, true);
                        break;
                    case 4:
                        var tupleList3 = new List<Tuple<string, Dictionary<string, string>, DataTable>>()
                        {
                            new Tuple<string, Dictionary<string, string>, DataTable>("团队信息", colums3, tour.ToDataTable()),
                            new Tuple<string, Dictionary<string, string>, DataTable>("景点信息", colums2, list.ToDataTable()),
                        };
                        stream = AutofacConfig.GetManager<IExportManager>().ExportExcelBlockData("颠倒日月", tupleList3, true);
                        break;
                    default:
                        stream = AutofacConfig.GetManager<ISpireManager>().ExportExcel();
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
                    new Tuple<string, Dictionary<string, string>, DataTable>("景点信息", colums3, GetSpotList().ToDataTable()),
                    new Tuple<string, Dictionary<string, string>, DataTable>("游客信息", colums4, GetTourList().ToDataTable()),
                };
                var tupleList2 = new List<Tuple<int, string[], DataTable>>()
                {
                    new Tuple<int, string[], DataTable>(1, colums6, GetTourPlan().ToDataTable()),
                };
                MemoryStream stream;
                switch (type)
                {
                    case 1:
                        stream = _exportManager.ExportWordTemplate(Server.MapPath("~/Temple/个人信息.docx"), colums1);
                        //stream = _spireManager.ExportWord(Server.MapPath("~/Temple/个人信息.docx"), colums1);
                        break;
                    case 2:
                        stream = _exportManager.ExportWordCreateTable(Server.MapPath("~/Temple/景点信息.docx"), colums2, tupleList1);
                        //stream = _spireManager.ExportWord(Server.MapPath("~/Temple/景点信息.docx"), colums2, tupleList1);
                        break;
                    case 3:
                        stream = _exportManager.ExportWordExtendTable(Server.MapPath("~/Temple/出游计划.docx"), colums5, tupleList2);
                        //stream = _spireManager.ExportWord(Server.MapPath("~/Temple/出游计划.docx"), colums5, tupleList2);
                        break;
                    default:
                        stream = _spireManager.ExportWord(Server.MapPath("~/Temple/景点信息.docx"), colums2, tupleList1);
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
        /// 导入Excel数据
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
                    return Json(new { success = false, data = errors }, JsonRequestBehavior.AllowGet);
                }
                var dt = ds.Tables[0];
                ImportXenoblade2(dt);
                //var dt = _exportManager.ExcelToDataTable("", true, null, myfile.InputStream);
                //if (dt == null || dt.Rows.Count == 0)
                //{
                //    errors.Add("失败,文件内没有内容！");
                //    return Json(new { success = false, data = errors }, JsonRequestBehavior.AllowGet);
                //}

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

        /// <summary>
        /// 文档下载
        /// path：文件路径
        /// name：文件名称
        /// isLocal：是否本地文件
        /// </summary>
        public void DownLoad(string path, string name, bool isLocal = false)
        {
            var filePath = path;
            if (filePath.IsNoValue() || name.IsNoValue()) return;

            byte[] buffer;
            if (isLocal)
            {
                filePath = Server.MapPath(filePath);
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    buffer = new byte[(int) fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                }
            }
            else
            {
                var webResponse = (HttpWebResponse) ((HttpWebRequest) WebRequest.Create(path)).GetResponse();
                using (var fs = webResponse.GetResponseStream())
                {
                    buffer = new byte[webResponse.ContentLength];
                    int offset = 0, actuallyRead;
                    do
                    {
                        actuallyRead = fs?.Read(buffer, offset, buffer.Length - offset) ?? 0;
                        offset += actuallyRead;
                    }
                    while (actuallyRead > 0);
                    webResponse.Close();
                }
            }
            Response.Clear();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");

            Response.AddHeader("Content-Disposition", "attachment; filename=" + name);
            Response.BinaryWrite(buffer);
            Response.End();
        }

        public ActionResult GetDropDowm()
        {
            var list = new List<BaseItem>()
            {
                new BaseItem(){ Id = "2", Text = "第二天"},
                new BaseItem(){ Id = "3", Text = "第三天"},
                new BaseItem(){ Id = "4", Text = "第四天"},
                new BaseItem(){ Id = "5", Text = "第五天"},
            };
            return Json(list, JsonRequestBehavior.AllowGet);
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
                    TicketName = "成\n人\n票",
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

        /// <summary>
        /// 导入异度之刃2Excel
        /// </summary>
        /// <param name="dt"></param>
        private void ImportXenoblade2(DataTable dt)
        {
            if(dt.Rows.Count < 1)
                return;

            var bmanager = new BaseManager<Xblade2>();
            var bsmanager = new BaseManager<Xblade2_Sub>();
            foreach (DataRow dr in dt.Rows)
            {
                var info = new Xblade2()
                {
                    Name = dr[0].ToString(),
                    Role = dr[1].ToString(),
                    Partner = dr[2].ToString(),
                    Fetter = dr[3].ToString(),
                    Target = dr[4].ToString(),
                    IsActive = true,
                    CreateBy = "erose",
                    CreateDate = DateTime.Now
                };

                var idx = bmanager.Insert(info);

                var list = new List<Xblade2_Sub>();
                if (!string.IsNullOrEmpty(dr[5].ToString()))
                {
                    list.AddRange(dr[5].ToString().Split('\n').Select(n => new Xblade2_Sub()
                    {
                        Xkey = idx,
                        Equib = n,
                        Genrn = "饰品",
                        IsReach = false,
                        IsActive = true,
                        CreateBy = "erose",
                        CreateDate = DateTime.Now

                    }));
                }
                if (!string.IsNullOrEmpty(dr[6].ToString()))
                {
                    list.Add(new Xblade2_Sub()
                    {
                        Xkey = idx,
                        Equib = dr[6].ToString(),
                        Genrn = "晶片",
                        IsReach = false,
                        IsActive = true,
                        CreateBy = "erose",
                        CreateDate = DateTime.Now

                    });
                }
                if (!string.IsNullOrEmpty(dr[7].ToString()))
                {
                    list.AddRange(dr[7].ToString().Split('\n').Select(n => new Xblade2_Sub()
                    {
                        Xkey = idx,
                        Equib = n,
                        Genrn = "辅助核心",
                        IsReach = false,
                        IsActive = true,
                        CreateBy = "erose",
                        CreateDate = DateTime.Now

                    }));
                }
                if (!string.IsNullOrEmpty(dr[8].ToString()))
                {
                    list.AddRange(dr[8].ToString().Split('\n').Select(n => new Xblade2_Sub()
                    {
                        Xkey = idx,
                        Equib = n,
                        Genrn = "喜好物品",
                        IsReach = false,
                        IsActive = true,
                        CreateBy = "erose",
                        CreateDate = DateTime.Now
                    }));
                }
                foreach (var item in list)
                {
                    bsmanager.Insert(item);
                }
            }
        }
        #endregion
    }
}