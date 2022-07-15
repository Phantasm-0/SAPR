using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SAPR.RodSrc.Rules
{
    class PosIntRule : ValidationRule
    {
        public Type ValidationType { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = value.ToString();

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Value cannot be coverted to string.");
            bool canConvert = false;
            int intVal = 0;
            canConvert = int.TryParse(strValue, NumberStyles.Integer, cultureInfo, out intVal);
            if (intVal <= 0)
            {
                return new ValidationResult(false, $"Value cannot be less than 0");
            }
            return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Double");


        }
    }
}