using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUAT_Measurements
{
    class Template
    {
        public string templateID;
        public string templateName;
        public List<string> itemSizeDataRowList;
        public List<string> imprintAreaDataRowList;
        public List<string> chargesDataRowList;

        public Template()
        {
            //get the last 4 digits of time in millis
            templateID = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString().Substring(4);
            Random index = new Random();
            int maxIndex = MyronWebUAT.id_charactersbank.Length - 1;
            // final id is 4 random characters from the bank + the 4 digits from the time in millis
            templateID = MyronWebUAT.dataType_template.ToString() + "_"
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + templateID;

            itemSizeDataRowList = new List<string>();
            imprintAreaDataRowList = new List<string>();
            chargesDataRowList = new List<string>();
        }

    }
}
