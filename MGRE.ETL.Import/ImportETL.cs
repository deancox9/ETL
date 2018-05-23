using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Globalization;

using MGRE.ETL.Common;
using MGRE.ETL.Contracts;
using MGRE.ETL.Application.Data;

namespace MGRE.ETL.Import
{
    #region .Net Class Documentation
    /// <summary>
    /// Class with methods for importing ETL files
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  <revision by="deanc" ref="24063" date="2013-07-30">created</revision>
    /// </history>
    #endregion
    public class ImportETL
    {
        public ImportETL() { }

        public bool ImportETLFile(ETLImportDefinition importDefinition, string importFileLocation, string userName)
        {
            ETLData data = new ETLData("HUB");
            DateTime startDateTime = DateTime.Now;

            int lineNo = 1;
            
            List<string> tempDataValues = new List<string>();
            bool dataHeldOverMultipleLines = false;

            try
            {
                foreach (ETLImportTablesDefinition table in importDefinition.TableDefinitions)
                {
                    //Locate csv file
                    string filePath = importFileLocation + @"\" + importDefinition.ImportSubDirectory + @"\" + table.ETLFileName;
                    string sFileContents = "";
                    using (StreamReader oStreamReader = new StreamReader(File.OpenRead(filePath)))
                    {
                        sFileContents = oStreamReader.ReadToEnd();
                    }

                    

                    //Replace any carriage Returns conatined within "s.
                    sFileContents = Regex.Replace(sFileContents,
                                    @"\r\n(?=[^""]*""(?:[^""]*""[^""]*"")*[^""]*$)",
                                    String.Empty);
                    
                    DataSet tableToPopulate = new DataSet();

                    tableToPopulate = data.GetTableDataSet(table.TableName, table.SelectProcedureName);

                    //List<CSVData> csvList = new List<CSVData>();

                    string[] fileLines = sFileContents.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    
                    List<string> columnNames = new List<string>();

                    int dataStartLineNo = table.LineDataStarts;

                    int footerLineCount = table.FooterLineCount;

                    foreach (string line in fileLines)
                    {

                        //Could be a case (especially decimal values) whereby a comma (thousand delimeter) can be in the file
                        //e.g. "-28,000" - In these cases remove any commas that exist between "s.
                        string fileLine = Regex.Replace(line,
                                        @",(?=[^""]*""(?:[^""]*""[^""]*"")*[^""]*$)",
                                        String.Empty);

                        if (lineNo == 1)
                        {
                            //Check header is correct
                            if (fileLine.Replace(",", "").Trim().ToUpper() != table.ETLHeaderName.ToUpper())
                            {
                                throw new MGREException("ETL file header is incorrect : " + fileLine.Trim().ToUpper());
                            }
                        }
                        else if (lineNo == dataStartLineNo)
                        {
                            //Retrieve column names and hold in array
                            columnNames = fileLine.Split(",".ToCharArray()).ToList<string>();
                        }
                        else if (lineNo == fileLines.Count() - footerLineCount)
                        {
                            //End of file exit loop
                            break;
                        }
                        else if (lineNo > dataStartLineNo)
                        {
                            List<string> dataValues = new List<string>();
                            //Build up CSVData list recording column Name, Data Value and row number
                            if (!dataHeldOverMultipleLines)
                            {
                                tempDataValues.Clear();
                                dataValues = fileLine.Split(",".ToCharArray()).ToList<string>();
                            }
                            else
                            {
                                int j = 0;
                                foreach (string l in fileLine.Split(",".ToCharArray()))
                                {
                                    if (j == 0)
                                    {
                                        tempDataValues[tempDataValues.Count - 1] = tempDataValues[tempDataValues.Count - 1] + " " + l;
                                    }
                                    else
                                    {
                                        tempDataValues.Add(l);
                                    }
                                    j++;
                                }
                            }

                            if (columnNames.Count != tableToPopulate.Tables[table.TableName].Columns.Count)
                            {
                                throw new MGREException("Number of columns in ETL file do not match number of columns in database table : " + table.TableName);
                            }

                            if (dataValues.Count != columnNames.Count && tempDataValues.Count != columnNames.Count)
                            {
                                //We have an issue where data is held over multiple lines (by carriage returns), most likely candidate is notes
                                //In this case we need to cache the current datavalues and append subsequent data until we get the complete line of data.
                                if (!dataHeldOverMultipleLines)
                                {
                                    tempDataValues = dataValues;
                                }
                                dataHeldOverMultipleLines = true;
                            }
                            else
                            {
                                if (dataHeldOverMultipleLines)
                                {
                                    dataValues = tempDataValues;
                                }

                                int columnNo = 0;
                                DataRow row = tableToPopulate.Tables[table.TableName].NewRow();
                                foreach (string dataValue in dataValues)
                                {
                                    DataColumn col = tableToPopulate.Tables[table.TableName].Columns[columnNames[columnNo].Trim()];

                                    if (col == null)
                                    {
                                        throw new MGREException("Possible ETL Schema Change, column not found : " + columnNames[columnNo].Trim());
                                    }

                                    row[columnNames[columnNo].Trim()] = SetValue(dataValue, col.DataType);
                                    columnNo++;
                                }

                                tableToPopulate.Tables[table.TableName].Rows.Add(row);
                                dataHeldOverMultipleLines = false;
                            }


                            
                        }

                        lineNo++;
                    }

                    if (table.DeleteProcedureName.Length > 0)
                    {
                        //Clear out table before populating
                        data.DeleteData(table.TableName, table.DeleteProcedureName);
                    }

                    data.InsertImportData(tableToPopulate, table.TableName, table.InsertProcedureName);
                }

                //Record Import History
                data.RecordHistory(importDefinition.ETLImportGUID, userName, startDateTime, true);

                //Move file to Success Folder
                foreach (ETLImportTablesDefinition table in importDefinition.TableDefinitions)
                {
                    string filePath = importFileLocation + @"\" + importDefinition.ImportSubDirectory + @"\" + table.ETLFileName;
                    string successPath = importFileLocation + @"\" + importDefinition.ImportSubDirectory + @"\Success\" + table.ETLFileName.Replace(".csv", startDateTime.Year.ToString() + startDateTime.Month.ToString() + startDateTime.Day.ToString() + startDateTime.Hour.ToString() + startDateTime.Minute.ToString() + ".csv");
                    System.IO.File.Move(filePath, successPath);
                }

                return true;
            }
            catch (Exception ex)
            {
                MGRELog.Write(ex);

                //Record Import History
                data.RecordHistory(importDefinition.ETLImportGUID, userName, startDateTime, false);

                //Move file to Fail Folder
                foreach (ETLImportTablesDefinition table in importDefinition.TableDefinitions)
                {
                    string filePath = importFileLocation + @"\" + importDefinition.ImportSubDirectory + @"\" + table.ETLFileName;
                    string failPath = importFileLocation + @"\" + importDefinition.ImportSubDirectory + @"\Fail\" + table.ETLFileName.Replace(".csv", startDateTime.Year.ToString() + startDateTime.Month.ToString() + startDateTime.Day.ToString() + startDateTime.Hour.ToString() + startDateTime.Minute.ToString() + ".csv");
                    System.IO.File.Move(filePath, failPath);
                }

                throw ex;
            }
        }

        private object SetValue(string dataValue, Type dataType)
        {
            if (dataValue == "")
            {
                return null;
            }

            switch (dataType.Name.ToUpper())
            {
                case "STRING":
                    return dataValue;
                case "DATETIME":
                    //Assume that Voyager ETL exports dates in UK format
                    string temp = dataValue;
                    string day = temp.Substring(0, temp.IndexOf("/"));
                    temp = temp.Substring(day.Length + 1);
                    string month = temp.Substring(0, temp.IndexOf("/"));
                    temp = temp.Substring(month.Length + 1);                 
                    string year = temp;

                    DateTime date = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day));

                    return date;    
                case "INT32":
                    return Int32.Parse(dataValue);
                case "DECIMAL":
                    decimal number;
                    if (decimal.TryParse(dataValue.Replace(@"""", ""), out number))
                    {
                        return number;
                    }
                    else
                    {
                        throw new MGREException("Unable to convert decimal value in csv file");
                    }
            }
            return null;
        }

    }

}
