using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.COM
{
    public interface IExportManager
    {
        #region Excel
        /// <summary>
        /// List导出Excel
        /// </summary>
        /// <param name="tuple">Sheet名, key:列名-Value:字段名, 数据源</param>
        /// <param name="isWidth">列宽自适应(谨慎,字数少的可用)</param>
        /// <returns></returns>
        MemoryStream ExportExcelForList<T>(Tuple<string, Dictionary<string, string>, List<T>> tuple, bool isWidth = false);

        /// <summary>
        /// DataTable批量导出Excel
        /// 多个Sheet
        /// </summary>
        /// <param name="tupleList">Sheet名, key:列名-Value:字段名, 数据源</param>
        /// <param name="isWidth">列宽自适应(谨慎,字数少的可用)</param>
        /// <returns></returns>
        MemoryStream ExportExcelBatchDataTable(List<Tuple<string, Dictionary<string, string>, DataTable>> tupleList, bool isWidth = false);

        /// <summary>
        /// 块状数据批量导出Excel
        /// </summary>
        /// <param name="sheetName">Sheet名</param>
        /// <param name="tupleList">标题名, key:列名-Value:字段名, 数据源</param>
        /// <param name="isWidth">列宽自适应(谨慎,字数少的可用)</param>
        /// <returns></returns>
        MemoryStream ExportExcelBlockData(string sheetName, List<Tuple<string, Dictionary<string, string>, DataTable>> tupleList, bool isWidth = false);
        #endregion

        #region Word(需要NPOI版本2.3)
        /// <summary>
        /// 导出Word模板
        /// 仅替换文档变量
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        /// <returns></returns>
        MemoryStream ExportWordTemplate(string path, Dictionary<string, string> dics);
        /// <summary>
        /// 导出Word模板
        /// 替换文档变量和添加表格
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        /// <param name="tuples">表格信息：表头, key:字段名-value:列名, 数据源</param>
        /// <returns></returns>
        MemoryStream ExportWordCreateTable(string path, Dictionary<string, string> dics, List<Tuple<string, Dictionary<string, string>, DataTable>> tuples);
        /// <summary>
        /// 导出Word模板
        /// 替换文档变量和填充表格
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        /// <param name="tuples">表格信息：表格序号(从1开始), 字段名, 数据源</param>
        /// <returns></returns>
        MemoryStream ExportWordExtendTable(string path, Dictionary<string, string> dics, List<Tuple<int, string[], DataTable>> tuples);

        #endregion
    }
}