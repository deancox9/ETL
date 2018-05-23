using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGRE.ETL.Import
{
    public class CSVData
    {
        string columnName;
        string dataValue;
        int rowNumber;

        public CSVData(string columnName, string dataValue, int rowNumber)
        {
            this.columnName = columnName;
            this.dataValue = dataValue;
            this.rowNumber = rowNumber;
        }

        public string ColumnName
        {
            get { return columnName; }
            set { value = columnName; }
        }

        public string DataValue
        {
            get { return dataValue; }
            set { value = dataValue; }
        }

        public int RowNumber
        {
            get { return rowNumber; }
            set { value = rowNumber; }
        }
    }
}
