using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using SAPR.DrawSrc;
namespace SAPR.RodSrc
{
    class RodManager: INotifyPropertyChanged

    {
        private List<string> properties= new List<string>(6) {"Number","Length","Area", "Elastic",  "LinearLoad", "FirstNode", "LastNode" }; 
        public LinkedList<Rod> Rods = new LinkedList<Rod>();
        public Button addButton = new Button() { Content= "Добавить стержень" , Height = 45, Background = Brushes.Green};
        public Button deleteButton = new Button() { Content = "Удалить стержень", Height = 45, Background = Brushes.Red };
        private LinkedList<LinkedList<TextBox>>  TextBoxes = new LinkedList<LinkedList<TextBox>>();
        private Grid grid;
        private Draw drawer;
        public  RodManager(Grid grid,Canvas canvas)
        {
            drawer = new Draw(canvas);
            this.grid = grid;
            grid.ShowGridLines = true;
            AddRod(grid, new RoutedEventArgs());
            AddRowDefinition(); 
            AddRowDefinition();
            ButtonInit();


        }
        private void ButtonInit()
        {
            addButton.Click += AddRod;
            deleteButton.Click += DeleteRod;
            addTextBlock();
            AddOnGrid(addButton);
            AddOnGrid(deleteButton);
            Grid.SetColumnSpan(addButton, 4);
            Grid.SetColumnSpan(deleteButton, 4);
            SetOnGrid(addButton, 0, 2);
            SetOnGrid(deleteButton, 4, 2);
        }
        private void AddRod(object sender, RoutedEventArgs e)
        {
            Rod rod = new Rod(Rods.Count + 1, 1, 1, 1,1,1);
            rod.PropertyChanged += Rod_PropertyChanged ;
            Rods.AddLast(rod);
            AddRowDefinition();
            CreateTextBoxes();
            SetOnGrid(addButton, 0,Rods.Count+1);
            SetOnGrid(deleteButton,4, Rods.Count+1);

        }

        private void Rod_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            drawer.DrawOnCanvas(Rods);
        }

        private void DeleteRod(object sender, RoutedEventArgs e)
        {
            if(Rods.Count <= 1) return;
            Rods.RemoveLast();
            LinkedList<TextBox> temp = TextBoxes.Last.Value;
            foreach (TextBox textBox in temp)
                grid.Children.Remove(textBox);
            SetOnGrid(addButton, 1, Rods.Count + 1);
            SetOnGrid(deleteButton, 4, Rods.Count + 1);
        }

        public void CreateTextBoxes()
        {
            int row = Rods.Count;
            int i = 0;
            LinkedList<TextBox> temp = new LinkedList<TextBox>();
            foreach (string property in properties)
            {

                Binding binding = new Binding()
                {
                    Source = Rods.Last.Value,
                    Path = new PropertyPath(property),
                    Mode = BindingMode.TwoWay,
                    ValidatesOnDataErrors = true
                };
                DoublePositiveNumRule Rule = new DoublePositiveNumRule();
                Rule.ValidatesOnTargetUpdated = true;
                binding.ValidationRules.Add(Rule);
                var texBox = new TextBox()
                {
                    Height = 45,
                    MinWidth = 45
                };
                texBox.SetBinding(TextBox.TextProperty, binding);
                AddOnGrid(texBox);
                SetOnGrid(texBox,i,row);
                i++;
                temp.AddLast(texBox);
            }
            TextBoxes.AddLast(temp);

        }
        private void addTextBlock()
        {
            int i = 0;
            foreach (string property in properties)
            {
                TextBlock textBlock = new TextBlock()
                {
                    Text = property
                };
                AddOnGrid(textBlock);
                SetOnGrid(textBlock, i, 0);
                i++;

            }

        }

        private void AddRowDefinition()
        {
            RowDefinition rd = new RowDefinition();
            rd.Height = GridLength.Auto;
            grid.RowDefinitions.Add(rd);
        }
        private void SetOnGrid(UIElement uiElement, int collumn,int row)
        {
            Grid.SetColumn(uiElement, collumn);
            Grid.SetRow(uiElement, row);
        }
        private void AddOnGrid(UIElement uIElement)
        {
            grid.Children.Add(uIElement);
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




    }
}
