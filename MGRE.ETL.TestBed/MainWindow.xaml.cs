using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MGRE.ETL.Contracts;
using MGRE.ETL.Business.Rules;
using MGRE.ETL.Common;

namespace MGRE.ETL.TestBed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            RunExportNowByName("Property");
        }

        private void RunExportNowByName(string importName)
        {
            try
            {
                MGRE.ETL.Data.Contracts.IETLDAL etlDAL = new Data.ETLData();

                ETLExportBO bo = new ETLExportBO(etlDAL);

                bo.RunExportNow(importName, "deanc");

                //ETLImportBO boImp = new ETLImportBO(etlDAL);
                //boImp.RunImportNow("CADFinTxn", "deanc");
            }
            catch (MGREException vex)
            {
                MGRELog.Write(vex);
                //throw new FaultException<MGREExceptionData>(vex.AppError, new FaultReason(vex.Message));
            }
            catch (Exception ex)
            {
                MGRELog.Write(ex);
                throw;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MGRE.ETL.Data.Contracts.IETLDAL etlDAL = new Data.ETLData();

                //ETLExportBO bo = new ETLExportBO(etlDAL);

                //bo.RunExportNow(importName, "deanc");

                DualAppBO bo = new DualAppBO(etlDAL);
                DualAppShortcutSetting settings =  bo.GetSettings(127);

            }
            catch (MGREException vex)
            {
                MGRELog.Write(vex);
                //throw new FaultException<MGREExceptionData>(vex.AppError, new FaultReason(vex.Message));
            }
            catch (Exception ex)
            {
                MGRELog.Write(ex);
                throw;
            }
        }
    }
}
