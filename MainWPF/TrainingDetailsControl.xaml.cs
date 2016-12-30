using System;
using System.Collections.Generic;
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
    public partial class TrainingDetailsControl : UserControl
    {
        public TrainingDetailsControl()
        {
            InitializeComponent();
        }

        public int? FamilyID
        {
            set
            {
                if (value.HasValue)
                    dgTrainingDetails.ItemsSource =
                        BaseDataBase._TablingStoredProcedure("sp_Get_All_Trainees_ByFamilyID", new System.Data.SqlClient.SqlParameter("@FamilyID", value)).DefaultView;
            }
        }
    }
}
