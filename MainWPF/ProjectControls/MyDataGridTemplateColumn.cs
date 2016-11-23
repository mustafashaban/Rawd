using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MainWPF
{
    public class MyDataGridTemplateColumn : DataGridTemplateColumn
    {
        protected override object PrepareCellForEdit(FrameworkElement editingElement,
                                                 RoutedEventArgs editingEventArgs)
        {
            editingElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            return base.PrepareCellForEdit(editingElement, editingEventArgs);
        } 
    }
}
