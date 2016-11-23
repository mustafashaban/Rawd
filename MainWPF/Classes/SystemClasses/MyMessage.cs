using System;
using System.Threading;
using System.Windows.Threading;
namespace MainWPF
{
    public class MyMessage
    {
        public static void InsertMessage()
        {
            MessageWindow x = new MessageWindow();
            x.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            x.Top = App.Current.MainWindow.ActualHeight - x.Height - 60;
            x.Left = App.Current.MainWindow.ActualWidth - x.Width;
            x.Show();
        }
        public static void UpdateMessage()
        {
            MessageWindow x = new MessageWindow();
            x.txt.Text = "تم التعديل بنجاح";
            x.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            x.Top = App.Current.MainWindow.ActualHeight - x.Height - 60;
            x.Left = App.Current.MainWindow.ActualWidth - x.Width;
            x.Show();
        }
        public static void DeleteMessage()
        {
            MessageWindow x = new MessageWindow();
            x.txt.Text = "تم الحذف بنجاح";
            x.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            x.Top = App.Current.MainWindow.ActualHeight - x.Height - 60;
            x.Left = App.Current.MainWindow.ActualWidth - x.Width;
            x.Show();
        }
        public static void CustomMessage( string Message)
        {
            MessageWindow x = new MessageWindow();
            x.txt.Text = Message;
            x.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            x.Top = App.Current.MainWindow.ActualHeight - x.Height - 60;
            x.Left = App.Current.MainWindow.ActualWidth - x.Width;
            x.Show();
        }


        void Run()
        {
 
        }
    }
}
