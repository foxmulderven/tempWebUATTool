using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;


namespace WebUAT_Measurements
{
    class WebUATController
    {
        public List<Translation> standardTranslations;
        public List<Translation> customTextTranslations;
        public List<ValuedFixedTranslation> fixedTranslations;
        public List<ValuedConditionedTranslation> conditionedTranslations;
        public List<DataRow> dataRows;
        public List<Template> templates;

        public WebUATController()
        {
            standardTranslations = new List<Translation>();
            customTextTranslations = new List<Translation>();
            fixedTranslations = new List<ValuedFixedTranslation>();
            conditionedTranslations = new List<ValuedConditionedTranslation>();
            dataRows = new List<DataRow>();
            templates = new List<Template>();
        }

        /// <summary>
        /// Constructor that loads from the JSON files all the data
        /// </summary>
        public static WebUATController LoadWebUATJsonData()
        {
            WebUATController result = null;

            //Check if there's previous WebUATController data saved as json
            if (File.Exists(MyronWebUAT.json_filepath))
            {
                // If so, load the file and return the object
                StreamReader streamReader = new StreamReader(MyronWebUAT.json_filepath);
                JsonTextReader reader = new JsonTextReader(streamReader);
                JsonSerializer serializer = new JsonSerializer();
                result = serializer.Deserialize<WebUATController>(reader);
                reader.Close();
            }
            // If file doesn't exist create a new instance of WebUATController and save it as Json 
            else
            {
                result = new WebUATController();
                result.SaveWebUATJsonData();
            }

            return result;
        }

        public void SaveWebUATJsonData()
        {
            using (StreamWriter file = File.CreateText(MyronWebUAT.json_filepath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, this);
                file.Dispose();
            }
        }

        /// <summary>
        /// Gets the description of the translation that matches the id entered in params, doesn't matter what kind of translation is it.
        /// If not found, empty string is returned.
        /// </summary>
        public string GetTranslationDescription(string translationID)
        {
            string description = "";
            Translation translation = FindTranslationById(translationID);

            //Check if the translation was found
            if (translation != null)
            {
                if (translationID.StartsWith(MyronWebUAT.dataType_customText.ToString() + "_"))
                {
                    description = translation.eng_MY;
                }
                else if (translationID.StartsWith(MyronWebUAT.dataType_stdTranslation.ToString() + "_"))
                {
                    description = translation.eng_MY;
                }
                else if (translationID.StartsWith(MyronWebUAT.dataType_fixedTranslation.ToString() + "_"))
                {
                    description = ((ValuedFixedTranslation)translation).valueDescription;
                }
                else if (translationID.StartsWith(MyronWebUAT.dataType_conditionedTranslation.ToString() + "_"))
                {
                    description = ((ValuedConditionedTranslation)translation).valueDescription;
                }

            }
            return description;
        }

        /// <summary>
        /// Returns the Translation object that matches the translationID entered as param. If not found, null is returned. 
        /// Note that even if it's a Translation, it can be a reference to a ValuedFixedTranslation or ValuedConditionedTranslation 
        /// (cast requried).
        /// </summary>
        public Translation FindTranslationById(string translationID)
        {
            Translation translation = null;
            bool notFound = true;
            int i = 0;

            //Evaluate the id to know which list should be used for the search
            if (translationID.StartsWith(MyronWebUAT.dataType_customText.ToString() + "_"))
            {
                while (notFound && i < customTextTranslations.Count)
                {
                    if (customTextTranslations[i].translationID == translationID)
                    {
                        notFound = false;
                        translation = customTextTranslations[i];
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            else if (translationID.StartsWith(MyronWebUAT.dataType_stdTranslation.ToString() + "_"))
            {
                while (notFound && i < standardTranslations.Count)
                {
                    if (standardTranslations[i].translationID == translationID)
                    {
                        notFound = false;
                        translation = standardTranslations[i];
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            else if (translationID.StartsWith(MyronWebUAT.dataType_fixedTranslation.ToString() + "_"))
            {
                while (notFound && i < fixedTranslations.Count)
                {
                    if (fixedTranslations[i].translationID == translationID)
                    {
                        notFound = false;
                        translation = fixedTranslations[i];
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            else if (translationID.StartsWith(MyronWebUAT.dataType_conditionedTranslation.ToString() + "_"))
            {
                while (notFound && i < conditionedTranslations.Count)
                {
                    if (conditionedTranslations[i].translationID == translationID)
                    {
                        notFound = false;
                        translation = conditionedTranslations[i];
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return translation;
        }

        /// <summary>
        /// This method returns the DataRow that matches the id entered as param.
        /// </summary>
        public DataRow FindDataRowById(string dataRowID)
        {
            DataRow temp = null;
            bool notFound = true;
            int index = 0;
            while (notFound && index < dataRows.Count)
            {
                if (dataRows[index].dataRowID == dataRowID)
                {
                    notFound = false;
                    temp = dataRows[index];
                }
                else
                {
                    index++;
                }
            }

            return temp;
        }
    }
}
