using System;
namespace WebUAT_Measurements
{
    class Translation
    {
        //Unique id for this translation
        public string translationID;
        //Basic translations. Used as prefix in derived translations classes.
        public string eng_MY;
        public string eng_CAAZ;
        public string fr_CA;

        public Translation() { }

        public Translation(int translationType, string newEng_MY, string newEng_CAAZ, string newFr_CA)
        {
            //get the last 4 digits of time in millis
            translationID = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString().Substring(4);
            Random index = new Random();
            int maxIndex = MyronWebUAT.id_charactersbank.Length-1;
            // final id is 4 random characters from the bank + the 4 digits from the time in millis
            translationID = translationType.ToString() + "_"
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString() 
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + translationID;
            //Assign the new translations
            eng_MY = newEng_MY;
            eng_CAAZ = newEng_CAAZ;
            fr_CA = newFr_CA;
        }

        public Translation(string newEng_MY, string newEng_CAAZ, string newFr_CA)
        {
            //get the last 4 digits of time in millis
            translationID = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString().Substring(4);
            Random index = new Random();
            int maxIndex = MyronWebUAT.id_charactersbank.Length - 1;
            // final id is 4 random characters from the bank + the 4 digits from the time in millis
            translationID = MyronWebUAT.dataType_stdTranslation.ToString() + "_"
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + MyronWebUAT.id_charactersbank[index.Next(0, maxIndex)].ToString()
                + translationID;
            //Assign the new translations
            eng_MY = newEng_MY;
            eng_CAAZ = newEng_CAAZ;
            fr_CA = newFr_CA;
        }

        public void FillTranslation(string newEng_MY, string newEng_CAAZ, string newFr_CA)
        {
            eng_MY = newEng_MY;
            eng_CAAZ = newEng_CAAZ;
            fr_CA = newFr_CA;
        }
    }
}
