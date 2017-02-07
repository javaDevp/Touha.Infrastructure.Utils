using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Touha.Infrastructure.Utils.Office
{
    public class ExcelHelper
    {
        #region Fields
        /// <summary>
        /// 单例
        /// </summary>
        private static volatile ExcelHelper _helper;
        /// <summary>
        /// 锁对象
        /// </summary>
        private static readonly object _lockObj = new object();
        /// <summary>
        /// 文件路径
        /// </summary>
        private readonly string _filePath;
        #endregion

        #region Constructors
        /// <summary>
        /// 私有构造方法
        /// </summary>
        /// <param name="filePath"></param>
        private ExcelHelper(string filePath)
        {
            _filePath = filePath;
        }
        /// <summary>
        /// 单例方法
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static ExcelHelper GetInstance(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("filePath");
            if (_helper == null)
            {
                lock (_lockObj)
                {
                    if (_helper == null)
                        _helper = new ExcelHelper(filePath);
                }
            }
            return _helper;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 获取Excel文档中表格数
        /// </summary>
        /// <returns></returns>
        public int GetTableCount(string sheetName)
        {
            XSSFWorkbook workbook = new XSSFWorkbook(_filePath);
            XSSFSheet sheet = workbook.GetSheet(sheetName) as XSSFSheet;
            if (sheet != null)
                return sheet.GetTables().Count;
            throw new ArgumentException("sheetName");
            //Workbook workbook = new Workbook(_filePath);
            //Worksheet worksheet = GetSheetByName(workbook, sheetName);
            //return worksheet.Tables.Count;
        }

        /// <summary>
        /// 获取指定表格数据
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="index"></param>
        /// <param name="map">指定word中表头与DataTable的ColumnName的映射关系</param>
        /// <returns></returns>
        public DataTable GetTableData(string sheetName, int index, Dictionary<string, string> map)
        {
            XSSFWorkbook workbook = new XSSFWorkbook(_filePath);
            XSSFSheet sheet = workbook.GetSheet(sheetName) as XSSFSheet;
            DataTable dt = new DataTable();
            if (sheet != null)
            {
                if (sheet.GetTables().Count > index)
                {
                    XSSFTable table = sheet.GetTables()[index];
                    CellReference startCellReference = table.GetStartCellReference();
                    CellReference endCellReference = table.GetEndCellReference();
                    IRow rowHead = sheet.GetRow(startCellReference.Row);
                    //构造DataTable列
                    int matchCount = 0;
                    for (int i = startCellReference.Col; i <= endCellReference.Col; i++)
                    {
                        if (map.ContainsKey(rowHead.Cells[i].StringCellValue))
                        {
                            string columnName;
                            if (map.TryGetValue(rowHead.Cells[i].StringCellValue, out columnName))
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
                    for (int i = startCellReference.Row + 1; i <= endCellReference.Row; i++)
                    {
                        IRow rowData = sheet.GetRow(i);
                        DataRow dr = dt.NewRow();
                        for (int k = startCellReference.Col; k <= endCellReference.Col; k++)
                        {
                            //判断是否为需要的列数据
                            if (map.ContainsKey(rowHead.Cells[k].StringCellValue))
                            {
                                dr[map[rowHead.Cells[k].StringCellValue]] = rowData.Cells[k].StringCellValue;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    throw new ArgumentException("index");
                }
            }
            else
            {
                throw new ArgumentException("sheetName");
            }
            return dt;
        }
        #endregion
    }
}
