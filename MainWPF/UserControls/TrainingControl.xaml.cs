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
    /// Interaction logic for TrainingControl.xaml
    /// </summary>
    public partial class TrainingControl : Window
    {
        public TrainingControl()
        {
            InitializeComponent();
            this.DataContext = new Training();
        }
        public TrainingControl(Training t)
        {
            InitializeComponent();
            this.DataContext = t;
            btnExecute.Content = "تعديل";
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            var x = (Training)this.DataContext;
            if (x.IsValidate())
            {
                if (x.TrainingID == null)
                {
                    if (x.InsertTrainingData())
                    {
                        MyMessage.InsertMessage();
                        DialogResult = true;
                    }
                }
                else
                {
                    if (x.UpdateTrainingData())
                    {
                        MyMessage.UpdateMessage();
                        DialogResult = true;
                    }
                }
            }
        }
    }
}
