using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Xls;
using System.Drawing;
using Spire.Xls.Collections;

namespace Test.COM.Export
{
    public class SpireManager : ISpireManager
    {
        #region 导出文档
        /// <summary>
        /// 导出文档
        /// 仅替换文档变量
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        public MemoryStream ExportWord(string path, Dictionary<string, string> dics)
        {
            var document = new Document();
            document.LoadFromFile(path);

            //变量替换
            ReplaceWordContent(document, dics);

            var ms = new MemoryStream();
            document.SaveToStream(ms, Spire.Doc.FileFormat.Docx);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// 导出文档
        /// 替换内容及新增表格
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        /// <param name="tuples">表格信息：表头, key:字段名-value:列名, 数据源</param>
        public MemoryStream ExportWord(string path, Dictionary<string, string> dics,
            List<Tuple<string, Dictionary<string, string>, DataTable>> tuples)
        {
            var document = new Document();
            document.LoadFromFile(path);

            //变量替换
            ReplaceWordContent(document, dics);
            //添加表格
            foreach (var tuple in tuples)
                CreatWordTable(document, tuple.Item1, tuple.Item2, tuple.Item3);

            var ms = new MemoryStream();
            document.SaveToStream(ms, Spire.Doc.FileFormat.Docx);
            ms.Position = 0;
            return ms;
        }
        /// <summary>
        /// 导出文档
        /// 替换内容及扩展表格
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        /// <param name="tuples">表格信息：表格序号(从1开始), 字段名, 数据源</param>
        public MemoryStream ExportWord(string path, Dictionary<string, string> dics, 
            List<Tuple<int, string[], DataTable>> tuples)
        {
            var document = new Document();
            document.LoadFromFile(path);

            //变量替换
            ReplaceWordContent(document, dics);
            //扩展表格
            foreach (var tuple in tuples)
                ExtendWordTable(document.Sections[0].Tables[tuple.Item1 - 1] as Table, tuple.Item2, tuple.Item3);

            var ms = new MemoryStream();
            document.SaveToStream(ms, Spire.Doc.FileFormat.Docx);
            ms.Position = 0;
            return ms;
        }

        #region 内部方法
        /// <summary>
        /// 替换Word内容
        /// </summary>
        /// <param name="doc">文档</param>
        /// <param name="dics">变量信息：key:变量名-value:变量值</param>
        private void ReplaceWordContent(Document doc, Dictionary<string, string> dics)
        {
            if (doc == null || dics == null || !dics.Any())
                return;

            foreach (var dic in dics)
            {
                doc.FindString($"${{{dic.Key}}}", false, true).GetAsOneRange().Text = dic.Value;
            }
        }
        /// <summary>
        /// 新增Word表格
        /// </summary>
        /// <param name="doc">文档</param>
        /// <param name="title">表头</param>
        /// <param name="dics">标列信息：key:字段名-value:列名</param>
        /// <param name="dt">数据源</param>
        private void CreatWordTable(Document doc, string title, Dictionary<string, string> dics, DataTable dt)
        {
            if(doc == null || dics == null || !dics.Any() || dt == null)
                return;

            //添加表格
            var section = doc.Sections[0];

            //标题
            if (!string.IsNullOrEmpty(title))
            {
                var tph = section.AddParagraph();
                var tag = tph.AppendText(title);
                tph.Format.HorizontalAlignment = HorizontalAlignment.Center;//水平居中
                tag.CharacterFormat.Bold = true;
            }

            var table = section.AddTable(true);
            table.ResetCells(dt.Rows.Count + 1, dics.Count);

            //表头
            for (var i = 0; i < dics.Count; i++)
            {
                var aph = table[0, i].AddParagraph();
                aph.AppendText(dics.ElementAt(i).Value);
                aph.Format.HorizontalAlignment = HorizontalAlignment.Center;
            }

            //内容
            var row = 1;
            foreach (DataRow dr in dt.Rows)
            {
                var column = 0;
                foreach (var dic in dics)
                {
                    table[row, column].AddParagraph().AppendText(dr[dic.Key].ToString());
                    column++;
                }
                row++;
            }

            //换行
            section.AddParagraph();
        }
        /// <summary>
        /// 表格新增行
        /// </summary>
        /// <param name="table">表格</param>
        /// <param name="columns">字段名</param>
        /// <param name="dt">数据源</param>
        private void ExtendWordTable(Table table, string[] columns, DataTable dt)
        {
            if(table == null || columns == null || dt == null)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                var tr = table.AddRow(true, columns.Length);
                for (var i = 0; i < columns.Length; i++)
                {
                    tr.Cells[i].AddParagraph().AppendText(dr[columns[i]].ToString());
                }
            }
        }
        #endregion

        #endregion

        #region 导出表格

        public MemoryStream ExportExcel()
        {
            var wb = new Workbook();
            wb.Worksheets.Clear();
            var sheet = wb.Worksheets.Add("出行人表");

            var dataTable = new DataTable();

            #region 数据
            dataTable.Columns.Add(" ");
            dataTable.Columns.Add("中文名");
            dataTable.Columns.Add("称谓");
            dataTable.Columns.Add("姓");
            dataTable.Columns.Add("名");
            dataTable.Columns.Add("护照号码");
            dataTable.Columns.Add("出生日期");
            dataTable.Columns.Add("护照有效期");
            dataTable.Columns.Add("国籍");
            dataTable.Columns.Add("备注");

            dataTable.Rows.Add("No.", "Chinese name", "Title", "Last name", "First name", "Passport number",
                "Date of birth", "Date of  expire", "Nationaility", "REMARK");

            dataTable.Rows.Add("1", "徐婧", "MS", "XU", "JING", "E65992527",
                "1991-10-18", "2025-12-30", "CHN", "");
            dataTable.Rows.Add("2", "徐婧", "MS", "XU", "JING", "E65992527",
                "1991-10-18", "2025-12-30", "CHN", "");
            dataTable.Rows.Add("3", "徐婧", "MS", "XU", "JING", "E65992527",
                "1991-10-18", "2025-12-30", "CHN", "");
            dataTable.Rows.Add("4", "徐婧", "MS", "XU", "JING", "E65992527",
                "1991-10-18", "2025-12-30", "CHN", "");
            dataTable.Rows.Add("5", "徐婧", "MS", "XU", "JING", "E65992527",
                "1991-10-18", "2025-12-30", "CHN", "");
            dataTable.Rows.Add("6", "徐婧", "MS", "XU", "JING", "E65992527",
                "1991-10-18", "2025-12-30", "CHN", "");
            dataTable.Rows.Add("7", "徐婧", "MS", "XU", "JING", "E65992527",
                "1991-10-18", "2025-12-30", "CHN", "");
            dataTable.Rows.Add("8", "徐婧", "MS", "XU", "JING", "E65992527",
                "1991-10-18", "2025-12-30", "CHN", "");
            dataTable.Rows.Add("9", "徐婧", "MS", "XU", "JING", "E65992527",
                "1991-10-18", "2025-12-30", "CHN", "");
            dataTable.Rows.Add("10", "徐婧", "MS", "XU", "JING", "E65992527",
                "1991-10-18", "2025-12-30", "CHN", "");
            #endregion

            var row = dataTable.Rows.Count;
            //将DataTable数据写入工作表
            sheet.InsertDataTable(dataTable, true, 1, 1, true);

            #region 设置样式
            var style1 = wb.Styles.Add("style1");
            style1.Font.FontName = "宋体";
            style1.Font.Size = 11;
            style1.HorizontalAlignment = HorizontalAlignType.Center;

            var style2 = wb.Styles.Add("style2");
            style2.Font.FontName = "宋体";
            style2.Font.Size = 11;
            style2.Font.IsBold = true;
            style2.HorizontalAlignment = HorizontalAlignType.Center;

            //设置字体
            sheet.Range["A1:J1"].CellStyleName = style1.Name;
            sheet.Range["A2:J2"].CellStyleName = style2.Name;
            sheet.Range[$"A3:J{row + 2}"].CellStyleName = style1.Name;

            //背景色
            sheet.Range["A2:J2"].Style.Color = Color.DarkOrange;

            //设置网格线样式及颜色
            sheet.Range[$"A1:J{row + 2}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A1:J{row + 2}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A1:J{row + 2}"].BorderAround(LineStyleType.Thin);

            #endregion 样式

            sheet.Range[$"C{row + 4}:H{row + 6}"].CellStyleName = style1.Name;

            sheet.Range[$"C{row + 4}"].RichText.Text = "2018/8/19";
            sheet.Range[$"C{row + 5}"].RichText.Text = "2018/8/24";
            sheet.Range[$"C{row + 6}"].RichText.Text = "34 ADT + 11 CHD + 0 INF";

            sheet.Range[$"D{row + 4}"].RichText.Text = "狮航（SL） 973";
            sheet.Range[$"D{row + 5}"].RichText.Text = "狮航（SL） 972";

            sheet.Range[$"E{row + 4}"].RichText.Text = "NKG";
            sheet.Range[$"E{row + 5}"].RichText.Text = "HKT";

            sheet.Range[$"F{row + 4}"].RichText.Text = "HKT";
            sheet.Range[$"F{row + 5}"].RichText.Text = "NKG";

            sheet.Range[$"G{row + 4}"].RichText.Text = "1545";
            sheet.Range[$"G{row + 5}"].RichText.Text = "815";

            sheet.Range[$"H{row + 4}"].RichText.Text = "1945";
            sheet.Range[$"H{row + 4}"].RichText.Text = "1405";

            sheet.Range[$"C{row + 6}:H{row + 6}"].Merge();
            sheet.Range[$"C{row + 4}:H{row + 6}"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"C{row + 4}:H{row + 6}"].BorderInside(LineStyleType.Thin);

            //冻结行
            sheet.FreezePanes(3, 1);

            //筛选器
            var filters = sheet.AutoFilters;
            filters.Range = sheet.Range[2, 1, sheet.LastRow, 10];

            //自适应列宽
            sheet.Range.AutoFitColumns();

            var ms = new MemoryStream();
            wb.SaveToStream(ms, Spire.Xls.FileFormat.Version2007);
            ms.Position = 0;
            return ms;
        }


        #endregion
    }
}
