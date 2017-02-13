using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XWPF.UserModel;

namespace Touha.Infrastructure.Utils.Office
{
    public class WordHelper
    {
        #region Fields
        /// <summary>
        /// 文件路径
        /// </summary>
        private string _filePath;
        /// <summary>
        /// 单例
        /// </summary>
        private static volatile WordHelper _helper;
        /// <summary>
        /// 锁对象
        /// </summary>
        private static readonly object _lockObj = new object();
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        private WordHelper(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// 单例模式
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static WordHelper GetInstance(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("filePath");
            if (_helper == null)
            {
                lock (_lockObj)
                {
                    if (_helper == null)
                        _helper = new WordHelper(filePath);
                }
            }
            return _helper;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 获取word文档中表格数
        /// </summary>
        /// <returns></returns>
        public int GetTableCount()
        {
            using (Stream stream = File.Open(_filePath, FileMode.Open))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                return doc.Tables.Count;
            }

        }

        /// <summary>
        /// 获取指定表格数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="map">指定word中表头与DataTable的ColumnName的映射关系</param>
        /// <returns></returns>
        public DataTable GetTableData(int index, Dictionary<string, string> map)
        {
            using (Stream stream = File.Open(_filePath, FileMode.Open))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                DataTable dt = new DataTable();
                if (doc.Tables.Count > index)
                {
                    XWPFTable table = doc.Tables[index];
                    IList<XWPFTableRow> rows = table.Rows;
                    XWPFTableRow rowHead = rows[0];
                    //构造DataTable列
                    int matchCount = 0;
                    foreach (var col in rowHead.GetTableCells())
                    {
                        if (map.ContainsKey(col.GetText()))
                        {
                            string columnName;
                            if (map.TryGetValue(col.GetText(), out columnName))
                            {
                                dt.Columns.Add(columnName);
                                matchCount++;
                            }
                            else
                            {
                                throw new Exception("表格格式不正确");
                            }
                        }
                    }
                    if (matchCount != map.Keys.Count)
                        throw new Exception("表格格式不正确");
                    //填充数据
                    for (int i = 0; i < rows.Count - 1; i++)
                    {
                        XWPFTableRow rowData = rows[i + 1];
                        DataRow dr = dt.NewRow();
                        for (int k = 0; k < rowData.GetTableCells().Count; k++)
                        {
                            //判断是否为需要的列数据
                            if (map.ContainsKey(rowHead.GetTableCells()[k].GetText()))
                            {
                                dr[map[rowHead.GetTableCells()[k].GetText()]] = rowData.GetTableCells()[k].GetText();
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    throw new ArgumentException("index");
                }
                return dt;
            }
        }

        /// <summary>
        /// 将指定表格数据转XML
        /// </summary>
        /// <param name="index"></param>
        /// <param name="columnName"></param>
        /// <param name="rootName"></param>
        /// <returns></returns>
        public string GetTableXml(int index, string columnName, string rootName)
        {
            string xml = "";
            using (Stream stream = File.Open(_filePath, FileMode.Open))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                if (doc.Tables.Count > index)
                {
                    xml += xml += string.Format("<{0}>", rootName);
                    XWPFTable table = doc.Tables[index];
                    IList<XWPFTableRow> rows = table.Rows;
                    XWPFTableRow rowHead = rows[0];
                    //寻找作为Xml节点的列
                    //var cells = from cell in rowHead.GetTableCells()
                    //    where cell.GetText().Equals(column, StringComparison.OrdinalIgnoreCase)
                    //    select cell;
                    bool isOutter = false;
                    string preOuterColumn = "";
                    int columnIndex = -1;
                    for (int i = 0; i < rowHead.GetTableCells().Count; i++)
                    {
                        var cell = rowHead.GetCell(i);
                        if (cell.GetText().Equals(columnName))
                        {
                            columnIndex = i;
                            break;
                        }
                    }
                    if (columnIndex > -1)
                    {
                        //填充数据
                        for (int i = 0; i < rows.Count - 1; i++)
                        {
                            XWPFTableRow rowData = rows[i + 1];
                            if (rowData.GetTableCells().Count > 1)
                            {
                                //叶子节点
                                xml += string.Format("<{0}></{0}>", rowData.GetCell(columnIndex).GetText());
                            }
                            if (rowData.GetTableCells().Count == 1)
                            {
                                //非叶子节点
                                if (isOutter)
                                {
                                    isOutter = false;
                                    xml += string.Format("</{0}>", preOuterColumn);
                                }
                                preOuterColumn = rowData.GetCell(0).GetText();
                                xml += string.Format("<{0}>", preOuterColumn);
                                isOutter = true;
                            }
                        }
                        if (isOutter)
                        {
                            isOutter = false;
                            xml += string.Format("</{0}>", preOuterColumn);
                        }
                        xml += string.Format("</{0}>", rootName);
                    }

                }
            }
            return xml;
        }
        #endregion
    }
}
