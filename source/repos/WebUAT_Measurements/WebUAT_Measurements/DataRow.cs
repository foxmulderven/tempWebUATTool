using System;
using System.Collections.Generic;

namespace WebUAT_Measurements
{
    class DataRow
    {
        public string dataRowID;
        public string description;
        public List<string> dataRowParts;

        public DataRow()
        {
            //get the last 4 digits of time in millis
            dataRowID = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString().Substring(4);
            Random index = new Random();
            int maxIndex = MyronWebUAT.id_charactersbank.Length - 1;
            // final id is 4 random characters from the bank + the 4 digits from the time in millis
            dataRowID = MyronWebUAT.dataType_dataRow.ToString() + "_"
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + dataRowID;

            //Instantiate the parts List
            dataRowParts = new List<string>();
        }


    }
}
