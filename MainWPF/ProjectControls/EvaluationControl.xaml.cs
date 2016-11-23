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
    /// Interaction logic for EvaluationControl.xaml
    /// </summary>
    public partial class EvaluationControl : UserControl
    {
        public EvaluationControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty;
        static EvaluationControl()
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata(new int?(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(XXXX));
            EvaluationControl.ValueProperty = DependencyProperty.Register("Value", typeof(int?), typeof(EvaluationControl), metadata);
        }

        private static void XXXX(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EvaluationControl x = (EvaluationControl)d;
            x.Value = (int?)e.NewValue;
        }
        
        public int? Value
        {
            set
            {
                SetValue(ValueProperty, value);
                if (CanDoChanged) ValueChanged();
            }
            get
            {
                return (int?)GetValue(EvaluationControl.ValueProperty);
            }
        }

        static bool CanDoChanged = true;
        void ValueChanged()
        {
            CanDoChanged = false;
            switch (Value)
            {
                case 1:
                    rad1.IsChecked = true;
                    break;
                case 2:
                    rad2.IsChecked = true;
                    break;
                case 3:
                    rad3.IsChecked = true;
                    break;
                case 4:
                    rad4.IsChecked = true;
                    break;
                default:
                    rad1.IsChecked = rad2.IsChecked = rad3.IsChecked = rad4.IsChecked = false;
                    break;
            }
            CanDoChanged = true;
        }

        private void rad_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (CanDoChanged)
            {
                CanDoChanged = false;

                if (rad1.IsChecked == true) Value = 1;
                else if (rad2.IsChecked == true) Value = 2;
                else if (rad3.IsChecked == true) Value = 3;
                else if (rad4.IsChecked == true) Value = 4;
                else Value = null;

                CanDoChanged = true;
            }
        }



        public enum ModeEnumeration { A, B, C };
        private ModeEnumeration? mode;
        public ModeEnumeration? Mode
        {
            get { return mode; }
            set { 
                mode = value;
                ModeChanged();
            }
        }
        void ModeChanged()
        {
            if (mode == ModeEnumeration.B)
            {
                rad1.Content = "كبير";
                rad2.Content = "متوسط";
                rad3.Content = "صغير";
            }
            else if (mode == ModeEnumeration.A)
            {
                rad1.Content = "جيد";
                rad2.Content = "متوسط";
                rad3.Content = "سيء";
            }
        }

    }
}
