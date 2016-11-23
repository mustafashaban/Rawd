using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for StudySideInsertControl.xaml
    /// </summary>
    public partial class StudySideInsertControl : Window
    {
        public StudySideInsertControl()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MyMessageBox.Show("يجب إدخال اسم الجهة المستخدمة أولاً");
            }
            else
            {
                BaseDataBase._StoredProcedure("sp_Add2MergedStudySide", new SqlParameter("@Name", txtName.Text.Trim()));
                this.DialogResult = true;
            }
        }
    }
}
