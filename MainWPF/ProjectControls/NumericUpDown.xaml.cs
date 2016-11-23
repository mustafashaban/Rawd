using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
        }
        public static RoutedCommand UpRoutedCommand = new RoutedCommand();
        public static RoutedCommand DownRoutedCommand = new RoutedCommand();
        public static RoutedCommand EnterRoutedCommand = new RoutedCommand();

        public delegate void ValueEventHandler(object sender, RoutedPropertyChangedEventArgs<object> e);
        public event ValueEventHandler ValueChanged;

        public int? Value
        {
            get { return (int?)GetValue(ValueProperty); }
            set
            {
                if (!value.HasValue)
                {
                    btnMinus.IsEnabled = true;
                    btnAdd.IsEnabled = true;
                    txt.Text = "";
                }
                else
                {
                    btnMinus.IsEnabled = value.Value != Minimum;
                    btnAdd.IsEnabled = value.Value != Maximum;
                    txt.Text = value.ToString();
                }
                SetValue(ValueProperty, value);
                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int?), typeof(NumericUpDown), new FrameworkPropertyMetadata(new int?(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ValuePropertyChanged)));

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown x = (NumericUpDown)d;
            x.Value = (int?)e.NewValue;
        }

        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set
            {
                if (value < Minimum)
                    throw new Exception("Minimum Value Must be less than Maximum Value");
                SetValue(MinimumProperty, value);
            }
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0));

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { 
                if(value < Minimum)
                    throw new Exception("Maximum Value Must be more than Minimum Value");
                SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(NumericUpDown), new PropertyMetadata(100));


        private void RepeatButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            if (!Value.HasValue)
                Value = Minimum;
            else if (Value.Value > Maximum - 1)
                return;
            else Value = Value.Value + 1;
        }
        private void RepeatButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            if (!Value.HasValue)
                Value = Minimum;
            else if (Value.Value < Minimum + 1)
                return;
            else Value = Value.Value - 1;
        }
        private void ExecutedUpCommand(object sender, ExecutedRoutedEventArgs e)
        {
            RepeatButtonPlus_Click(null, null);
        }
        private void ExecutedDownCommand(object sender, ExecutedRoutedEventArgs e)
        {
            RepeatButtonMinus_Click(null, null);
        }
        private void ExecutedEnterCommand(object sender, ExecutedRoutedEventArgs e)
        {
            txt_LostFocus(null, null);
        }

        private void txt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txt.Text.Trim() == "")
            {
                if (Value.HasValue)
                    Value = null;
            }
            else
            {
                try
                {
                    int num = int.Parse(txt.Text.Trim());
                    if (num > Maximum || num < Minimum)
                        Value = Minimum;
                    else Value = num;
                }
                catch
                {
                    Value = null;
                }
            }
        }

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            txt.SelectAll();
        }
    }
}
