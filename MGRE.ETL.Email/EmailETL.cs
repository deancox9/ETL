using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using MGRE.ETL.Common;
using MGRE.ETL.Contracts;
using MGRE.ETL.Application.Data;

using PRUPIM.Common.EmailService.Server;
using PRUPIM.Common.EmailService.Client;

namespace MGRE.ETL.Email
{
    #region .Net Class Documentation
    /// <summary>
    /// Class with methods for Emailing ETL data rather than generating csv files
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  <revision by="deanc" ref="24063" date="2013-07-30">created</revision>
    /// </history>
    #endregion
    public class EmailETL
    {
        public EmailETL() { }

        public bool Email(ETLExportDefinition exportDefinition)
        {
            return Email(exportDefinition, "");
        }

        public bool Email(ETLExportDefinition exportDefinition, string etlDirectory)
        {

            //Hold emails in collection so that we email all or nothing if an error is encountered.
            List<MailMessageSender> emails = new List<MailMessageSender>();
            ETLData data = new ETLData(exportDefinition.ApplicationDatabase);

            try
            {
                foreach (ETLExportEmail email in exportDefinition.Emails)
                {
                    MailMessageSender msg = new MailMessageSender();
                    msg.AppId = AppSettingsWrapper.applicationIdSetting.Value;

                    msg.FromAddress = email.FromAddress;
                    msg.Subject = email.Subject;

                    if (!email.EmailIsDataSpecific)
                    {
                        if (email.ToAssignees != null)
                        {
                            string[] tos = email.ToAssignees.Split(',');

                            foreach (string to in tos)
                            {
                                msg.Recipients.AddTO(to);
                            }
                        }

                        if (email.CCAssignees != null)
                        {
                            string[] ccs = email.CCAssignees.Split(',');

                            foreach (string cc in ccs)
                            {
                                msg.Recipients.AddCC(cc);
                            }
                        }

                        if (email.BCCAssignees != null)
                        {
                            string[] bccs = email.BCCAssignees.Split(',');

                            foreach (string bcc in bccs)
                            {
                                msg.Recipients.AddBCC(bcc);
                            }
                        }
                    }

                    if (etlDirectory.Length == 0)
                    {
                        //Data is explicitly listed in the email for someone to manually type in the data into Yardi Voyager

                        DataSet ds = new DataSet();
                        ds = data.GetETLData(email.ProcedureName, "EmailData");

                        if (ds != null)
                        {
                            //Send one email per row
                            //This only works for a flat structure table (not implemented a solution that will allow Parent - child relationship)
                            //In the case of Property and Job Cost from AIMS this can be explained in a flat structure table.
                            //An example that doesn't follow this is Capval Property Valuation and multiple unit ERV records.
                            //At present CapVal is not requesting an email solution, hopefully capval will only ever use an ETL solution that 
                            //does not experience this issue.  Not sure how an email solution would work anyway with regards an email solution - 
                            //potentially a lot of data to manually key in!

                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                if (email.EmailIsDataSpecific)
                                {
                                    string[] tos = row["_EmailRecipients"].ToString().Split(',');

                                    foreach (string to in tos)
                                    {
                                        msg.Recipients.AddTO(to);
                                    }

                                    //May need to add CCs here as well if developer adds a cc column?

                                }

                                msg.Body = ExamineData(row, email.Body);
                                msg.IsHtml = true;

                                emails.Add(msg);

                            }

                        }
                    }
                    else
                    {
                        //Attach ETL files to Email for someone to manually import the data via the ETL screens in Voyager
                        foreach (string etlFile in Directory.GetFiles(etlDirectory))
                        {
                            FileInfo file = new FileInfo(etlFile);

                            Attachment attach = new Attachment(etlDirectory, file.Name);

                            msg.Attachments.Add(attach);
                        }

                        msg.Body = email.Body;
                        msg.IsHtml = true;

                        emails.Add(msg);
                    }
                }

                foreach (MailMessageSender email in emails)
                {
                    email.SendMail();
                }

                //Now mark ETL row as processed! - update _Status column of record
                data.UpdateETLDataStatus(exportDefinition.StatusProcedureName, (int)Common.Enums.ETLStatus.Success);
            }
            catch (Exception ex)
            {
                MGRELog.Write(ex);
                //Now mark ETL row as failed! - update _Status column of record
                data.UpdateETLDataStatus(exportDefinition.StatusProcedureName, (int)Common.Enums.ETLStatus.Failed);
            }

            return true;
        }

        private string ExamineData(DataRow row, string data)
        {
            if (data.Contains("[") && data.Contains("]"))
            {
                string alteredData = "";
                //We have dynamic New Starter info to insert
                string[] dataChunks = data.Split(']');

                foreach (string dataChunk in dataChunks)
                {
                    //Replace [..] with new starter info
                    int startIndex = dataChunk.IndexOf('[');
                    if (startIndex >= 0)
                    {
                        string fieldName = dataChunk.Substring(startIndex + 1);

                        //Default NewStarter table
                        alteredData += dataChunk.Substring(0, startIndex) + ExamineColumnType(row, fieldName);
                    }
                    else
                    {
                        alteredData += dataChunk;
                    }
                }

                return alteredData;
            }
            else
            {
                //No dynamic new starter info, just return data passed in
                return data;
            }
        }

        private string ExamineColumnType(DataRow row, string column)
        {
            if (row.Table.Columns[column].DataType.ToString() == "System.Byte")
            {
                if (row[column].ToString() == "1") return "Yes";
                else return "No";
            }
            else
            {
                return row[column].ToString();
            }
        }

    }
}
