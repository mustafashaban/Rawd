﻿using System;
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
using System.Windows.Shapes;

namespace MainWPF
{
    public partial class StudentDetailsControl : UserControl
    {
        public StudentDetailsControl(Orphan o)
        {
            InitializeComponent();
            this.DataContext = o;
            FillData(o);
        }
        public StudentDetailsControl(Family f)
        {
            InitializeComponent();
            tcm.Items.RemoveAt(0);
            tcm.Items.RemoveAt(2);
            tcm.Items.RemoveAt(7);
            Orphan o = new Orphan();
            o.OrphanFamily = f;
            if (!f.FamilyID.HasValue)
            {
                f.ApplyDate = BaseDataBase.DateNow;
                f.OrphanNursemaid = new Guardian() { Gender = "أنثى" };
                f.OrphanGuardian = new Guardian() { Gender = "ذكر" };
                f.FamilyHouse = new House();
                cOrphanFamily.dgChild.ItemsSource = new List<FamilyPerson>();
                btnShowDetails_Click(null, null);
            }
            else
                FillData(o);
            this.DataContext = o;
        }
        async void FillData(Orphan o)
        {
            Family f = o.OrphanFamily;
            var hs = House.GetHouseAllByFamilyID(f.FamilyID);
            if (hs != null && hs.Count() > 0) f.FamilyHouse = hs.Last();
            else f.FamilyHouse = new House();

            if (f.FamilyID.HasValue)
                cOrphanFamily.dgChild.ItemsSource = (from x in FamilyPerson.GetFamilyPersonByFamilyID(f.FamilyID.Value) orderby x.DOB select x).ToList();
            else cOrphanFamily.dgChild.ItemsSource = new List<FamilyPerson>();

            cFamilyNeed.FamilyID = f.FamilyID;
            cExternalFamilySupport.FamilyID = f.FamilyID;
            cSpecialCard.FamilyID = f.FamilyID;
            cListerGroup.FamilyID = f.FamilyID;
            cTrainings.FamilyID = f.FamilyID;

            if (o.OrphanID.HasValue)
                cSponsor.OrphanID = o.OrphanID;
            cOrders.FamilyID = f.FamilyID;
            cOrphanFamily.txtFamilyCode.IsReadOnly = true;

            if (o.OrphanID.HasValue)
            {
                o.Account = Account.GetAccountByOwnerID(Account.AccountType.Student, o.OrphanID.Value);
                cAccount.Account = o.Account;
            }
            Guardian.GetAllGuardianByFamily(f);
            if (f.OrphanNursemaid == null)
                f.OrphanNursemaid = new Guardian() { Gender = "أنثى" };
            if (f.OrphanGuardian == null)
                f.OrphanGuardian = new Guardian() { Gender = "ذكر" };

            f.FamilyOrphans = await Orphan.GetAllOrphanByFamily(f, o, true);
            cOrphanFamily.Orphans = await Orphan.GetAllOrphanByFamily(f, o);
        }

        private void btnShowDetails_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanUpdateFamily)
            {
                MyMessageBox.Show("ليس لديك صلاحية اظهار تفاصيل عائلة");
                return;
            }
            var sb = cOrphanFamily.FindResource("sbShowDetails") as Storyboard;
            if (sb != null)
            {
                sb.Begin(cOrphanFamily);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            var f = cOrphanFamily.DataContext as Family;
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
                if (!(string.IsNullOrEmpty(f.OrphanGuardian.FirstName) && string.IsNullOrEmpty(f.OrphanGuardian.LastName)))
                {
                    if (!f.OrphanGuardian.IsValidate())
                        return;
                }
                if (!(string.IsNullOrEmpty(f.OrphanNursemaid.FirstName) && string.IsNullOrEmpty(f.OrphanNursemaid.LastName)))
                {
                    if (!f.OrphanNursemaid.IsValidate())
                        return;
                }
                if (!string.IsNullOrEmpty(f.FamilyHouse.OldAddress) || !string.IsNullOrEmpty(f.FamilyHouse.Address))
                {
                    if (!f.FamilyHouse.IsValidate())
                        return;
                }
                foreach (var fp in cOrphanFamily.dgChild.ItemsSource as List<FamilyPerson>)
                {
                    if (!fp.IsValidate())
                        return;
                }

                if (cOrphanFamily.Orphans == null || cOrphanFamily.Orphans.Count == 0)
                {
                    MyMessageBox.Show("يجب ادخال بيانات الايتام");
                    return;
                }

                if (!f.FamilyID.HasValue)
                {
                    if (!DBMain.InsertData(f))
                        return;
                    else
                    {
                        MyMessage.InsertMessage();
                        var tih = this.Parent as TabItem;
                        if (tih != null)
                            tih.Header = f.FamilyCode + " " + f.FamilyName;
                    }
                }
                else if (!DBMain.UpdateData(f))
                    return;
                else MyMessage.UpdateMessage();

                //Father
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
                //Mother
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
                //Guardian
                if (!(string.IsNullOrEmpty(f.OrphanGuardian.FirstName) && string.IsNullOrEmpty(f.OrphanGuardian.LastName)))
                {
                    f.OrphanGuardian.FamilyID = f.FamilyID;
                    if (f.OrphanGuardian.GuardianID.HasValue)
                        Guardian.UpdateData(f.OrphanGuardian);
                    else
                    {
                        Guardian.InsertData(f.OrphanGuardian);
                    }
                }
                else if (f.OrphanGuardian.GuardianID.HasValue)
                {
                    Guardian.DeleteData(f.OrphanGuardian);
                }
                //Nursemaid
                if (!(string.IsNullOrEmpty(f.OrphanNursemaid.FirstName) && string.IsNullOrEmpty(f.OrphanNursemaid.LastName)))
                {
                    f.OrphanNursemaid.FamilyID = f.FamilyID;
                    if (f.OrphanNursemaid.GuardianID.HasValue)
                        Guardian.UpdateData(f.OrphanNursemaid);
                    else
                    {
                        Guardian.InsertData(f.OrphanNursemaid);
                    }
                }
                else if (f.OrphanNursemaid.GuardianID.HasValue)
                {
                    Guardian.DeleteData(f.OrphanNursemaid);
                }

                f.FamilyHouse.FamilyID = f.FamilyID;
                if (!string.IsNullOrEmpty(f.FamilyHouse.OldAddress) || !string.IsNullOrEmpty(f.FamilyHouse.HouseSection))
                {
                    if (f.FamilyHouse.HouseID == null)
                        DBMain.InsertData(f.FamilyHouse);
                    else DBMain.UpdateData(f.FamilyHouse);
                }

                foreach (var fp in cOrphanFamily.dgChild.ItemsSource as List<FamilyPerson>)
                {
                    if (fp.FamilyPersonID.HasValue)
                        DBMain.UpdateData(fp);
                    else if (f.FamilyID.HasValue)
                    {
                        fp.FamilyID = f.FamilyID;
                        DBMain.InsertData(fp);
                    }
                }
                foreach (var o in cOrphanFamily.Orphans)
                {
                    if (o.OrphanID.HasValue)
                        Orphan.UpdateData(o);
                    else if (f.FamilyID.HasValue)
                    {
                        if (Orphan.InsertData(o))
                        {
                            o.Account = new Account();
                            o.Account.Name = o.FirstName + " " + o.LastName;
                            o.Account.Type = o.Type == "يتيم" ? Account.AccountType.Orphan : o.Type == "يتيم طالب علم" ? Account.AccountType.OrphanStudent : Account.AccountType.Student;
                            o.Account.CurrentBalance = 0;
                            o.Account.CreateDate = BaseDataBase.DateNow;
                            o.Account.OwnerID = o.OrphanID;
                            o.Account.Status = "مفعل";
                            o.Account.IsDebit = true;

                            Account.InsertData(o.Account);
                        }
                    }
                }
                cFamilyNeed.FamilyID = f.FamilyID;
                cExternalFamilySupport.FamilyID = f.FamilyID;
                cSpecialCard.FamilyID = f.FamilyID;
                cListerGroup.FamilyID = f.FamilyID;
                cOrders.FamilyID = f.FamilyID;

                f.UpdateFamilyPersonCount();
            }
        }


        private void btnWithdrawn_Click(object sender, RoutedEventArgs e)
        {
            var o = DataContext as Orphan;
            if (o != null && o.Account != null)
            {
                if (o.CurrentSponsorship == null)
                {
                    MyMessageBox.Show("الطالب الحالي ليس مكفولا بعد\nيجب اختيار كفالة للطالب أولا");
                    return;
                }
                var t = Transition.GetLastTransitionByAccount(o.Account);
                var diff = (BaseDataBase.DateNow - t.CreateDate.Value).Days;
                if (diff < 25)
                {
                    if (!BaseDataBase.CurrentUser.IsAdmin)
                    {
                        MyMessageBox.Show($"اخر دفعة تم تسليمها للطالب منذ {diff} يوم\nلا يمكن التسليم حاليا");
                        return;
                    }
                    else if (MyMessageBox.Show($"اخر دفعة تم تسليمها للطالب منذ {diff} يوم\nهل تريد المتابعة؟", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                        return;
                }
                Transition_StudentWindow w = new Transition_StudentWindow(o);
                if (w.ShowDialog() == true)
                {
                    if (o.OrphanID.HasValue)
                        o.Account = Account.GetAccountByOwnerID(Account.AccountType.Student, o.OrphanID.Value);
                }
            }
        }
    }
}
