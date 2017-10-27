using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.HSSF.Model;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Test.COM.Entity;

namespace Test.COM
{
    public class ExportManager : IExportManager
    {
        private class NpoiMemoryStream : MemoryStream
        {
            public NpoiMemoryStream()
            {
                AllowClose = true;
            }

            public bool AllowClose { get; set; }

            public override void Close()
            {
                if (AllowClose)
                    base.Close();
            }
        }
        /// <summary>
        /// List导出Excel
        /// </summary>
        /// <param name="tuple">Sheet名, key:列名-Value:字段名, 数据源</param>
        /// <param name="isWidth">列宽自适应(谨慎,字数少的可用)</param>
        /// <returns></returns>
        public MemoryStream ExportExcelForList<T>(Tuple<string, Dictionary<string, string>, List<T>> tuple, bool isWidth = false)
        {
            var workbook = new XSSFWorkbook();
            //添加Sheet
            workbook = AddSheetForList(workbook, tuple, isWidth);

            var ms = new NpoiMemoryStream { AllowClose = false };
            workbook.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;
        }
        /// <summary>
        /// DataTable批量导出Excel
        /// 多个Sheet
        /// </summary>
        /// <param name="tupleList">Sheet名, key:列名-Value:字段名, 数据源</param>
        /// <param name="isWidth">列宽自适应(谨慎,字数少的可用)</param>
        /// <returns></returns>
        public MemoryStream ExportExcelBatchDataTable(List<Tuple<string, Dictionary<string, string>, DataTable>> tupleList, bool isWidth = false)
        {
            var workbook = new XSSFWorkbook();
            //添加Sheet
            workbook = tupleList.Aggregate(workbook, (current, t) => AddSheetForDataTable(current, t, isWidth));

            var ms = new NpoiMemoryStream { AllowClose = false };
            workbook.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;
        }
        /// <summary>
        /// WorkBook添加Sheet
        /// 数据源List
        /// </summary>
        protected virtual XSSFWorkbook AddSheetForList<T>(XSSFWorkbook workbook, Tuple<string, Dictionary<string, string>, List<T>> tuple, bool isWidth)
        {
            
            //创建工作簿
            var sheet = workbook.CreateSheet(tuple.Item1);
            //创建样式
            var style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;// 左右居中   
            style.VerticalAlignment = VerticalAlignment.Center;// 上下居中   
            style.WrapText = false;

            //创建标题行
            var titleRow = sheet.CreateRow(0);
            for (var i = 0; i < tuple.Item2.Count; i++)
            {
                var text = tuple.Item2.ElementAt(i).Key;
                if (string.IsNullOrEmpty(text))
                    continue;
                var cell = titleRow.CreateCell(i, CellType.String);
                cell.SetCellValue(text);
                cell.CellStyle = style;
            }
            sheet.CreateFreezePane(0, 1, 0, 1);
            //写入数据
            var rowcount = 1;

            foreach (var info in tuple.Item3)
            {
                var row = sheet.CreateRow(rowcount);

                var properties = info.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                var col = 0;
                foreach (var column in tuple.Item2)
                {
                    var property = properties.FirstOrDefault(n => n.Name == column.Value);

                    if (property != null)
                    {
                        var value = property.GetValue(info, null);

                        if (property.PropertyType.Name.StartsWith("Int"))
                        {
                            row.CreateCell(col).SetCellValue(Convert.ToInt32(value));
                        }
                        else if (property.PropertyType.Name.StartsWith("Decimal") || property.PropertyType.Name.StartsWith("Double") || property.PropertyType.Name.StartsWith("Single"))
                        {
                            row.CreateCell(col).SetCellValue(Convert.ToDouble(value));
                        }
                        else if (property.PropertyType.Name.StartsWith("DateTime"))
                        {
                            row.CreateCell(col).SetCellValue(Convert.ToDateTime(value).ToString("yyyy-MM-dd"));
                        }
                        else if (property.PropertyType.Name.StartsWith("Boolean"))
                        {
                            row.CreateCell(col).SetCellValue(Convert.ToBoolean(value) ? "是" : "否");
                        }
                        else
                        {
                            row.CreateCell(col).SetCellValue(value.ToString());
                        }
                    }
                    else
                    {
                        row.CreateCell(col).SetCellValue("");
                    }

                    row.GetCell(col).CellStyle = style;
                    col++;
                }

                rowcount++;
            }
            //列宽自适应
            if (isWidth)
            {
                //列宽自适应，只对英文和数字有效  
                for (var i = 0; i <= tuple.Item2.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                //获取当前列的宽度，然后对比本列的长度，取最大值  
                for (var columnNum = 0; columnNum <= tuple.Item2.Count; columnNum++)
                {
                    if (columnNum == 0)
                    {
                        sheet.SetColumnWidth(columnNum, 9 * 256);
                        continue;
                    }
                    var columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                    for (var rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                    {
                        //当前行未被使用过  
                        var currentRow = sheet.GetRow(rowNum) ?? sheet.CreateRow(rowNum);
                        if (currentRow.GetCell(columnNum) == null)
                            continue;
                        var currentCell = currentRow.GetCell(columnNum);
                        var length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                    sheet.SetColumnWidth(columnNum, columnWidth * 256);
                }
            }

            return workbook;
        }
        /// <summary>
        /// WorkBook添加Sheet
        /// 数据源DataTable
        /// </summary>
        protected virtual XSSFWorkbook AddSheetForDataTable(XSSFWorkbook workbook, Tuple<string, Dictionary<string, string>, DataTable> tuple, bool isWidth)
        {
            //创建工作簿
            var sheet = workbook.CreateSheet(tuple.Item1);
            //创建样式
            var style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;// 左右居中   
            style.VerticalAlignment = VerticalAlignment.Center;// 上下居中   
            style.WrapText = false;
            //创建标题行
            var titleRow = sheet.CreateRow(0);
            for (var i = 0; i < tuple.Item2.Count; i++)
            {
                var text = tuple.Item2.ElementAt(i).Key;
                if (string.IsNullOrEmpty(text))
                    continue;
                var cell = titleRow.CreateCell(i, CellType.String);
                cell.SetCellValue(text);
                cell.CellStyle = style;
            }
            sheet.CreateFreezePane(0, 1, 0, 1);
            //写入数据
            var rowcount = 1;

            foreach (DataRow dr in tuple.Item3.Rows)
            {
                var row = sheet.CreateRow(rowcount);
                var col = 0;
                foreach (var column in tuple.Item2)
                {
                    var value = dr.Table.Columns[column.Value] == null ? "" : dr[column.Value].ToString();

                    row.CreateCell(col).SetCellValue(value);
                    row.GetCell(col).CellStyle = style;
                    col++;
                }

                rowcount++;
            }
            //列宽自适应
            if (isWidth)
            {
                //列宽自适应，只对英文和数字有效  
                for (var i = 0; i <= tuple.Item2.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                //获取当前列的宽度，然后对比本列的长度，取最大值  
                for (var columnNum = 0; columnNum <= tuple.Item2.Count; columnNum++)
                {
                    if (columnNum == 0)
                    {
                        sheet.SetColumnWidth(columnNum, 9 * 256);
                        continue;
                    }
                    var columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                    for (var rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                    {
                        //当前行未被使用过  
                        var currentRow = sheet.GetRow(rowNum) ?? sheet.CreateRow(rowNum);
                        if (currentRow.GetCell(columnNum) == null)
                            continue;
                        var currentCell = currentRow.GetCell(columnNum);
                        var length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                    sheet.SetColumnWidth(columnNum, columnWidth * 256);
                }
            }

            return workbook;
        }
    }
}
