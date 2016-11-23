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
    public partial class OrphanDetailsControl : UserControl
    {
        public OrphanDetailsControl(int? OrphanID)
        {
            InitializeComponent();
            cOrhans.OrphanID = OrphanID;
            cAdminEvaluation.OrphanID = OrphanID;
            cSupervisor.OrphanID = OrphanID;
            cSponsor.OrphanID = OrphanID;
            cContinous.OrphanID = OrphanID;
            cTraining.OrphanID = OrphanID;
            //cIntrnalSuppot.OrphanID = OrphanID;

            if (!(cOrhans.DataContext as Orphan).IsDetached)
            {
                tcm.Items.RemoveAt(10);
                tcm.Items.RemoveAt(5);
                tcm.Items.RemoveAt(4);
                tcm.Items.RemoveAt(3);
                tcm.Items.RemoveAt(2);
                tcm.Items.RemoveAt(1);
            }
            else
            {
                cHouse.OrphanID = OrphanID;
                cListerGroup.OrphanID = OrphanID;
                cGuardian.OrphanID = OrphanID;
                cNursemaid.OrphanID = OrphanID;
            }
        }
    }
}
