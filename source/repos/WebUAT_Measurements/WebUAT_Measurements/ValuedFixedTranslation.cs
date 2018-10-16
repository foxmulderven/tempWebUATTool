using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUAT_Measurements
{
    class ValuedFixedTranslation : Translation
    {
        //Determines what type of value this translation uses: Fraction, integer, text translation.
        public int valueType;
        //Helps to identify what this value represents. Will be assigned to the LABEL!
        public string valueDescription;
        public bool canUseVariable;
        public string variable;

        public ValuedFixedTranslation(string newEng_MY, string newEng_CAAZ, string newFr_CA, int newValueType, string newValueDescription, bool newCanUseVariable, string newVariable)
            : base(MyronWebUAT.dataType_fixedTranslation, newEng_MY, newEng_CAAZ, newFr_CA)
        {
            valueType = newValueType;
            valueDescription = newValueDescription;
            canUseVariable = newCanUseVariable;
            variable = newVariable;
        }

        public void FillValuedFixedTranslation(string newEng_MY, string newEng_CAAZ, string newFr_CA, int newValueType, string newValueDescription, bool newCanUseVariable, string newVariable)
        {
            FillTranslation(newEng_MY, newEng_CAAZ, newFr_CA);

            valueType = newValueType;
            valueDescription = newValueDescription;
            canUseVariable = newCanUseVariable;
            variable = newVariable;

        }
    }
}
