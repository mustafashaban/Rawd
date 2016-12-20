using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class OrphanControl : UserControl
    {
        public OrphanControl()
        {
            InitializeComponent();
        }

        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Orphan;
            if (x.IsValidate())
            {
                if (x.OrphanFamily.FamilyID == null)
                {
                    MyMessageBox.Show("لم يتم إدخال بيانات العائلة");
                    return;
                }

                if (x.OrphanFamily.FamilyType == "أيتام")
                {
                    if (x.Gender == "ذكر" && (((BaseDataBase.DateNow - x.DOB.Value)).Days / 30 / 12) >= 14)
                    {
                        if (MyMessageBox.Show("عمر اليتيم أكبر من 14\nهل تريد المتابعة", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                            return;
                    }
                }

                if (x.OrphanID != null)
                {
                    if (Orphan.UpdateData(x))
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (Orphan.InsertData(x))
                        MyMessage.InsertMessage();
                }
            }
        }

        private void btnOrphanHealth_Click(object sender, RoutedEventArgs e)
        {
            Orphan o = this.DataContext as Orphan;
            if (o != null && o.OrphanID != null)
            {
                OrphanHealthControl w = new OrphanHealthControl();
                w.OrphanID = o.OrphanID;
                w.ShowDialog();
            }
            else MyMessageBox.Show("لم يتم إدخال بيانات اليتيم");
        }

        private void btnOrphanImpeding_Click(object sender, RoutedEventArgs e)
        {
            Orphan o = this.DataContext as Orphan;
            OrphanImpedingControl w = new OrphanImpedingControl();
            w.OrphanID = o.OrphanID;
            w.ShowDialog();
        }

        private void btnOrphanTeachingStage_Click(object sender, RoutedEventArgs e)
        {
            Orphan o = this.DataContext as Orphan;
            TeachingStageControl w = new TeachingStageControl();
            w.OrphanID = (this.DataContext as Orphan).OrphanID;
            w.ShowDialog();
        }

        private void btnOrphanQualification_Click(object sender, RoutedEventArgs e)
        {
            var o = (Orphan)this.DataContext;
            QualificationContainerWindow w = new QualificationContainerWindow(Qualification.QualificationPersonType.Orphan);
            w.PersonID = o.OrphanID;
            w.ShowDialog();
        }

        private void btnRemoveImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
