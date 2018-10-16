using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUAT_Measurements
{
    class ValuedConditionedTranslation : Translation
    {
        //Helps to identify what this value represents. Will be assigned to the LABEL!
        public string valueDescription;
        public bool canUseVariable;
        public string variable;
        public int conditionType;
        public string elseEng_MY;
        public string elseEng_CAAZ;
        public string elseFR_CA;

        public ValuedConditionedTranslation(string newEng_MY, string newEng_CAAZ, string newFR_CA,
            string newValueDescription, bool newCanUseVariable, string newVariable,
            int newConditionType, string newElseEng_MY, string newElseEng_CAAZ, string newElseEng_FRCA)
            : base(MyronWebUAT.dataType_conditionedTranslation, newEng_MY, newEng_CAAZ, newFR_CA)
        {
            valueDescription = newValueDescription;
            canUseVariable = newCanUseVariable;
            variable = newVariable;
            conditionType = newConditionType;
            elseEng_MY = newElseEng_MY;
            elseEng_CAAZ = newElseEng_CAAZ;
            elseFR_CA = newElseEng_FRCA;
        }

        public void FillValuedConditionedTranslation(string newEng_MY, string newEng_CAAZ, string newFR_CA,
            string newValueDescription, bool newCanUseVariable, string newVariable,
            int newConditionType, string newElseEng_MY, string newElseEng_CAAZ, string newElseEng_FRCA)
        {
            FillTranslation(newEng_MY, newEng_CAAZ, newFR_CA);

            valueDescription = newValueDescription;
            canUseVariable = newCanUseVariable;
            variable = newVariable;
            conditionType = newConditionType;
            elseEng_MY = newElseEng_MY;
            elseEng_CAAZ = newElseEng_CAAZ;
            elseFR_CA = newElseEng_FRCA;
        }

    }
}
