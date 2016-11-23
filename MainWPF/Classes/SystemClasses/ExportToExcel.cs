﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data;

namespace MainWPF
{
    class ExportToExcel
    {
        /// <summary>
        /// Class for generator of Excel file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        public class ExportListToExcel<T, U> where T : class where U : List<T>
        {
            public ExportListToExcel(List<T> ListToPrint)
            {
                dataToPrint = ListToPrint;
            }
            public ExportListToExcel(List<T> ListToPrint, List<string> HeaderData)
            {
                dataToPrint = ListToPrint;
                headerData = HeaderData;
            }
            List<string> headerData;
            List<T> dataToPrint;
            // Excel object references.
            private Excel.Application _excelApp = null;
            private Excel.Workbooks _books = null;
            private Excel._Workbook _book = null;
            private Excel.Sheets _sheets = null;
            private Excel._Worksheet _sheet = null;
            private Excel.Range _range = null;
            private Excel.Font _font = null;
            // Optional argument variable
            private object _optionalValue = Missing.Value;

            /// <summary>
            /// Generate report and sub functions
            /// </summary>
            public void GenerateReport()
            {
                try
                {
                    if (dataToPrint != null)
                    {
                        if (dataToPrint.Count != 0)
                        {
                            Mouse.SetCursor(Cursors.Wait);
                            CreateExcelRef();
                            FillSheet();
                            OpenReport();
                            Mouse.SetCursor(Cursors.Arrow);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error while generating Excel report");
                }
                finally
                {
                    ReleaseObject(_sheet);
                    ReleaseObject(_sheets);
                    ReleaseObject(_book);
                    ReleaseObject(_books);
                    ReleaseObject(_excelApp);
                }
            }
            /// <summary>
            /// Make MS Excel application visible
            /// </summary>
            private void OpenReport()
            {
                _excelApp.Visible = true;
            }
            /// <summary>
            /// Populate the Excel sheet
            /// </summary>
            private void FillSheet()
            {
                object[] header = CreateHeader();
                WriteData(header);
            }
            /// <summary>
            /// Write data into the Excel sheet
            /// </summary>
            /// <param name="header"></param>
            private void WriteData(object[] header)
            {
                object[,] objData = new object[dataToPrint.Count, header.Length];

                for (int j = 0; j < dataToPrint.Count; j++)
                {
                    var item = dataToPrint[j];
                    for (int i = 0; i < header.Length; i++)
                    {
                        var y = typeof(T).InvokeMember(header[i].ToString(),
                        BindingFlags.GetProperty, null, item, null);
                        objData[j, i] = (y == null) ? "" : y.ToString();
                    }
                }
                AddExcelRows("A2", dataToPrint.Count, header.Length, objData);
                AutoFitColumns("A1", dataToPrint.Count + 1, header.Length);
            }
            /// <summary>
            /// Method to make columns auto fit according to data
            /// </summary>
            /// <param name="startRange"></param>
            /// <param name="rowCount"></param>
            /// <param name="colCount"></param>
            private void AutoFitColumns(string startRange, int rowCount, int colCount)
            {
                _range = _sheet.get_Range(startRange, _optionalValue);
                _range = _range.get_Resize(rowCount, colCount);
                _range.Columns.AutoFit();
            }
            /// <summary>
            /// Create header from the properties
            /// </summary>
            /// <returns></returns>
            private object[] CreateHeader()
            {
                if (headerData == null)
                {
                    PropertyInfo[] headerInfo = typeof(T).GetProperties();

                    // Create an array for the headers and add it to the
                    // worksheet starting at cell A1.
                    List<object> objHeaders = new List<object>();
                    for (int n = 0; n < headerInfo.Length; n++)
                    {
                        objHeaders.Add(headerInfo[n].Name);
                    }

                    var headerToAdd = objHeaders.ToArray();
                    AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
                    SetHeaderStyle();

                    return headerToAdd;
                }
                else
                {
                    var headerToAdd = headerData.ToArray();
                    AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
                    SetHeaderStyle();

                    return headerToAdd;
                }
            }
            /// <summary>
            /// Set Header style as bold
            /// </summary>
            private void SetHeaderStyle()
            {
                _font = _range.Font;
                _font.Bold = true;
            }
            /// <summary>
            /// Method to add an excel rows
            /// </summary>
            /// <param name="startRange"></param>
            /// <param name="rowCount"></param>
            /// <param name="colCount"></param>
            /// <param name="values"></param>
            private void AddExcelRows(string startRange, int rowCount, int colCount, object values)
            {
                _range = _sheet.get_Range(startRange, _optionalValue);
                _range = _range.get_Resize(rowCount, colCount);
                _range.set_Value(_optionalValue, values);
            }
            /// <summary>
            /// Create Excel application parameters instances
            /// </summary>
            private void CreateExcelRef()
            {
                _excelApp = new Excel.Application();
                _books = (Excel.Workbooks)_excelApp.Workbooks;
                _book = (Excel._Workbook)(_books.Add(_optionalValue));
                _sheets = (Excel.Sheets)_book.Worksheets;
                _sheet = (Excel._Worksheet)(_sheets.get_Item(1));
            }
            /// <summary>
            /// Release unused COM objects
            /// </summary>
            /// <param name="obj"></param>
            private void ReleaseObject(object obj)
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
                catch (Exception ex)
                {
                    obj = null;
                    MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    GC.Collect();
                }
            }
        }


        public class ExportDataTableToExcel
        {
            public ExportDataTableToExcel(DataTable TableToPrint)
            {
                dataToPrint = TableToPrint;
            }
            DataTable dataToPrint;
            // Excel object references.
            private Excel.Application _excelApp = null;
            private Excel.Workbooks _books = null;
            private Excel._Workbook _book = null;
            private Excel.Sheets _sheets = null;
            private Excel._Worksheet _sheet = null;
            private Excel.Range _range = null;
            private Excel.Font _font = null;
            // Optional argument variable
            private object _optionalValue = Missing.Value;

            /// <summary>
            /// Generate report and sub functions
            /// </summary>
            public void GenerateReport()
            {
                try
                {
                    if (dataToPrint != null)
                    {
                        if (dataToPrint.Rows.Count != 0)
                        {
                            CreateExcelRef();
                            FillSheet();
                            OpenReport();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error while generating Excel report");
                }
                finally
                {
                    ReleaseObject(_sheet);
                    ReleaseObject(_sheets);
                    ReleaseObject(_book);
                    ReleaseObject(_books);
                    ReleaseObject(_excelApp);
                }
            }
            /// <summary>
            /// Make MS Excel application visible
            /// </summary>
            private void OpenReport()
            {
                _excelApp.Visible = true;
            }
            /// <summary>
            /// Populate the Excel sheet
            /// </summary>
            private void FillSheet()
            {
                object[] header = CreateHeader();
                WriteData(header);
            }
            /// <summary>
            /// Write data into the Excel sheet
            /// </summary>
            /// <param name="header"></param>
            private void WriteData(object[] header)
            {
                object[,] objData = new object[dataToPrint.Rows.Count, header.Length];

                for (int i = 0; i < dataToPrint.Rows.Count; i++)
                {
                    for (int j = 0; j < header.Length; j++)
                    {
                        var item = dataToPrint.Rows[i].ItemArray[j];
                        if (item != null)
                        {
                            if (item is DateTime)
                                objData[i, j] = ((DateTime)item).ToString("dd/MM/yyyy");
                            else if (item is bool)
                                objData[i, j] = ((bool)item) ? 1 : 0;
                            else
                                objData[i, j] = item.ToString();
                        }
                        else
                        {
                            objData[i, j] = "";
                        }
                    }
                }
                AddExcelRows("A2", dataToPrint.Rows.Count, header.Length, objData);
                _sheet.DisplayRightToLeft = true;
                AutoFitColumns("A1", dataToPrint.Rows.Count + 1, header.Length);
            }
            /// <summary>
            /// Method to make columns auto fit according to data
            /// </summary>
            /// <param name="startRange"></param>
            /// <param name="rowCount"></param>
            /// <param name="colCount"></param>
            private void AutoFitColumns(string startRange, int rowCount, int colCount)
            {
                _range = _sheet.get_Range(startRange, _optionalValue);
                _range = _range.get_Resize(rowCount, colCount);
                _range.Columns.AutoFit();
            }
            /// <summary>
            /// Create header from the properties
            /// </summary>
            /// <returns></returns>
            private object[] CreateHeader()
            {
                List<string> objHeaders = new List<string>();
                for (int n = 0; n < dataToPrint.Columns.Count; n++)
                {
                    objHeaders.Add(dataToPrint.Columns[n].Caption);
                }

                var headerToAdd = objHeaders.ToArray();
                AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
                SetHeaderStyle();

                return headerToAdd;
            }
            /// <summary>
            /// Set Header style as bold
            /// </summary>
            private void SetHeaderStyle()
            {
                _font = _range.Font;
                _font.Bold = true;
            }
            /// <summary>
            /// Method to add an excel rows
            /// </summary>
            /// <param name="startRange"></param>
            /// <param name="rowCount"></param>
            /// <param name="colCount"></param>
            /// <param name="values"></param>
            private void AddExcelRows(string startRange, int rowCount, int colCount, object values)
            {
                _range = _sheet.get_Range(startRange, _optionalValue);
                _range = _range.get_Resize(rowCount, colCount);
                _range.set_Value(_optionalValue, values);
            }
            /// <summary>
            /// Create Excel application parameters instances
            /// </summary>
            private void CreateExcelRef()
            {
                _excelApp = new Excel.Application();
                _books = (Excel.Workbooks)_excelApp.Workbooks;
                _book = (Excel._Workbook)(_books.Add(_optionalValue));
                _sheets = (Excel.Sheets)_book.Worksheets;
                _sheet = (Excel._Worksheet)(_sheets.get_Item(1));
            }
            /// <summary>
            /// Release unused COM objects
            /// </summary>
            /// <param name="obj"></param>
            private void ReleaseObject(object obj)
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
                catch (Exception ex)
                {
                    obj = null;
                    MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    GC.Collect();
                }
            }
        }


        public class ExportDataSetToExcel
        {
            public ExportDataSetToExcel(DataSet DataSetToPrint)
            {
                ds = DataSetToPrint;
            }
            DataSet ds;
            DataTable currentDataTable;
            // Excel object references.
            private Excel.Application _excelApp = null;
            private Excel.Workbooks _books = null;
            private Excel._Workbook _book = null;
            private Excel.Sheets _sheets = null;
            private Excel._Worksheet _sheet = null;
            private Excel.Range _range = null;
            private Excel.Font _font = null;
            // Optional argument variable
            private object _optionalValue = Missing.Value;

            /// <summary>
            /// Generate report and sub functions
            /// </summary>
            public void GenerateReport()
            {
                try
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            currentDataTable = ds.Tables[i];
                            if (currentDataTable.Rows.Count != 0)
                            {
                                if (_sheet == null) CreateExcelRef();
                                else
                                {
                                    _sheet = _excelApp.Sheets.Add();
                                }
                                FillSheet();
                            }
                        }
                        OpenReport();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error while generating Excel report");
                }
                finally
                {
                    ReleaseObject(_sheet);
                    ReleaseObject(_sheets);
                    ReleaseObject(_book);
                    ReleaseObject(_books);
                    ReleaseObject(_excelApp);
                }
            }
            /// <summary>
            /// Make MS Excel application visible
            /// </summary>
            private void OpenReport()
            {
                _excelApp.Visible = true;
            }
            /// <summary>
            /// Populate the Excel sheet
            /// </summary>
            private void FillSheet()
            {
                object[] header = CreateHeader();
                WriteData(header);
            }
            /// <summary>
            /// Write data into the Excel sheet
            /// </summary>
            /// <param name="header"></param>
            private void WriteData(object[] header)
            {
                object[,] objData = new object[currentDataTable.Rows.Count, header.Length];

                for (int i = 0; i < currentDataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < header.Length; j++)
                    {
                        var item = currentDataTable.Rows[i].ItemArray[j];
                        if (item != null)
                        {
                            if (item is DateTime)
                                objData[i, j] = ((DateTime)item).ToString("dd/MM/yyyy");
                            else if (item is bool)
                                objData[i, j] = ((bool)item) ? 1 : 0;
                            else
                                objData[i, j] = item.ToString();
                        }
                        else
                        {
                            objData[i, j] = "";
                        }
                    }
                }
                AddExcelRows("A2", currentDataTable.Rows.Count, header.Length, objData);
                _sheet.DisplayRightToLeft = true;
                AutoFitColumns("A1", currentDataTable.Rows.Count + 1, header.Length);
            }
            /// <summary>
            /// Method to make columns auto fit according to data
            /// </summary>
            /// <param name="startRange"></param>
            /// <param name="rowCount"></param>
            /// <param name="colCount"></param>
            private void AutoFitColumns(string startRange, int rowCount, int colCount)
            {
                _range = _sheet.get_Range(startRange, _optionalValue);
                _range = _range.get_Resize(rowCount, colCount);
                _range.Columns.AutoFit();
            }
            /// <summary>
            /// Create header from the properties
            /// </summary>
            /// <returns></returns>
            private object[] CreateHeader()
            {
                List<string> objHeaders = new List<string>();
                for (int n = 0; n < currentDataTable.Columns.Count; n++)
                {
                    objHeaders.Add(currentDataTable.Columns[n].Caption);
                }

                var headerToAdd = objHeaders.ToArray();
                AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
                SetHeaderStyle();

                return headerToAdd;
            }
            /// <summary>
            /// Set Header style as bold
            /// </summary>
            private void SetHeaderStyle()
            {
                _font = _range.Font;
                _font.Bold = true;
            }
            /// <summary>
            /// Method to add an excel rows
            /// </summary>
            /// <param name="startRange"></param>
            /// <param name="rowCount"></param>
            /// <param name="colCount"></param>
            /// <param name="values"></param>
            private void AddExcelRows(string startRange, int rowCount, int colCount, object values)
            {
                _range = _sheet.get_Range(startRange, _optionalValue);
                _range = _range.get_Resize(rowCount, colCount);
                _range.set_Value(_optionalValue, values);
            }
            /// <summary>
            /// Create Excel application parameters instances
            /// </summary>
            private void CreateExcelRef()
            {
                _excelApp = new Excel.Application();
                _books = (Excel.Workbooks)_excelApp.Workbooks;
                _book = (Excel._Workbook)(_books.Add(_optionalValue));
                _sheets = (Excel.Sheets)_book.Worksheets;
                _sheet = (Excel._Worksheet)(_sheets.get_Item(1));
            }
            /// <summary>
            /// Release unused COM objects
            /// </summary>
            /// <param name="obj"></param>
            private void ReleaseObject(object obj)
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
                catch (Exception ex)
                {
                    obj = null;
                    MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    GC.Collect();
                }
            }
        }
    }
}