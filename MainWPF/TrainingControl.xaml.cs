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
    public partial class TrainingControl : UserControl
    {
        public TrainingControl(Training t)
        {
            InitializeComponent();
            this.DataContext = t;
        }

        private void HandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is DataGrid && !e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        private void dgTrainees_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                e.Cancel = true;
                return;
            }
            var t = DataContext as Training;
            var tt = e.Row.DataContext as Trained;
            if (tt != null && t != null)
            {
                if (!tt.IsValidate())
                    e.Cancel = true;
                else
                {
                    dgTrainees.SelectedCells.Clear();
                    var dgc = new DataGridCellInfo(dgTrainees.Items[dgTrainees.Items.Count - 1], dgTrainees.Columns[1]);
                    dgTrainees.SelectedCells.Add(dgc);
                    dgTrainees.CurrentCell = dgc;
                }
            }
        }

        private void btnDeleteTrained_Click(object sender, RoutedEventArgs e)
        {
            DeleteTrained_CommandExecuted(null, null);
        }

        private void DeleteTrained_CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var t = dgTrainees.SelectedCells[0].Item as Trained;
            if (t != null)
            {
                if (MyMessageBox.Show("هل تريد حذف السجل؟", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (t.ID.HasValue)
                        Trained.DeleteData(t);
                    t.Training.Trainees.Remove(t);
                    dgTrainees.Items.Refresh();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Training;
            if (x.IsValidate())
            {
                if (x.ID == null)
                {
                    if (Training.InsertData(x))
                    {
                        foreach (var t in x.Trainees)
                            Trained.InsertData(t);
                        MyMessage.InsertMessage();
                    }
                }
                else
                {
                    foreach (var t in x.Trainees)
                    {
                        if (t.ID.HasValue)
                            Trained.UpdateData(t);
                        else Trained.InsertData(t);
                    }
                    if (Training.UpdateData(x))
                        MyMessage.UpdateMessage();
                }
            }
        }

        private void btnAddTrained_Click(object sender, RoutedEventArgs e)
        {
            var tr = this.DataContext as Training;
            SelectTraineesControl w = new SelectTraineesControl(tr);
            if (w.ShowDialog() == true)
            {
                List<Trained> TraineesToRemove = new List<Trained>();
                foreach (var tt in tr.Trainees)
                {
                    if ((w.dgTrained.ItemsSource as List<Trained>).Where(x => x.TrainedID == tt.TrainedID && x.TrainedType == tt.TrainedType).FirstOrDefault() == null)
                        TraineesToRemove.Add(tt);
                }
                foreach (var tt in TraineesToRemove)
                {
                    if (tt.ID.HasValue)
                        Trained.DeleteData(tt);
                    tr.Trainees.Remove(tt);
                }

                foreach (var tt in w.dgTrained.ItemsSource as List<Trained>)
                {
                    if (tr.Trainees.Where(x => x.TrainedID == tt.TrainedID && x.TrainedType == tt.TrainedType).FirstOrDefault() == null)
                    {
                        tr.Trainees.Add(tt);

                        if (tr.ID.HasValue)
                            Trained.InsertData(tt);
                    }
                }
                dgTrainees.Items.Refresh();
            }
        }
    }
}
