using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using Test.COM.Entity;
using Test.DAO.DaoBase;

namespace Test.Controllers
{
    public class BuggerController : Controller
    {
        private static readonly HttpClient Http = new HttpClient();

        #region 页面

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region 方法

        public ActionResult RequestPage(string url)
        {       
            // 设置配置以支持文档加载
            var config = Configuration.Default.WithDefaultLoader();
            // 请求网址
            var document = BrowsingContext.New(config).OpenAsync(url);
            var content = document.Result.DocumentElement.TextContent;

            //死亡细胞数据
            DebuggerDeadCells(document);

            // 根据class获取html元素
            // var node = document.Result.QuerySelectorAll("");
            // We are only interested in the text - select it with LINQ
            var list = new List<BaseItem>();
            //if (cells != null && cells.Any())
            //{
            //    list = cells.Select(n => new BaseItem()
            //    {
            //        Key = n.Text().Split('：')[1],
            //        Value = n.GetAttribute("href")
            //    }).ToList();
            //}

            return View("List", list);
        }

        #endregion

        #region 死亡细胞
        /// <summary>
        /// https://cowlevel.net/article/1884891
        /// </summary>
        private void DebuggerDeadCells(Task<IDocument> document)
        {
            var rootNode = document.Result.QuerySelectorAll("article.tab-discuss-all");
            var childNodes = rootNode[0]?.Children?.ToList();

            if(childNodes == null || !childNodes.Any()) return;

            #region 怪物信息,11-182
            var mList = new List<DeadCells_Monster>();
            for (var i = 11; i < 183; i++)
            {
                var cur = childNodes[i].Children.FirstOrDefault();
                if (cur != null && cur.TagName == "IMG")
                {
                    var mons = new DeadCells_Monster()
                    {
                        Name = "",
                        Image = cur.GetAttribute("v-lazy"),
                        IsActive = true,
                        CreateBy = "erose",
                        CreateDate = DateTime.Now
                    };
                    var j = i;
                    do
                    {
                        var kats = childNodes[++j];
                        if (kats != null)
                        {
                            if (kats.Children.Any() && kats.Children.First().TagName == "STRONG")
                            {
                                mons.Name = kats.Children.First().TextContent.Trim();
                            }
                            else if (kats.TextContent.Contains("技能："))
                            {
                                mons.Skill = kats.TextContent.Replace("技能：", "").Trim();
                                if (!childNodes[j + 1].TextContent.Contains("地图："))
                                {
                                    var n = j;
                                    while (!childNodes[n + 1].TextContent.Contains("地图："))
                                    {
                                        mons.Skill += $"@{childNodes[++n].TextContent.Trim()}";
                                    }
                                    j = n;
                                }
                            }
                            else if (kats.TextContent.Contains("地图："))
                            {
                                mons.Map = kats.TextContent.Replace("地图：", "").Trim();
                                if (!childNodes[j + 1].TextContent.Contains("注意："))
                                {
                                    var n = j;
                                    while (!childNodes[n + 1].TextContent.Contains("注意："))
                                    {
                                        mons.Map += $"@{childNodes[++n].TextContent.Trim()}";
                                    }
                                    j = n;
                                }
                            }
                            else if (kats.TextContent.Contains("注意："))
                            {
                                mons.Note = kats.TextContent.Replace("注意：", "").Trim();
                                if (!childNodes[j + 1].Children.Any() && childNodes[j + 1].TagName == "P")
                                {
                                    var n = j;
                                    while (!childNodes[n + 1].Children.Any() && childNodes[j + 1].TagName == "P")
                                    {
                                        mons.Note += $"@{childNodes[++n].TextContent.Trim()}";
                                    }
                                    j = n;
                                }
                            }
                        }
                    } while (childNodes[j + 1]?.Children?.FirstOrDefault()?.TagName != "IMG" && j < 183);
                    mList.Add(mons);
                    i = j;
                }
            }
            #endregion

            #region 武器信息,476-808
            var wList = new List<DeadCells_Weapon>();
            var wDic = new Dictionary<int, int[]>()
            {
                {1, new[] {476, 596}},
                {2, new[] {600, 656}},
                {3, new[] {659, 677}},
                {4, new[] {682, 733}},
                {5, new[] {734, 771}},
                {6, new[] {772, 808}}
            };
            foreach (var info in wDic)
            {
                for (var i = info.Value[0]; i < info.Value[1]; i++)
                {
                    if (childNodes[i].GetElementsByTagName("IMG").Length < 1) continue;
                    var weap = new DeadCells_Weapon()
                    {
                        TypeId = info.Key,
                        Name = childNodes[i].Children[0].TextContent.Trim(),
                        Image = childNodes[i].GetElementsByTagName("IMG")[0].GetAttribute("v-lazy"),
                        IsActive = true,
                        CreateBy = "erose",
                        CreateDate = DateTime.Now
                    };

                    var j = i;
                    do
                    {
                        var kats = childNodes[++j];
                        if (kats != null)
                        {
                            if (kats.TextContent.Contains("武器效果："))
                            {
                                weap.Effect = kats.TextContent.Replace("武器效果：", "").Trim();
                                if (!childNodes[j + 1].TextContent.Contains("图纸获取："))
                                {
                                    var n = j;
                                    while (!childNodes[n + 1].TextContent.Contains("图纸获取："))
                                    {
                                        weap.Effect += $"@{childNodes[++n].TextContent.Trim()}";
                                    }
                                    j = n;
                                }
                            }
                            else if (kats.TextContent.Contains("图纸获取："))
                            {
                                weap.DesignPaper = kats.TextContent.Replace("图纸获取：", "").Trim();
                                if (!childNodes[j + 1].TextContent.Contains("清图适性：") && info.Key != 3)
                                {
                                    var n = j;
                                    while (!childNodes[n + 1].TextContent.Contains("清图适性："))
                                    {
                                        weap.DesignPaper += $"@{childNodes[++n].TextContent.Trim()}";
                                    }
                                    j = n;
                                }
                            }
                            else if (kats.TextContent.Contains("清图适性："))
                            {
                                weap.KillAbility = kats.TextContent.Replace("清图适性：", "").Trim();
                                if (!childNodes[j + 1].TextContent.Contains("对BOSS适性："))
                                {
                                    var n = j;
                                    while (!childNodes[n + 1].TextContent.Contains("对BOSS适性："))
                                    {
                                        weap.KillAbility += $"@{childNodes[++n].TextContent.Trim()}";
                                    }
                                    j = n;
                                }
                            }
                            else if (kats.TextContent.Contains("对BOSS适性："))
                            {
                                weap.BossAbility = kats.TextContent.Replace("对BOSS适性：", "").Trim();
                                if (!childNodes[j + 1].TextContent.Contains("评价："))
                                {
                                    var n = j;
                                    while (!childNodes[n + 1].TextContent.Contains("评价："))
                                    {
                                        weap.BossAbility += $"@{childNodes[++n].TextContent.Trim()}";
                                    }
                                    j = n;
                                }
                            }
                            else if (kats.TextContent.Contains("评价："))
                            {
                                weap.Evaluate = kats.TextContent.Replace("评价：", "").Trim();
                                if (!childNodes[j + 1].TextContent.Contains("最适词缀："))
                                {
                                    var n = j;
                                    while (!childNodes[n + 1].TextContent.Contains("最适词缀："))
                                    {
                                        weap.Evaluate += $"@{childNodes[++n].TextContent.Trim()}";
                                    }
                                    j = n;
                                }
                            }
                            else if (kats.TextContent.Contains("最适词缀："))
                            {
                                weap.BestAttribute = kats.TextContent.Replace("最适词缀：", "").Trim();
                                if (!childNodes[j + 1].Children.Any() && childNodes[j + 1].TagName == "P")
                                {
                                    var n = j;
                                    while (!childNodes[n + 1].Children.Any() && childNodes[n + 1].TagName == "P")
                                    {
                                        weap.BestAttribute += $"@{childNodes[++n].TextContent.Trim()}";
                                    }
                                    j = n;
                                }
                            }
                        }
                    } while (childNodes[j + 1].TagName != "UL" && !childNodes[j + 1].Children.Any() && j < info.Value[1]);
                    wList.Add(weap);
                    i = j;
                }
            }
            #endregion

            #region 保存数据
            //if (mList.Any())
            //    new BaseManager<DeadCells_Monster>().InsertList(mList);
            if (wList.Any())
                new BaseManager<DeadCells_Weapon>().InsertList(wList);
            #endregion
        }

        #endregion
    }
}