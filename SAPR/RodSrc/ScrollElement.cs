using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace SAPR.RodSrc
{
    class ScrollElement
    {
        public TextBox textBox;
        public ScrollElement(Rod source, string property)
        {
            textBox = new TextBox();
            textBox.Text = property;
            Binding binding = new Binding();
            binding.Source = source;
            binding.Path = new PropertyPath(property);
            textBox.SetBinding(TextBox.TextProperty,binding);
        }

    }
}
