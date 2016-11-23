using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainWPF
{
    public class NextOrderCriteria : ModelViewContext
    {
        int? id;
        public int? ID
        {
            get { return id; }
            set { id = value; }
        }

        int? fromMember;
        public int? FromMember
        {
            get { return fromMember; }
            set { fromMember = value;
                NotifyPropertyChanged("FromMember");
            }
        }

        int? toMember;
        public int? ToMember
        {
            get { return toMember; }
            set { toMember = value; }
        }

        int? numberOfDays;
        public int? NumberOfDays
        {
            get { return numberOfDays; }
            set { numberOfDays = value; }
        }

        internal bool IsValidate()
        {
            bool isValid = true;
            this.ClearAllErrors();
            if (!NumberOfDays.HasValue || !FromMember.HasValue || !ToMember.HasValue)
            {
                isValid = false;
                this.SetError("FromMember", "يوجد قيم مفقودة - يجب ادخال كافة القيم");
            }
            else if (FromMember > ToMember)
            {
                isValid = false;
                this.SetError("FromMember", "خطأ في ادخال القيم - الحد الاعلى يجب ان يكون اكبر من الحد الادنى");
            }
            if (!isValid)
            {
                string s = "";
                foreach (var item in errors)
                {
                    s += item.Value + "\n";
                }
                MyMessageBox.Show(s);
            }
            return isValid;
        }
    }
}
