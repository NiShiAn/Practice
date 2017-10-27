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
    }
}
