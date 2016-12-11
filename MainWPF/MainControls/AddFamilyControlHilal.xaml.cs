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
    public partial class AddFamilyControlHilal : UserControl
    {
        TempFamily tf;
        bool IsLoaded = false;
        public AddFamilyControlHilal()
        {
            InitializeComponent();
            DisableTabs();

            Family f = new Family();
            f.FamilyType = "نازحين";
            f.ApplyDate = BaseDataBase.DateNow;
            cFamily.DataContext = f;

            f.FamilyHouse = new House();
            cFamily.dgChild.ItemsSource = new List<FamilyPerson>();

            var sb = cFamily.FindResource("sbShowDetails") as Storyboard;
            if (sb != null)
                sb.Begin(cFamily);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
        public AddFamilyControlHilal(int FamilyID)
        {
            InitializeComponent();
            var f = Family.GetFamilyByID(FamilyID);
            cFamily.DataContext = f;
            FillData(f);
            btnUpdate.Visibility = System.Windows.Visibility.Visible;
            btnExecute.Visibility = System.Windows.Visibility.Collapsed;
            if (BaseDataBase.CurrentUser.IsAdmin)
                btnUserDetails.Visibility = System.Windows.Visibility.Visible;

        }

        async void FillData(Family f)
        {
            var hs = House.GetHouseAllByFamilyID(f.FamilyID);
            if (hs != null && hs.Count() > 0) f.FamilyHouse = hs.Last();
            else f.FamilyHouse = new House();

            if (f.FamilyID.HasValue)
                cFamily.dgChild.ItemsSource = (from x in FamilyPerson.GetFamilyPersonByFamilyID(f.FamilyID.Value) orderby x.DOB select x).ToList();
            else cFamily.dgChild.ItemsSource = new List<FamilyPerson>();

            cFamilyNeed.FamilyID = f.FamilyID;
            cExternalFamilySupport.FamilyID = f.FamilyID;
            //cFamilyProperty.FamilyID = FamilyID;
            cSpecialCard.FamilyID = f.FamilyID;
            cListerGroup.FamilyID = f.FamilyID;
            cAdminEvaluation.FamilyID = f.FamilyID;
            cOrders.FamilyID = f.FamilyID;
            cFamily.txtFamilyCode.IsReadOnly = true;
            var lst = cOrders?.lvOrders?.ItemsSource as List<Order>;
            if (lst != null && lst.Count > 0 && lst.Where(x => x.NextOrderDate.HasValue).Count() > 0)
            {
                var MaxNextOrderDate = lst.Where(x => x.NextOrderDate.HasValue).Select(x => x.NextOrderDate.Value).Max();
                if (MaxNextOrderDate > BaseDataBase.DateNow)
                    MyMessageBox.Show("متبقي " + (MaxNextOrderDate - BaseDataBase.DateNow).Days + " ايام للاستلام القادم\nتاريخ الاستلام القادم " + MaxNextOrderDate.ToShortDateString());
            }
            await Task.Delay(10);
        }


        public AddFamilyControlHilal(TempFamily tf)
        {
            InitializeComponent();
            DisableTabs();
            this.tf = tf;

            //Family
            Family f = new Family();
            //f.FamilyCode = tf.FamilyCode;
            f.FamilyName = tf.FamilyName;
            f.FamilyType = tf.FamilyType;
            f.ApplyDate = tf.ApplyDate;
            f.Notes = tf.Notes;
            f.FamilyPersonCount = tf.FamilyPersonCount.ToString();
            f.FamilyIdentityID = tf.FamilyIdentityID;
            f.DefinedPersonPhone = tf.Phone;
            cFamily.DataContext = f;

            //Father
            f.FamilyFather.FirstName = tf.FatherFirstName;
            f.FamilyFather.FatherName = tf.FatherFatherName;
            f.FamilyFather.LastName = tf.FatherLastName;
            f.FamilyFather.DOB = tf.FatherDOB;
            f.FamilyFather.BirthPlace = tf.FatherBornPlace;
            f.FamilyFather.PID = tf.FatherPID;
            f.FamilyFather.Phone = tf.Phone;
            f.FamilyFather.Mobile = tf.Mobile1;

            //Mother
            f.FamilyMother.FirstName = tf.MotherFirstName;
            f.FamilyMother.FatherName = tf.MotherFatherName;
            f.FamilyMother.LastName = tf.MotherLastName;
            f.FamilyMother.DOB = tf.MotherDOB;
            f.FamilyMother.BirthPlace = tf.MotherBornPlace;
            f.FamilyMother.PID = tf.MotherPID;
            f.FamilyMother.Phone = tf.Phone;
            f.FamilyMother.Mobile = tf.Mobile2;

            //House
            House h = new House();
            h.Phone = tf.Phone;
            h.HouseSection = tf.HouseSection;
            h.HouseStreet = tf.HouseStreet;
            h.Address = tf.HouseAddress;
            h.OldAddress = tf.HouseOldAddress;
            h.HouseBuildingNumber = tf.HouseBuildingNumber;
            h.HouseFloor = tf.HouseFloor;
            f.FamilyHouse = h;

            List<FamilyPerson> chlds = new List<FamilyPerson>();
            foreach (var c in tf.TempChilds)
                chlds.Add(new FamilyPerson() { FirstName = c.Name, DOB = c.DOB, Gender = c.Gender, RelationShip = c.Gender == "ذكر" ? "ابن" : "ابنة" });
            cFamily.dgChild.ItemsSource = (from x in chlds orderby x.DOB select x).ToList();

            cFamily.UserControl_Loaded(null, null);

            var sb = cFamily.FindResource("sbShowDetails") as Storyboard;
            if (sb != null)
                sb.Begin(cFamily);
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            var f = cFamily.DataContext as Family;
            if (f.IsValidate())
            {
                if (!(string.IsNullOrEmpty(f.FamilyFather.FirstName) && string.IsNullOrEmpty(f.FamilyFather.LastName)))
                {
                    if (!f.FamilyFather.IsValidate())
                        return;
                }
                if (!(string.IsNullOrEmpty(f.FamilyMother.FirstName) && string.IsNullOrEmpty(f.FamilyMother.LastName)))
                {
                    if (!f.FamilyMother.IsValidate())
                        return;
                }
                if (!string.IsNullOrEmpty(f.FamilyHouse.OldAddress) || !string.IsNullOrEmpty(f.FamilyHouse.Address))
                {
                    if (!f.FamilyHouse.IsValidate())
                        return;
                }
                foreach (var fp in cFamily.dgChild.ItemsSource as List<FamilyPerson>)
                {
                    if (!fp.IsValidate())
                        return;
                }

                if (DBMain.InsertData(f))
                {
                    var tih = this.Parent as TabItem;
                    if (tih != null)
                        tih.Header = f.FamilyCode + " " + f.FamilyName;

                    if (tf != null)
                    {
                        tf.FamilyID = f.FamilyID;
                        TempFamily.UpadteData(tf);
                    }
                    if (!(string.IsNullOrEmpty(f.FamilyFather.FirstName) && string.IsNullOrEmpty(f.FamilyFather.LastName)))
                    {
                        DBMain.InsertData(f.FamilyFather);
                    }

                    if (!(string.IsNullOrEmpty(f.FamilyMother.FirstName) && string.IsNullOrEmpty(f.FamilyMother.LastName)))
                    {
                        DBMain.InsertData(f.FamilyMother);
                    }

                    f.FamilyHouse.FamilyID = f.FamilyID;
                    if (!string.IsNullOrEmpty(f.FamilyHouse.OldAddress) || !string.IsNullOrEmpty(f.FamilyHouse.HouseSection))
                        DBMain.InsertData(f.FamilyHouse);

                    foreach (var fp in cFamily.dgChild.ItemsSource as List<FamilyPerson>)
                    {
                        if (f.FamilyID.HasValue && !fp.FamilyPersonID.HasValue)
                        {
                            fp.FamilyID = f.FamilyID;
                            DBMain.InsertData(fp);
                        }
                    }

                    btnUpdate.Visibility = System.Windows.Visibility.Visible;
                    btnExecute.Visibility = System.Windows.Visibility.Collapsed;

                    cFamilyNeed.FamilyID = f.FamilyID;
                    cExternalFamilySupport.FamilyID = f.FamilyID;
                    cSpecialCard.FamilyID = f.FamilyID;
                    cListerGroup.FamilyID = f.FamilyID;
                    cAdminEvaluation.FamilyID = f.FamilyID;
                    cOrders.FamilyID = f.FamilyID;

                    f.UpdateFamilyPersonCount();
                    EnableTabs();
                    MyMessage.InsertMessage();
                }
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanUpdateFamily)
            {
                MyMessageBox.Show("ليس لديك صلاحية تعديل بيانات عائلة");
                return;
            }
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            var f = cFamily.DataContext as Family;
            if (f.IsValidate())
            {
                if (!(string.IsNullOrEmpty(f.FamilyFather.FirstName) && string.IsNullOrEmpty(f.FamilyFather.LastName)))
                {
                    if (!f.FamilyFather.IsValidate())
                        return;
                }
                if (!(string.IsNullOrEmpty(f.FamilyMother.FirstName) && string.IsNullOrEmpty(f.FamilyMother.LastName)))
                {
                    if (!f.FamilyMother.IsValidate())
                        return;
                }
                if (!string.IsNullOrEmpty(f.FamilyHouse.OldAddress) || !string.IsNullOrEmpty(f.FamilyHouse.Address))
                {
                    if (!f.FamilyHouse.IsValidate())
                        return;
                }
                foreach (var fp in cFamily.dgChild.ItemsSource as List<FamilyPerson>)
                {
                    if (!fp.IsValidate())
                        return;
                }

                if (DBMain.UpdateData(f))
                {
                    if (!(string.IsNullOrEmpty(f.FamilyFather.FirstName) && string.IsNullOrEmpty(f.FamilyFather.LastName)))
                    {
                        if (f.FamilyFather.ParentrID.HasValue)
                            DBMain.UpdateData(f.FamilyFather);
                        else
                        {
                            DBMain.InsertData(f.FamilyFather);
                        }
                    }
                    else if (f.FamilyFather.ParentrID.HasValue)
                    {
                        DBMain.DeleteData(f.FamilyFather);
                    }

                    if (!(string.IsNullOrEmpty(f.FamilyMother.FirstName) && string.IsNullOrEmpty(f.FamilyMother.LastName)))
                    {
                        if (f.FamilyMother.ParentrID.HasValue)
                            DBMain.UpdateData(f.FamilyMother);
                        else
                        {
                            DBMain.InsertData(f.FamilyMother);
                        }
                    }
                    else if (f.FamilyMother.ParentrID.HasValue)
                    {
                        DBMain.DeleteData(f.FamilyMother);
                    }

                    f.FamilyHouse.FamilyID = f.FamilyID;
                    if (!string.IsNullOrEmpty(f.FamilyHouse.OldAddress) || !string.IsNullOrEmpty(f.FamilyHouse.HouseSection))
                    {
                        if (f.FamilyHouse.HouseID == null)
                            DBMain.InsertData(f.FamilyHouse);
                        else DBMain.UpdateData(f.FamilyHouse);
                    }

                    foreach (var fp in cFamily.dgChild.ItemsSource as List<FamilyPerson>)
                    {
                        if (!fp.FamilyPersonID.HasValue)
                        {
                            fp.FamilyID = f.FamilyID;
                            DBMain.InsertData(fp);
                        }
                        else
                        {
                            DBMain.UpdateData(fp);
                        }
                    }
                    f.UpdateFamilyPersonCount();
                    MyMessage.UpdateMessage();
                }
            }
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(cFamily.DataContext as Family).FamilyID.HasValue)
            {
                btnExecute_Click(null, null);
            }
            else
            {
                btnUpdate_Click(null, null);
            }
        }
        private void NewCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            btnNew_Click(null, null);
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanEnterFamily)
            {
                MyMessageBox.Show("ليس لديك صلاحية لاضافة عائلة جديدة");
            }
            else
            {
                var tih = this.Parent as TabItem;
                if (tih != null)
                    tih.Header = "إضافة عائلة";

                Family f = new Family();
                f.FamilyType = "نازحين";
                f.ApplyDate = BaseDataBase.DateNow;
                cFamily.DataContext = null;
                cFamily.DataContext = f;
                f.FamilyHouse = new House();
                cFamily.dgChild.ItemsSource = null;
                cFamily.dgChild.ItemsSource = new List<FamilyPerson>();
                btnExecute.Visibility = System.Windows.Visibility.Visible;
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;

                DisableTabs();
                cFamily.txtFamilyCode.IsReadOnly = false;
                cFamily.cmboSector.IsEnabled = true;
                cFamily.cmboPlace_SelectionChanged(null, null);
                Keyboard.Focus(cFamily.cmboFamilyType);
            }
        }

        void DisableTabs()
        {
            for (int i = 1; i < tcm.Items.Count; i++)
            {
                (tcm.Items[i] as TabItem).IsEnabled = false;
            }
        }
        void EnableTabs()
        {
            for (int i = 1; i < tcm.Items.Count; i++)
            {
                (tcm.Items[i] as TabItem).IsEnabled = true;
            }
        }

        private void btnUserDetails_Click(object sender, RoutedEventArgs e)
        {
            var f = cFamily.DataContext as Family;
            if (f != null)
                MyMessageBox.Show(string.Format("مدخل البيانات : {0}\nتاريخ ادخال البيانات : {1}\nآخر مدخل بيانات : {2}\nتاريخ آخر ادخال : {3}", f.CreatePerson, f.CreateDate.HasValue ? f.CreateDate.Value.ToString("dd-MM-yyy") : "", f.ModifiedPerson, f.LastModifiedDate.HasValue ? f.LastModifiedDate.Value.ToString("dd-MM-yyy") : ""));
        }

        private void btnShowDetails_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanUpdateFamily)
            {
                MyMessageBox.Show("ليس لديك صلاحية اظهار تفاصيل عائلة");
                return;
            }
            var sb = cFamily.FindResource("sbShowDetails") as Storyboard;
            if (sb != null)
            {
                sb.Begin(cFamily);
            }
        }


    }
}
