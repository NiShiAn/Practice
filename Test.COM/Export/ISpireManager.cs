using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Test.COM.Export
{
    public interface ISpireManager
    {
        /// <summary>
        /// 导出文档
        /// 仅替换文档变量
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        MemoryStream ExportWord(string path, Dictionary<string, string> dics);
        /// <summary>
        /// 导出文档
        /// 替换内容及新增表格
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        /// <param name="tuples">表格信息：表头, key:字段名-value:列名, 数据源</param>
        MemoryStream ExportWord(string path, Dictionary<string, string> dics,
            List<Tuple<string, Dictionary<string, string>, DataTable>> tuples);
        /// <summary>
        /// 导出文档
        /// 替换内容及扩展表格
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        /// <param name="tuples">表格信息：表格序号(从1开始), 字段名, 数据源</param>
        MemoryStream ExportWord(string path, Dictionary<string, string> dics,
            List<Tuple<int, string[], DataTable>> tuples);

        MemoryStream ExportExcel();
    }
}
