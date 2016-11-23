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
    public partial class DoubleUpDown : UserControl
    {
        public static RoutedCommand UpRoutedCommand = new RoutedCommand();
        public static RoutedCommand DownRoutedCommand = new RoutedCommand();
        public static RoutedCommand EnterRoutedCommand = new RoutedCommand();

        public delegate void ValueEventHandler(object sender, RoutedPropertyChangedEventArgs<object> e);
        public event ValueEventHandler ValueChanged;
        public DoubleUpDown()
        {
            InitializeComponent();
        }
        public double? SelectedValue
        {
            get { return (double?)GetValue(SelectedValueProperty); }
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
                    btnMinus.IsEnabled = value.Value != MinimumValue;
                    btnAdd.IsEnabled = value.Value != MaximumValue;
                    txt.Text = value.ToString();
                }
                SetValue(SelectedValueProperty, value);
                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
        }
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(double?), typeof(NumericUpDown), new FrameworkPropertyMetadata(new double?(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SelectedValuePropertyChanged)));

        private static void SelectedValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DoubleUpDown x = (DoubleUpDown)d;
            x.SelectedValue = (double?)e.NewValue;
        }

        public double MinimumValue
        {
            get { return (double)GetValue(MinimumValueProperty); }
            set
            {
                if (value < MinimumValue)
                    throw new Exception("MinimumValue Value Must be less than Maximum Value");
                SetValue(MinimumValueProperty, value);
            }
        }
        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register("MinimumValue", typeof(double), typeof(NumericUpDown), new PropertyMetadata(0));

        public double MaximumValue
        {
            get { return (double)GetValue(MaximumValueProperty); }
            set
            {
                if (value < MinimumValue)
                    throw new Exception("MaximumValue Value Must be more than Minimum Value");
                SetValue(MaximumValueProperty, value);
            }
        }
        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register("MaximumValue", typeof(double), typeof(NumericUpDown), new PropertyMetadata(100));


        private void RepeatButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            if (!SelectedValue.HasValue)
                SelectedValue = MinimumValue;
            else if (SelectedValue.Value > MaximumValue - 1)
                return;
            else SelectedValue = SelectedValue.Value + 1;
        }
        private void RepeatButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            if (!SelectedValue.HasValue)
                SelectedValue = MinimumValue;
            else if (SelectedValue.Value < MinimumValue + 1)
                return;
            else SelectedValue = SelectedValue.Value - 1;
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
                if (SelectedValue.HasValue)
                    SelectedValue = null;
            }
            else
            {
                try
                {
                    double num = double.Parse(txt.Text.Trim());
                    if (num > MaximumValue || num < MinimumValue)
                        SelectedValue = MinimumValue;
                    else SelectedValue = num;
                }
                catch
                {
                    SelectedValue = null;
                }
            }
        }

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            txt.SelectAll();
        }
    }
}