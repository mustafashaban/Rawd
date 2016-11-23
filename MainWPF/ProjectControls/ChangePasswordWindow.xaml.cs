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
using System.Windows.Shapes;

namespace MainWPF
{
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(pwOld.Password) || string.IsNullOrWhiteSpace(pwNew1.Password)|| string.IsNullOrWhiteSpace(pwNew2.Password))
            {
                MyMessageBox.Show("يجب ادخال كامل الحقول");
                return;
            }
            if (!User.VerifyHash(pwOld.Password, "SHA512", BaseDataBase.CurrentUser.Password))
            {
                MyMessageBox.Show("كلمة المرور الحالية غير صحيحة");
                return;
            }
            if (pwNew1.Password != pwNew2.Password)
            {
                MyMessageBox.Show("كلمة المرور الجديدة غير متطابقة");
                return;
            }

            BaseDataBase.CurrentUser.Password = pwNew1.Password;
            if (User.UpdateData(BaseDataBase.CurrentUser))
            {
                MyMessage.UpdateMessage();
                DialogResult = true;
            }
        }
    }
}
