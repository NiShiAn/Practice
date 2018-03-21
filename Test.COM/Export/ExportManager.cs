using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;

namespace Test.COM.Export
{
    public class ExportManager : IExportManager
    {
        #region 导出Excel
        private class NpoiMemoryStream : MemoryStream
        {
            public NpoiMemoryStream()
            {
                AllowClose = true;
            }

            public bool AllowClose { private get; set; }

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
            AddSheetForList(workbook, tuple, isWidth);

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
            foreach (var tuple in tupleList)
            {
                AddSheetForDataTable(workbook, tuple, isWidth);
            }

            var ms = new NpoiMemoryStream { AllowClose = false };
            workbook.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;
        }
        /// <summary>
        /// 块状数据批量导出Excel
        /// </summary>
        /// <param name="sheetName">Sheet名</param>
        /// <param name="tupleList">标题名, key:列名-Value:字段名, 数据源</param>
        /// <param name="isWidth">列宽自适应(谨慎,字数少的可用)</param>
        /// <returns></returns>
        public MemoryStream ExportExcelBlockData(string sheetName, List<Tuple<string, Dictionary<string, string>, DataTable>> tupleList, bool isWidth = false)
        {
            var workbook = new XSSFWorkbook();
            //创建工作簿
            var sheet = workbook.CreateSheet(sheetName);

            //创建样式
            var tStyle = workbook.CreateCellStyle();//标题样式
            var font = workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;//加粗
            tStyle.SetFont(font);
            tStyle.Alignment = HorizontalAlignment.Center;// 左右居中   
            tStyle.VerticalAlignment = VerticalAlignment.Center;// 上下居中   
            tStyle.WrapText = false;//是否换行
            tStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;//灰色
            tStyle.FillPattern = FillPattern.SolidForeground;
            var cStyle = workbook.CreateCellStyle();//内容样式
            cStyle.Alignment = HorizontalAlignment.Center;
            cStyle.VerticalAlignment = VerticalAlignment.Center;
            cStyle.WrapText = true;
            //填充数据
            var maxColumn = tupleList.Select(x => x.Item2.Count).Max() - 1;
            var rowIndex = 0;//行号
            foreach (var tuple in tupleList)
            {
                //标题
                var rowHead = sheet.CreateRow(rowIndex);
                var cellHead = rowHead.CreateCell(0, CellType.String);
                cellHead.SetCellValue(tuple.Item1);
                cellHead.CellStyle = tStyle;
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, maxColumn));
                rowIndex++;
                //列名
                var rowTitle = sheet.CreateRow(rowIndex);
                for (var i = 0; i < tuple.Item2.Count; i++)
                {
                    var text = tuple.Item2.ElementAt(i).Key;
                    if (string.IsNullOrEmpty(text))
                        continue;
                    var cellTitle = rowTitle.CreateCell(i, CellType.String);
                    cellTitle.SetCellValue(text);
                    cellTitle.CellStyle = cStyle;
                }
                sheet.CreateFreezePane(0, 1, 0, 1);
                rowIndex++;
                //数据
                foreach (DataRow dr in tuple.Item3.Rows)
                {
                    var row = sheet.CreateRow(rowIndex);
                    var height = 1;
                    var col = 0;
                    foreach (var column in tuple.Item2)
                    {
                        var value = dr.Table.Columns[column.Value] == null ? "" : dr[column.Value].ToString();
                        height = Math.Max(value.Count(ch => ch == '\n') + 1, height);

                        row.CreateCell(col).SetCellValue(value);
                        row.GetCell(col).CellStyle = cStyle;
                        col++;
                    }
                    row.Height = (short)(height * 15 * 20);
                    rowIndex++;
                }
            }
            //列宽自适应
            if (isWidth)
            {
                //列宽自适应，只对英文和数字有效  
                for (var i = 0; i <= maxColumn; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                //获取当前列的宽度，然后对比本列的长度，取最大值  
                for (var columnNum = 0; columnNum <= maxColumn; columnNum++)
                {
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
        protected virtual void AddSheetForList<T>(XSSFWorkbook workbook, Tuple<string, Dictionary<string, string>, List<T>> tuple, bool isWidth)
        {
            
            //创建工作簿
            var sheet = workbook.CreateSheet(tuple.Item1);
            
            //创建样式
            var tStyle = workbook.CreateCellStyle();//标题样式
            var font = workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;//加粗
            tStyle.SetFont(font);
            tStyle.Alignment = HorizontalAlignment.Center;// 左右居中   
            tStyle.VerticalAlignment = VerticalAlignment.Center;// 上下居中   
            tStyle.WrapText = false;//是否换行
            tStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;//灰色
            tStyle.FillPattern = FillPattern.SolidForeground;
            //自定义颜色
            //((XSSFColor)tStyle.FillForegroundColorColor).SetRgb(new byte[] { 226, 239, 218 });
            var cStyle = workbook.CreateCellStyle();//内容样式
            cStyle.Alignment = HorizontalAlignment.Center;
            cStyle.VerticalAlignment = VerticalAlignment.Center;
            cStyle.WrapText = true;
            //创建标题行
            var titleRow = sheet.CreateRow(0);
            for (var i = 0; i < tuple.Item2.Count; i++)
            {
                var text = tuple.Item2.ElementAt(i).Key;
                if (string.IsNullOrEmpty(text))
                    continue;
                var cell = titleRow.CreateCell(i, CellType.String);
                cell.SetCellValue(text);
                cell.CellStyle = tStyle;
            }
            sheet.CreateFreezePane(0, 1, 0, 1);
            //写入数据
            var rowcount = 1;

            foreach (var info in tuple.Item3)
            {
                var row = sheet.CreateRow(rowcount);
                var height = 1;
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
                            var content = value?.ToString() ?? "";
                            row.CreateCell(col).SetCellValue(content);
                            height = Math.Max(content.Count(ch => ch == '\n') + 1, height);
                        }
                    }
                    else
                    {
                        row.CreateCell(col).SetCellValue("");
                    }

                    row.GetCell(col).CellStyle = cStyle;
                    col++;
                }
                row.Height = (short) (height*15*20);
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
        }
        /// <summary>
        /// WorkBook添加Sheet
        /// 数据源DataTable
        /// </summary>
        protected virtual void AddSheetForDataTable(XSSFWorkbook workbook, Tuple<string, Dictionary<string, string>, DataTable> tuple, bool isWidth)
        {
            //创建工作簿
            var sheet = workbook.CreateSheet(tuple.Item1);
            //创建样式
            var tStyle = workbook.CreateCellStyle();//标题样式
            var font = workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;//加粗
            tStyle.SetFont(font);
            tStyle.Alignment = HorizontalAlignment.Center;// 左右居中   
            tStyle.VerticalAlignment = VerticalAlignment.Center;// 上下居中   
            tStyle.WrapText = false;//是否换行
            tStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;//灰色
            tStyle.FillPattern = FillPattern.SolidForeground;
            var cStyle = workbook.CreateCellStyle();//内容样式
            cStyle.Alignment = HorizontalAlignment.Center;
            cStyle.VerticalAlignment = VerticalAlignment.Center;
            cStyle.WrapText = true;
            //创建标题行
            var titleRow = sheet.CreateRow(0);
            for (var i = 0; i < tuple.Item2.Count; i++)
            {
                var text = tuple.Item2.ElementAt(i).Key;
                if (string.IsNullOrEmpty(text))
                    continue;
                var cell = titleRow.CreateCell(i, CellType.String);
                cell.SetCellValue(text);
                cell.CellStyle = tStyle;
            }
            sheet.CreateFreezePane(0, 1, 0, 1);
            //写入数据
            var rowcount = 1;

            foreach (DataRow dr in tuple.Item3.Rows)
            {
                var row = sheet.CreateRow(rowcount);
                var height = 1;
                var col = 0;
                foreach (var column in tuple.Item2)
                {
                    var value = dr.Table.Columns[column.Value] == null ? "" : dr[column.Value].ToString();
                    height = Math.Max(value.Count(ch => ch == '\n') + 1, height);

                    row.CreateCell(col).SetCellValue(value);
                    row.GetCell(col).CellStyle = cStyle;
                    col++;
                }
                row.Height = (short)(height * 15 * 20);
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
        }
        #endregion

        #region 导出Word(需要NPOI版本2.3)
        /// <summary>
        /// 导出Word模板
        /// 仅替换文档变量
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        /// <returns></returns>
        public MemoryStream ExportWordTemplate(string path, Dictionary<string, string> dics)
        {
            var document = new XWPFDocument(File.OpenRead(path));

            //变量替换
            DocReplaceVariable(document, dics);

            var ms = new NpoiMemoryStream { AllowClose = false };
            document.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;

        }
        /// <summary>
        /// 导出Word模板
        /// 替换文档变量和添加表格
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        /// <param name="tuples">表格信息：表头, key:字段名-value:列名, 数据源</param>
        /// <returns></returns>
        public MemoryStream ExportWordCreateTable(string path, Dictionary<string, string> dics, List<Tuple<string, Dictionary<string, string>, DataTable>> tuples)
        {
            var document = new XWPFDocument(File.OpenRead(path));

            //变量替换
            if (dics != null && dics.Any())
            {
                DocReplaceVariable(document, dics);
            }
            //添加表格
            if (tuples != null && tuples.Any())
            {
                foreach (var tuple in tuples)
                {
                    DocAddTable(document, tuple.Item1, tuple.Item2, tuple.Item3);
                }
            }

            var ms = new NpoiMemoryStream { AllowClose = false };
            document.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;
        }
        /// <summary>
        /// 导出Word模板
        /// 替换文档变量和填充表格
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        /// <param name="tuples">表格信息：表格序号(从1开始), 字段名, 数据源</param>
        /// <returns></returns>
        public MemoryStream ExportWordExtendTable(string path, Dictionary<string, string> dics, List<Tuple<int, string[], DataTable>> tuples)
        {
            var document = new XWPFDocument(File.OpenRead(path));

            //变量替换
            if (dics != null && dics.Any())
            {
                DocReplaceVariable(document, dics);
            }
            //扩展表格
            if (tuples != null && tuples.Any())
            {
                var tabCount = document.Tables.Count;
                foreach (var tuple in tuples)
                {
                    if (tuple.Item1 <= tabCount)
                    {
                        var table = document.Tables[tuple.Item1 - 1];
                        DocTableAddTr(table, tuple.Item2, tuple.Item3);
                    }
                }
            }

            var ms = new NpoiMemoryStream { AllowClose = false };
            document.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;
        }
        /// <summary>
        /// 文档替换变量
        /// </summary>
        /// <param name="doc">文档</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        protected virtual void DocReplaceVariable(XWPFDocument doc, Dictionary<string, string> dics)
        {
            //段落变量替换
            foreach (var paragraph in doc.Paragraphs)
            {
                foreach (var item in dics)
                {
                    if (!paragraph.Text.Contains("${" + item.Key + "}"))
                        continue;

                    paragraph.ReplaceText("${" + item.Key + "}", item.Value);
                }
            }
            //表格变量替换
            foreach (var table in doc.Tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        foreach (var paragraph in cell.Paragraphs)
                        {
                            foreach (var item in dics)
                            {
                                if (!paragraph.Text.Contains("${" + item.Key + "}"))
                                    continue;

                                paragraph.ReplaceText("${" + item.Key + "}", item.Value);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 文档添加Table
        /// </summary>
        /// <param name="doc">文档</param>
        /// <param name="title">表头</param>
        /// <param name="dics">标列信息：key:字段名-value:列名</param>
        /// <param name="dt">数据源</param>
        protected virtual void DocAddTable(XWPFDocument doc, string title, Dictionary<string, string> dics, DataTable dt)
        {
            var rowCount = dt.Rows.Count;
            var colCount = dics.Count;
            //创建表格
            var table = doc.CreateTable(1, 1);
            table.Width = 4900;
            //表头
            var tCell = table.GetRow(0).GetCell(0);
            var tCtt = tCell.GetCTTc();
            CT_TcPr ctPr = tCtt.AddNewTcPr();
            ctPr.gridSpan = new CT_DecimalNumber { val = colCount.ToString() };//合并列
            tCtt.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;
            var tCpr = tCell.Paragraphs[0].CreateRun();
            tCpr.IsBold = true;
            tCpr.SetText(title);
            //填充内容
            for (int n = 0; n < rowCount + 1; n++)
            {
                var tr = table.CreateRow();//创建行时默认创建一个单元格
                //标题
                for (int g = 0; g < colCount; g++)
                {
                    var dic = dics.ElementAt(g);
                    var text = n == 0 ? dic.Value : dic.Key == "Null" ? "" : dt.Rows[n - 1][dic.Key].ToString();//Null,不展示数据
                    var td = g == 0 ? tr.GetCell(0) : tr.CreateCell();

                    td.SetText(text);
                }
            }
            doc.CreateParagraph();//回车
        }
        /// <summary>
        /// Table添加Row
        /// </summary>
        /// <param name="table">表格</param>
        /// <param name="columns">字段名</param>
        /// <param name="dt">数据源</param>
        protected virtual void DocTableAddTr(XWPFTable table, string[] columns, DataTable dt)
        {
            var lctr = table.Rows[table.Rows.Count - 1].GetCTRow();
            foreach (DataRow dr in dt.Rows)
            {
                var nctr = new CT_Row();
                for (int i = 0; i < columns.Length; i++)
                {
                    var text = dr[columns[i]].ToString();

                    var lctcpr = lctr.GetTcArray(i).tcPr;
                    var nctct = nctr.AddNewTc();
                    if (lctcpr.gridSpan != null)
                    {
                        nctct.AddNewTcPr().AddNewGridspan().val = lctcpr.gridSpan.val;
                    }
                    
                    nctct.AddNewP().AddNewR().AddNewT().Value = text;
                }
                table.AddRow(new XWPFTableRow(nctr, table));
            }
        }
        #endregion

        #region 导入Excel
        /// <summary>
        /// Excel转化为DataSet
        /// </summary>
        /// <param name="isFirstColumn">是否首行为列名</param>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public DataSet ExcelToDataTableByStream(bool isFirstColumn, Stream stream, string fileName)
        {
            IWorkbook workbook = null;
            var ds = new DataSet();

            if (fileName.EndsWith(".xlsx"))
            {
                workbook = new XSSFWorkbook(stream);
            }
            else if (fileName.EndsWith(".xls"))
            {
                workbook = new HSSFWorkbook((FileStream) null);
            }

            var sheetNum = workbook?.NumberOfSheets;
            for (var x = 0; x < (sheetNum ?? 0); x++)
            {
                var data = new DataTable();
                var sheet = workbook?.GetSheetAt(x);

                var firstRow = sheet?.GetRow(0);
                if (firstRow == null) continue;

                int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                int startRow;
                if (isFirstColumn)
                {
                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        var column = new DataColumn(firstRow.GetCell(i).StringCellValue);
                        data.Columns.Add(column);
                    }
                    startRow = sheet.FirstRowNum + 1;
                }
                else
                {
                    startRow = sheet.FirstRowNum;
                }

                //最后一列的标号,即总的行数
                var rowCount = sheet.LastRowNum;
                for (var i = startRow; i <= rowCount; ++i)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue; //没有数据的行默认是null　　　　　　　

                    var dataRow = data.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {
                        if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                    data.Rows.Add(dataRow);
                }

                ds.Tables.Add(data);
            }

            return ds;
        }
        #endregion
    }
}
