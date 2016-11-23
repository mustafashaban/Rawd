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
    /// Interaction logic for AddFamilyControl.xaml
    /// </summary>
    public partial class AddFamilyControl : UserControl
    {
        TempFamily tf;
        List<string> ftList = new List<string>() { "أيتام"};
        public AddFamilyControl()
        {
            InitializeComponent();
            Family f = new Family();
            f.ApplyDate = BaseDataBase.DateNow;
            cFamily.DataContext = f;
            for (int i = 1; i < tcm.Items.Count; i++)
            {
                var x = tcm.Items[i] as TabItem;
                x.IsEnabled = false;
            }
        }
        public AddFamilyControl(int FamilyID)
        {
            InitializeComponent();
            cFamily.cmboFamilyType.IsEnabled = false;

            Family f = Family.GetFamilyByID(FamilyID);

            btnUpdate.Visibility = System.Windows.Visibility.Visible;
            btnExecute.Visibility = System.Windows.Visibility.Hidden;


            if (!ftList.Contains(f.FamilyType))
            {
                (tcm.Items[4] as TabItem).Header = "أفراد الأسرة";
                tcm.Items.RemoveAt(11);
                tcm.Items.RemoveAt(10);
                tcm.Items.RemoveAt(9);
            }
            else
            {
                cOrhans.FamilyID = FamilyID;
                cNursemaid.FamilyID = FamilyID;
                cGuardian.FamilyID = FamilyID;
            }

            cFamily.DataContext = f;
            cFamilyNeed.FamilyID = FamilyID;
            cExternalFamilySupport.FamilyID = FamilyID;
            cFamilyPersonControl.FamilyID = FamilyID;
            cFamilyProperty.FamilyID = FamilyID;
            cHouse.FamilyID = FamilyID;
            cListerGroup.FamilyID = FamilyID;
            cAdminEvaluation.FamilyID = FamilyID;
            //cIntrnalSuppot.FamilyID = FamilyID;
            cParentMother.DataContext = f.FamilyMother;
            cParentFather.DataContext = f.FamilyFather;
        }
        public AddFamilyControl(TempFamily tf)
        {
            InitializeComponent();
            this.tf = tf;

            for (int i = 1; i < tcm.Items.Count; i++)
            {
                var x = tcm.Items[i] as TabItem;
                x.IsEnabled = false;
            }
            
            //Family
            Family f = new Family();
            f.FamilyCode = tf.FamilyCode;
            f.FamilyName = tf.FamilyName;
            f.FamilyType = tf.FamilyType;
            f.ApplyDate = tf.ApplyDate;
            f.Notes = tf.Notes;
            f.FamilyPersonCount = tf.FamilyPersonCount.ToString();
            f.FamilyIdentityID = tf.FamilyIdentityID;
            f.DefinedPersonPhone = tf.Phone;
            cFamily.DataContext = f;
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            var x = cFamily.DataContext as Family;
            if (x.IsValidate())
            {
                if (DBMain.InsertData(x))
                {
                    var tih = this.Parent as TabItem;
                    if (tih != null)
                        tih.Header = x.FamilyCode + " " + x.FamilyName;

                    if (tf != null)
                    {
                        tf.FamilyID = x.FamilyID;
                        TempFamily.UpadteData(tf);
                    }
                    btnUpdate.Visibility = System.Windows.Visibility.Visible;
                    btnExecute.Visibility = System.Windows.Visibility.Hidden;

                    if (x.FamilyFather != null)
                        x.FamilyFather = new Father() { Gender = "ذكر" };
                    x.FamilyFather.FamilyID = x.FamilyID;
                    cParentFather.DataContext = x.FamilyFather;
                    if (x.FamilyMother != null)
                        x.FamilyMother = new Mother();
                    x.FamilyMother.FamilyID = x.FamilyID;
                    cParentMother.DataContext = x.FamilyMother;

                    House h = new House();
                    cHouse.FamilyID = x.FamilyID;
                    h.FamilyID = x.FamilyID;
                    cHouse.DataContext = h;

                    MyMessage.InsertMessage();

                    for (int i = 1; i < tcm.Items.Count; i++)
                    {
                        var ti = tcm.Items[i] as TabItem;
                        ti.IsEnabled = true;
                    }
                    if (!ftList.Contains(x.FamilyType))
                    {
                        (tcm.Items[4] as TabItem).Header = "أفرا د الأسرة";
                        tcm.Items.RemoveAt(11);
                        tcm.Items.RemoveAt(10);
                        tcm.Items.RemoveAt(9);
                    }
                    else
                    {
                        cOrhans.FamilyID = x.FamilyID;
                        cNursemaid.FamilyID = x.FamilyID;
                        cGuardian.FamilyID = x.FamilyID;
                    }
                }
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var x = cFamily.DataContext as Family;
            if (x.IsValidate())
            {
                if (DBMain.UpdateData(x))
                    MyMessage.UpdateMessage();
                if (x.FamilyType == "أيتام")
                {
                    (tcm.Items[4] as TabItem).Header = "أفراد الأسرة";
                }
            }
        }
    }
}
