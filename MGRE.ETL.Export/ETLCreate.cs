using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using MGRE.ETL.Common;
using MGRE.ETL.Contracts;
using MGRE.ETL.Application.Data;
using MGRE.ETL.Email;

namespace MGRE.ETL.Export
{
    #region .Net Class Documentation
    /// <summary>
    /// Class with methods for creating ETL files
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  <revision by="deanc" ref="24063" date="2013-07-30">created</revision>
    /// </history>
    #endregion
    public class ETLCreate
    {
        public ETLCreate() { }

        public bool CreateETLFiles(ETLExportDefinition exportDefinition, string exportFileLocation, string userName)
        {
            ETLData data = new ETLData(exportDefinition.ApplicationDatabase);
            DateTime startDateTime = DateTime.Now;
            try
            {
                foreach (ETLExportTablesDefinition table in exportDefinition.TableDefinitions)
                {
                    //Retrieve DataSet for particular ETL Table
                    DataSet ds = new DataSet();
                    ds = data.GetETLData(table.ProcedureName, table.TableName);

                    if (ds != null)
                    {
                        //Save all files into temp area (in case one or more files fail)
                        string exportETLFileLocation = exportFileLocation + @"\" +
                                                    exportDefinition.ExportSubDirectory + @"\Temp\" +
                                                    table.ETLFileName.Replace(".csv", DateTimeSuffix());

                        CreateCSVFile(ds, exportETLFileLocation, table.ETLHeaderName);
                    }
                }

                
                //Move all files from temp area into pick-up area
                foreach (string etlFile in Directory.GetFiles(exportFileLocation + @"\" +
                                                    exportDefinition.ExportSubDirectory + @"\Temp\"))
                {
                    System.IO.File.Move(etlFile, etlFile.Replace(@"Temp\", ""));
                }

                ////Now mark ETL as processed! - update _Status column of record
                //data.UpdateETLDataStatus(exportDefinition.StatusProcedureName, (int)Common.Enums.ETLStatus.Success);
                //Status update is done in Email method below!

                //Email notification that file has been created....
                Email.EmailETL email = new Email.EmailETL();
                email.Email(exportDefinition, "");

                //Record Import History
                ETLData hubData = new ETLData("HUB");
                hubData.RecordHistory(exportDefinition.ETLExportGUID, userName, startDateTime, true);

                
            }
            catch (Exception ex)
            {
                MGRELog.Write(ex);

                //Now mark ETL as failed! - update _Status column of record
                data.UpdateETLDataStatus(exportDefinition.StatusProcedureName, (int)Common.Enums.ETLStatus.Failed);

                //Record Import History
                ETLData hubData = new ETLData("HUB");
                hubData.RecordHistory(exportDefinition.ETLExportGUID, userName, startDateTime, false);

                throw ex;
            }

            return true;
        }

        public bool CreateETLFilesForEmail(ETLExportDefinition exportDefinition, string exportFileLocation, string userName)
        {
            ETLData data = new ETLData(exportDefinition.ApplicationDatabase);
            DateTime startDateTime = DateTime.Now;
            try
            {
                foreach (ETLExportTablesDefinition table in exportDefinition.TableDefinitions)
                {
                    //Retrieve DataSet for particular ETL Table
                    DataSet ds = new DataSet();
                    ds = data.GetETLData(table.ProcedureName, table.TableName);

                    if (ds != null)
                    {
                        //Save all files into temp area (in case one or more files fail)
                        string exportETLFileLocation = exportFileLocation + @"\" +
                                                    exportDefinition.ExportSubDirectory + @"\Temp\" +
                                                    table.ETLFileName.Replace(".csv", DateTimeSuffix());

                        CreateCSVFile(ds, exportETLFileLocation, table.ETLHeaderName);
                    }
                }

                //Email ETL File
                Email.EmailETL email = new Email.EmailETL();
                email.Email(exportDefinition, exportFileLocation + @"\" + exportDefinition.ExportSubDirectory + @"\Temp\");
              
                //Delete files from temp area
                foreach (string etlFile in Directory.GetFiles(exportFileLocation + @"\" +
                                                    exportDefinition.ExportSubDirectory + @"\Temp\"))
                {
                    System.IO.File.Delete(etlFile);
                }

                //Record Import History
                ETLData hubData = new ETLData("HUB");
                hubData.RecordHistory(exportDefinition.ETLExportGUID, userName, startDateTime, true);

            }
            catch (Exception ex)
            {
                MGRELog.Write(ex);

                //Delete files from temp area
                foreach (string etlFile in Directory.GetFiles(exportFileLocation + @"\" +
                                                    exportDefinition.ExportSubDirectory + @"\Temp\"))
                {
                    System.IO.File.Delete(etlFile);
                }

                //Now mark ETL as failed! - update _Status column of record
                data.UpdateETLDataStatus(exportDefinition.StatusProcedureName, (int)Common.Enums.ETLStatus.Failed);

                //Record Import History
                ETLData hubData = new ETLData("HUB");
                hubData.RecordHistory(exportDefinition.ETLExportGUID, userName, startDateTime, false);

                throw ex;
            }

            return true;
        }

        private static void CreateCSVFile(DataSet data,
                                    string csvFilename,
                                    string etlHeader)
        {

            //Create a StreamWriter object to open and write contents
            //of the DataGridView columns and rows.
            StreamWriter sr = File.CreateText(csvFilename);

            //Create a variable to hold the delimiter type 
            //(i.e., TAB or comma or whatever you choose)
            //The default for this procedure is a comma (",").
            string delimiter = ",";

            //Create a variable that holds the total number of columns
            //in the DataSet.
            int columnCount = data.Tables[0].Columns.Count - 1;

            if (etlHeader.Length != 0)
            {
                sr.WriteLine(etlHeader);
            }

            //Create a variable to hold the row data
            string rowData = "";

            //Interate through each column and get/write the column name.
            for (int i = 0; i <= columnCount; i++)
            {
                //Exclude any columns that are prefixed _
                //These columns are not part of the ETL schema and are used by the upstream application (e.g. _Status is used to marke the status of each individual ETL row)
                if (data.Tables[0].Columns[i].ColumnName.StartsWith("_") == false)
                {
                    //The Replace function will remove the delimiter
                    //from the field data if found.
                    rowData += data.Tables[0].Columns[i].ColumnName.Replace(delimiter, "") + (i < columnCount ? delimiter : "");
                }
            }

            //Write the column header data to the CSV file.
            sr.WriteLine(rowData);

            //Now collect data for each row and write to the CSV file.
            //Loop through each row in the DataGridView.
            foreach (DataRow row in data.Tables[0].Rows)
            {

                //Reset the value of the strRowData variable
                rowData = "";

                for (int j = 0; j <= columnCount; j++)
                {
                    if (data.Tables[0].Columns[j].ColumnName.StartsWith("_") == false)
                    {
                        //The IIf statement will not put a delimiter after the 
                        //last value added.

                        //The Replace function will remove the delimiter
                        //from the field data if found.
                        if (row[j].ToString() != "")
                        {
                            rowData += row[j].ToString().Replace(delimiter, "") + (j < columnCount ? delimiter : "");
                        }
                        else
                        {
                            rowData += (j < columnCount ? delimiter : "");
                        }


                    }
                }

                //Write the row data to the CSV file.
                sr.WriteLine(rowData);

            }

            //Close the StreamWriter object.
            sr.Close();

        }

        private string DateTimeSuffix()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        }

    }
}
