using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SAPR.Utility
{
    class ErrorCheck
    {
        static public bool CheckErrors(UIElement uIElement)
        {
            StringBuilder sb = new StringBuilder();
            GetErrors(sb, uIElement);
            string message = sb.ToString();
            if (message != "")
            {
                MessageBox.Show(message);
                return false;
            }
            return true;
        }

        static private void GetErrors(StringBuilder sb, DependencyObject obj)
        {
            foreach (object child in LogicalTreeHelper.GetChildren(obj))
            {
                TextBox element = child as TextBox;
                if (element == null) continue;

                if (Validation.GetHasError(element))
                {
                    sb.Append(element.Text + " найдена ошибка:\r\n");
                    foreach (ValidationError error in Validation.GetErrors(element))
                    {
                        sb.Append("  " + error.ErrorContent.ToString());
                        sb.Append("\r\n");
                    }
                }

                GetErrors(sb, element);
            }
        }
    }
}
