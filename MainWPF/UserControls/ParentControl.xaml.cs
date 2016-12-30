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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for ParentControl.xaml
    /// </summary>
    public partial class ParentControl : UserControl
    {
        public ParentControl()
        {
            InitializeComponent();
            this.DataContextChanged += ParentControl_DataContextChanged;
        }

        void ParentControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Parent)
            {
                if ((e.NewValue as Parent).Gender == "ذكر")
                {
                    chkIsNursemaid.Visibility = txtIsNursemaid.Visibility = btnTraining.Visibility = System.Windows.Visibility.Collapsed;
                }

                if (((Parent)e.NewValue).ParentrID != null)
                {
                    btnUpdate.Visibility = System.Windows.Visibility.Visible;
                    btnExecute.Visibility = System.Windows.Visibility.Hidden;
                    BaseDataBase.MakeTabItemGreen(this.Parent as TabItem);
                }
                else
                {
                    btnUpdate.Visibility = System.Windows.Visibility.Hidden;
                    btnExecute.Visibility = System.Windows.Visibility.Visible;
                    BaseDataBase.MakeTabItemRed(this.Parent as TabItem);
                }
            }
        }
        private void btnQualification_Click(object sender, RoutedEventArgs e)
        {
            Parent p = (Parent)this.DataContext;
            if (p != null && p.ParentrID != null)
            {
                QualificationContainerWindow q = new QualificationContainerWindow((p.Gender == "ذكر") ? Qualification.QualificationPersonType.Father : Qualification.QualificationPersonType.Mother);
                q.PersonID = p.ParentrID;
                q.ShowDialog();
            }
            else MyMessageBox.Show("لم يتم إدخال بيانات الوالد بعد");
        }
        private void btnHealth_Click(object sender, RoutedEventArgs e)
        {
            //Parent p = (Parent)this.DataContext;
            //if (p != null && p.ParentrID != null)
            //{
            //    ParentHealthControl phc = new ParentHealthControl();
            //    phc.ParentID = p.ParentrID;
            //    phc.ShowDialog();
            //}
            //else MyMessageBox.Show("لم يتم إدخال بيانات الوالد بعد");

        }


        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            var x = (Parent)this.DataContext;
            if (x.IsValidate())
            {
                if (DBMain.InsertData(x))
                {
                    BaseDataBase.MakeTabItemGreen(this.Parent as TabItem);
                    MyMessage.InsertMessage();
                    btnExecute.Visibility = System.Windows.Visibility.Hidden;
                    btnUpdate.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var x = (Parent)this.DataContext;
            if (x.IsValidate())
            {
                if (DBMain.UpdateData(x))
                {
                    MyMessage.UpdateMessage();
                }
            }
        }

        private void btnTrainigDetails_Click(object sender, RoutedEventArgs e)
        {
            Parent p = this.DataContext as Parent;
            if (p.ParentrID != null)
            {
                //TrainingTrainedDetails w = new TrainingTrainedDetails(p.ParentrID, Trained.TrainedEnumType.والدة);
                //w.ShowDialog();
            }
            else MyMessageBox.Show("لم يتم إدخال بيانات الوالد بعد");
        }

        private void btnDelParent_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Parent;
            if (x.ParentrID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات\n\nملاحظة:\nسيتم حذف كل من: المؤهلات، الحالة الصحية، الدورات التدريبية والأخوة للوالد", MessageBoxButton.YesNo) == MessageBoxResult.Yes && DBMain.DeleteData(x))
                {
                    BaseDataBase.MakeTabItemRed(this.Parent as TabItem);
                    MyMessage.DeleteMessage();
                    Parent p;
                    if (x.Gender == "ذكر")
                        p = new Father();
                    else p = new Mother();
                    p.FamilyID = x.FamilyID;
                    this.DataContext = p;
                }
            }
        }
    }
}
