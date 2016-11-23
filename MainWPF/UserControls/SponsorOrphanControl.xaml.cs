﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Interaction logic for SupervisorOrphanControl.xaml
    /// </summary>
    public partial class SponsorOrphanControl : UserControl, INotifyPropertyChanged
    {
        public SponsorOrphanControl()
        {
            InitializeComponent();
            cmboSoonsorType.ItemsSource = Sponsor_Orphan.GetAllSponsorTypes;
        }

        int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;
                OrphanSponsors = Sponsor_Orphan.GetSponsorAllByOrphanID(value);
            }
        }

        int place = 0;
        public int Place
        {
            get { return place; }
            set
            {
                if (value >= 0 && value <= TotalCount)
                {
                    place = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Place"));
                    if (value == 0)
                    {
                        btnBack.IsEnabled = btnNext.IsEnabled = false;
                    }
                    else
                    {
                        btnBack.IsEnabled = (value == 1) ? false : true;
                        btnNext.IsEnabled = (value == TotalCount) ? false : true;
                        this.DataContext = OrphanSponsors[Place - 1];
                    }
                }
            }
        }

        int totalCount = 0;
        public int TotalCount
        {
            get { return totalCount; }
            set
            {
                if (value == 0)
                    BaseDataBase.MakeTabItemRed(this.Parent as TabItem);
                else
                    BaseDataBase.MakeTabItemGreen(this.Parent as TabItem);
                totalCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCount"));
            }
        }

        List<Sponsor_Orphan> orphanSponsors;
        public List<Sponsor_Orphan> OrphanSponsors
        {
            get { return orphanSponsors; }
            set
            {
                orphanSponsors = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    Sponsor_Orphan so = new Sponsor_Orphan();
                    so.OrphanID = OrphanID;
                    this.DataContext = so;
                }
                else
                {
                    Place = 1;
                }
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Place++;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Place--;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Sponsor_Orphan;
            if (x.IsValidate())
            {
                if (x.OrphanID == null)
                {
                    MyMessageBox.Show("لم يتم إدخال بيانات اليتيم");
                    return;
                }
                if (x.ID != null)
                {
                    if (!BaseDataBase.CurrentUser.CanUpdate)
                        MyMessageBox.Show("لا يوجد لديك صلاحيات للتعديل");
                    else if (x.UpdateData())
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (x.InsertData())
                    {
                        MyMessage.InsertMessage();
                        OrphanSponsors.Add(x);
                        TotalCount = OrphanSponsors.Count;
                        Place = TotalCount;
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Sponsor_Orphan;
            if (x.ID != null)
            {
                var os = new Sponsor_Orphan();
                os.OrphanID = OrphanID;
                this.DataContext = os;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Sponsor_Orphan;
            if (x.ID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && x.DeleteData())
                {
                    MyMessage.DeleteMessage();
                    OrphanID = OrphanID;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var w = new SponsorSelectControl();
            if (w.ShowDialog() == true)
            {
                (this.DataContext as Sponsor_Orphan).SponsorID = (w.dgSupervisor.SelectedItem as Sponsor).SponsorID;
            }
        }

    }
}
