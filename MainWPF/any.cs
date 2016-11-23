using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace MainWPF
{
    public class ParagraphInlineBehavior : DependencyObject
    {
        public static readonly DependencyProperty TemplateResourceNameProperty =
            DependencyProperty.RegisterAttached("TemplateResourceName",
                                                typeof(string),
                                                typeof(ParagraphInlineBehavior),
                                                new UIPropertyMetadata(null, OnParagraphInlineChanged));
        public static string GetTemplateResourceName(DependencyObject obj)
        {
            return (string)obj.GetValue(TemplateResourceNameProperty);
        }
        public static void SetTemplateResourceName(DependencyObject obj, string value)
        {
            obj.SetValue(TemplateResourceNameProperty, value);
        }

        public static readonly DependencyProperty ParagraphInlineSourceProperty =
            DependencyProperty.RegisterAttached("ParagraphInlineSource",
                                                typeof(IEnumerable),
                                                typeof(ParagraphInlineBehavior),
                                                new UIPropertyMetadata(null, OnParagraphInlineChanged));
        public static IEnumerable GetParagraphInlineSource(DependencyObject obj)
        {
            return (IEnumerable)obj.GetValue(ParagraphInlineSourceProperty);
        }
        public static void SetParagraphInlineSource(DependencyObject obj, IEnumerable value)
        {
            obj.SetValue(ParagraphInlineSourceProperty, value);
        }

        private static void OnParagraphInlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Table tb = d as Table;
            IEnumerable arr = ParagraphInlineBehavior.GetParagraphInlineSource(tb);
            string templateName = ParagraphInlineBehavior.GetTemplateResourceName(tb);
            if (arr != null && templateName != null)
            {
                //tb.Columns.Clear();
                //tb.RowGroups.Clear();
                foreach (var item in arr)
                {
                    ArrayList templateList = tb.FindResource(templateName) as ArrayList;
                    for (int i = 0; i < templateList.Count; i++)
                    {
                        if (i == 0)
                        {
                            tb.RowGroups.Add(templateList[i] as TableRowGroup);
                        }
                        else
                        {
                            var tr = (templateList[i] as TableRowGroup);
                            tr.DataContext = item;
                            tb.RowGroups.Add(tr);
                        }
                    }
                }
            }
        }
    }
}
