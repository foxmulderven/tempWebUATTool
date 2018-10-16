using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUAT_Measurements
{
    class MyronWebUAT
    {
        //Commit test for GIThub

        //Characters bank
        public static string id_charactersbank = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        //JSON filepath
        public static string json_filepath = @"" + AppDomain.CurrentDomain.BaseDirectory + "WebUATData.json";

        //TAB INDEXES
        public static int tab_measurements = 0;
        public static int tab_admin = 1;

        //ADMIN CONSTANTS
        //Index of the content types on the combo box. UPDATE IF THE COMBO BOX DATA CHANGES!
        public static int admin_notSelected = -1; //Used in multiple selection checks
        public static int admin_contenttype_dataRows = 0;
        public static int admin_contenttype_standardTranslations = 1;
        public static int admin_contenttype_valuedFixedTranslations = 2;
        public static int admin_contenttype_valuedConditionedTranslations = 3;
        public static int admin_contenttype_templates = 4;

        //Edition modes (buttons in the middle, to determine what action to perform when save is clicked)
        public static int admin_editmode_none = -1;
        public static int admin_editmode_new = 0;
        public static int admin_editmode_edit = 1;
        
        //TRANSLATIONS CONSTANTS
        public static int valuetype_fraction = 0;
        public static int valuetype_integer = 1;
        public static int valuetype_translation = 2;
        public static string valueToken = "{x}";
        //Condition constants. Every time a new condition type is added must be also added here
        public static int condtiontype_qtyEqual1 = 1;
        public static int conditiontype_lastValueOnDataRow = 0;
        //DataRow constants. This also identifies the translations by the id. Each one starts with number_FULLID
        public static int dataType_customText = 0;
        public static int dataType_stdTranslation = 1;
        public static int dataType_fixedTranslation = 2;
        public static int dataType_conditionedTranslation = 3;
        public static int dataType_dataRow = 4;
        public static int dataType_template = 5;
    }
}
