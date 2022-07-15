using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using SAPR.DrawSrc;
using SAPR.RodSrc.Rules;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using SAPR.Processor;
using System.Text;
using SAPR.Utility;

namespace SAPR.RodSrc
{
    class RodManager: INotifyPropertyChanged

    {
        private const string MY_FOLDER = "\\SAPR_SAVES";
        private List<string> properties= new List<string>() {"Length","Area", "Elastic", "LinearStraining", "FirstNode", "LastNode" };
        private List<string> BlockNames = new List<string>() {"L, м", "A, cм ^ 2","E, МПа,","q, Н/м","F, qL","F, qL" };
        public Button addButton = new Button() { Content= "Добавить стержень" , Height = 45, Background = Brushes.LightGreen ,HorizontalAlignment = HorizontalAlignment.Stretch};
        public Button deleteButton = new Button() { Content = "Удалить стержень", Height = 45, Background = Brushes.Red, HorizontalAlignment = HorizontalAlignment.Stretch };
        private LinkedList<LinkedList<TextBox>>  TextBoxes = new LinkedList<LinkedList<TextBox>>();
        private Grid grid;
        private Draw drawer;
        private Construction construction;
        private Button Save;
        private Button Load;
        private Bases bases;
        private TextBox allowstraing;

        public RodManager(Grid grid, Canvas canvas,Bases bases, Construction construction, Button load, Button save, TextBox allowstraing)
        {
            allowInit(allowstraing);
            this.bases = bases;
            this.allowstraing = allowstraing;
            Save = save;
            Save.Click += Save_Click;
            Load = load;
            Load.Click += Load_Click;
            this.construction = construction;
            this.construction.PropertyChanged += Construction_PropertyChanged;
            drawer = new Draw(canvas);
            this.grid = grid;
            construction.SetBaseEvent(bases);
            AddRod(grid, new RoutedEventArgs());
            AddRowDefinition(); 
            AddRowDefinition();
            ButtonInit();
        }
        private void allowInit(TextBox allowstraing)
        {
            allowstraing.GotFocus += CleanTextBoxOnFocus;
            Binding binding = new Binding()
            {
                Source = Construction.Instance,
                Path = new PropertyPath("AllowStraining"),
                Mode = BindingMode.TwoWay,
                ValidatesOnDataErrors = true
            };
            binding.ValidationRules.Add(new DoubleRule());
            allowstraing.SetBinding(TextBox.TextProperty, binding);

        }


        private void Construction_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            drawer.TrueDraw(construction);
        }


        private void Load_Click(object sender, RoutedEventArgs e)
        {
            string myfolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + MY_FOLDER);
            Directory.CreateDirectory(myfolder);
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = myfolder + "\\";
            bool? result = fileDialog.ShowDialog();
            if (result == true) 
            { 
                string filename = fileDialog.FileName;
                string jsoncontruct;
                StreamReader streamReader = new StreamReader(filename);
                using (streamReader)
                {
                    jsoncontruct = streamReader.ReadToEnd();
                }
                Construction loadedconstruction = JsonConvert.DeserializeObject<Construction>(jsoncontruct);
                this.construction.AllowStraining = loadedconstruction.AllowStraining;
                construction.LeftBase = loadedconstruction.LeftBase;
                construction.RightBase = loadedconstruction.RightBase;
                foreach (LinkedList<TextBox> textBoxes in TextBoxes)
                {
                    foreach (TextBox textBox in textBoxes)
                    {
                        grid.Children.Remove(textBox);
                    }
                }
                TextBoxes.Clear();
                construction.Rods.Clear();
                int row = 1;
                foreach (Rod rod in loadedconstruction.Rods)
                {
                    AddRod(rod, row);
                    row += 1;
                }
            }

        }

        private void AddRod(Rod rod,int row)
        {
            rod.PropertyChanged += Rod_PropertyChanged;
            AddRowDefinition();
            CreateTextBoxes(row,rod);
            construction.Rods.AddLast(rod);
            SetOnGrid(addButton, 1, construction.Rods.Count + 1);
            SetOnGrid(deleteButton, 4, construction.Rods.Count + 1);
            drawer.TrueDraw(construction);
        }
        public void CreateTextBoxes(int row,Rod rodSource)
        {
            int i = 0;
            LinkedList<TextBox> temp = new LinkedList<TextBox>();
            foreach (string property in properties)
            {

                Binding binding = new Binding()
                {
                    Source = rodSource,
                    Path = new PropertyPath(property),
                    Mode = BindingMode.TwoWay,
                    ValidatesOnDataErrors = true
                };
                binding.ValidationRules.Add(ValidationRules(property, rodSource));
                var textBox = new TextBox()
                {
                    Height = 25,
                    MinWidth = 45
                };
                //textBox.GotFocus += CleanTextBoxOnFocus;
                //textBox.LostFocus += TextBox_LostFocus;
                textBox.SetBinding(TextBox.TextProperty, binding);
                AddOnGrid(textBox);
                SetOnGrid(textBox, i, row);
                i++;
                temp.AddLast(textBox);
            }
            TextBoxes.AddLast(temp);

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(ErrorCheck.CheckErrors(grid) is false)
            {
                return;
            }
            string save = JsonConvert.SerializeObject(construction);
            string myfolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + MY_FOLDER);
            Directory.CreateDirectory(myfolder);
            string[] files = Directory.GetFiles(myfolder);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = myfolder + "\\";
            saveFileDialog.FileName = $"save{files.Length + 1}.txt";
            saveFileDialog.ShowDialog();
            System.IO.File.WriteAllText(saveFileDialog.FileName, save);
        }     

        private void ButtonInit()
        {
            addButton.Click += AddRod;
            deleteButton.Click += DeleteRod;
            AddTextBlock();
            AddOnGrid(addButton);
            AddOnGrid(deleteButton);
            Grid.SetColumnSpan(addButton, 3);
            Grid.SetColumnSpan(deleteButton, 3);
            SetOnGrid(addButton, 1, 2);
            SetOnGrid(deleteButton, 4, 2);
        }

        private void AddRod(object sender, RoutedEventArgs e)
        {
            Rod rod = new Rod( construction.Rods.Count+1, 1,1, 1, 0,0,0);
            rod.PropertyChanged += Rod_PropertyChanged ;
            construction.Rods.AddLast(rod);
            AddRowDefinition();
            CreateTextBoxes();
            SetOnGrid(addButton, 1, construction.Rods.Count+1);
            SetOnGrid(deleteButton,4, construction.Rods.Count+1);
            drawer.TrueDraw(construction);

        }

        private void Rod_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            drawer.TrueDraw(construction);
        }

        private void DeleteRod(object sender, RoutedEventArgs e)
        {
            if(construction.Rods.Count <= 1) return;
            construction.Rods.RemoveLast();
            LinkedList<TextBox> temp = TextBoxes.Last.Value;
            foreach (TextBox textBox in temp)
                grid.Children.Remove(textBox);
            TextBoxes.RemoveLast();
            SetOnGrid(addButton, 1, construction.Rods.Count + 1);
            SetOnGrid(deleteButton, 4, construction.Rods.Count + 1);
            drawer.TrueDraw(construction);
        }

        public void CreateTextBoxes()
        {
            int row = construction.Rods.Count;
            int i = 0;
            LinkedList<TextBox> temp = new LinkedList<TextBox>();
            foreach (string property in properties)
            {

                Binding binding = new Binding()
                {
                    Source = construction.Rods.Last.Value,
                    Path = new PropertyPath(property),
                    Mode = BindingMode.TwoWay,
                    ValidatesOnDataErrors = true
                };
                binding.ValidationRules.Add(ValidationRules(property, construction.Rods.Last.Value));
                var textBox = new TextBox()
                {
                    Height = 25,
                    MinWidth = 45
                };
                textBox.GotFocus += CleanTextBoxOnFocus;
                textBox.LostFocus += TextBox_LostFocus;
                textBox.SetBinding(TextBox.TextProperty, binding);
                AddOnGrid(textBox);
                SetOnGrid(textBox,i,row);
                i++;
                temp.AddLast(textBox);
            }
            TextBoxes.AddLast(temp);

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if(tb.Text == String.Empty)
            {
                tb.Text = "1";
            }
        }

        private void CleanTextBoxOnFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = String.Empty;
        }

        private void AddTextBlock()
        {
            int i = 0;
            foreach (string property in BlockNames)
            {
                TextBlock textBlock = new TextBlock()
                {
                    Text = property,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Height = 30
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

        private ValidationRule ValidationRules(string property, Rod rod)
        {

            List<string> doublepos = new List<string>(7) {  "Length", "Area", "Elastic"};
            List<string> isdouble = new List<string>(7) {  "LinearLoad", "FirstNode" };
            if (doublepos.Contains(property))
            {
                return new DoublePositiveNumRule();
            }
            if (isdouble.Contains(property))
            {
                return new DoubleRule();
            }
            if (property == "Number")
            {
                return new PosIntRule();
            }
           /*if(property == "LastNode")
            {
                return new DoubleAndFirstNodeCheckRule(rod);
            }*/
            return new DoubleRule();

        }


        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




    }
}
