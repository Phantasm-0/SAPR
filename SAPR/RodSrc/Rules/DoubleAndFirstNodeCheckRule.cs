/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using System.Windows.Controls;
namespace SAPR.RodSrc
{
    class DoubleAndFirstNodeCheckRule : ValidationRule
    {
        public DoubleAndFirstNodeCheckRule(Rod rod)
        {
            myrod = rod;
        }
        private Rod myrod;
        private bool iszero = false;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            LinkedList<Rod> rods = RodManager.GetRods();
            if (DoubleCheck(value, cultureInfo))
            {
                if (myrod.Number == 1 || iszero == true)
                {
                    return new ValidationResult(true, null);
                }
                else
                {
                    LinkedListNode<Rod> prevrod = rods.Find(myrod).Previous;
                    if (prevrod.Value.FirstNode != 0)
                    {
                        return new ValidationResult(false, $" Previous rod have value in first node");
                    }
                    else
                        return new ValidationResult(true, null);
                }
            }
            else
                return new ValidationResult(false, $"Not a double");
        }
        private bool DoubleCheck(object value,CultureInfo cultureInfo)
        {
            string strValue = value.ToString();
            if (string.IsNullOrEmpty(strValue))
                return true;
            bool canConvert = false;
            double doubleVal = 0;
            canConvert = double.TryParse(strValue, NumberStyles.Float, cultureInfo, out doubleVal);
            if (doubleVal == 0 && canConvert == true)
            {
                iszero = true;
                return true;
            }
            else
                return canConvert;
        }


    }
}
*/
