using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class StatisticsClass
    {
        public ObservableCollection<BaseCharClass> Items { get; private set; }
        public StatisticsClass(string Query)
        {
            Items = new ObservableCollection<BaseCharClass>();
            DataTable dt = BaseDataBase._Tabling(Query);
            foreach (DataRow row in dt.Rows)
            {
                Items.Add(new BaseCharClass() { Name = row["Name"].ToString(), Number = int.Parse(row["Number"].ToString()) });
            }
        }
        public StatisticsClass(DataTable dt)
        {
            Items = new ObservableCollection<BaseCharClass>();
            foreach (DataRow row in dt.Rows)
            {
                Items.Add(new BaseCharClass() { Name = row["Name"].ToString(), Number = int.Parse(row["Number"].ToString()) });
            }
        }
        public StatisticsClass(ObservableCollection<BaseCharClass> Items)
        {
            this.Items = Items;
        }

        private object selectedItem = null;
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                // selected item has changed
                selectedItem = value;
            }
        }
    }

    // class which represent a data point in the chart
    public class BaseCharClass
    {
        public string Name { get; set; }

        public int Number { get; set; }
    }
}
