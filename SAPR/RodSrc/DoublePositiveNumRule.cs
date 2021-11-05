using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SAPR.RodSrc
{
    class DoublePositiveNumRule : ValidationRule
    {
        public Type ValidationType { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = value.ToString();

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Value cannot be coverted to string.");
            bool canConvert = false;
            double doubleVal = 0;
            canConvert = double.TryParse(strValue, NumberStyles.Float,cultureInfo,out doubleVal);
            return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Double");

        
    }
}
}
