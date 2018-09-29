using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using AngleSharp.Parser.Html;
using AngleSharp;
using AngleSharp.Extensions;
using Test.COM.Entity;

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
            // 根据class获取html元素
            var cells = document.Result.QuerySelectorAll("#pe100_page_contentpage ul li a");
            // We are only interested in the text - select it with LINQ
            var list = new List<BaseItem>();
            if (cells != null && cells.Any())
            {
                list = cells.Select(n => new BaseItem()
                {
                    Key = n.Text().Split('：')[1],
                    Value = n.GetAttribute("href")
                }).ToList();
            }

            return View("List", list);
        }

        #endregion
    }
}