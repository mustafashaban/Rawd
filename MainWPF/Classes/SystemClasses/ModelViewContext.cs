using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class ModelViewContext : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                if (this.errors.ContainsKey(columnName))
                {
                    return this.errors[columnName];
                }
                return string.Empty;
            }
        }

        public Dictionary<string, string> errors = new Dictionary<string, string>();
        public void SetError(string propertyName, string errorMessage)
        {
            errors[propertyName] = errorMessage;
            NotifyPropertyChanged(propertyName);
        }
        public void ClearError(string propertyName)
        {
            errors.Remove(propertyName);
        }
        public void ClearAllErrors()
        {
            List<string> properties = new List<string>();
            foreach (KeyValuePair<string, string> error in this.errors)
            {
                properties.Add(error.Key);
            }
            this.errors.Clear();
            foreach (string property in properties)
            {
                this.NotifyPropertyChanged(property);
            }
        }
    }
}
