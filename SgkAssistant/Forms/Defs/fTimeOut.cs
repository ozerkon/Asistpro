using System;
using System.Windows.Forms;

namespace SgkAssistant.Forms.Defs
{
    public partial class FTimeOut : Telerik.WinControls.UI.RadForm
    {
        int decision = 0;
        public FTimeOut(string ques, string desc)
        {
            InitializeComponent();
            lblQuestion.Text = ques;
            lblDesc.Text = desc;
        }

        private void Radio_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (sender == rbUnpaid) decision = 0;
            else if (sender == rbAdvance) decision = 1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(decision == 0) DialogResult = DialogResult.Yes;
            else if(decision == 1) DialogResult = DialogResult.No;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
