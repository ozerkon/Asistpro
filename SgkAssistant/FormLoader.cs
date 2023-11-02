using System.Windows.Forms;

namespace SgkAssistant.Forms
{
    public static class FormLoader
    {
        private static Form _mdiMainForm;

        public static Form MdiMainForm
        {
            get { return FormLoader._mdiMainForm; }
            set { FormLoader._mdiMainForm = value; }
        }

        public static DialogResult ShowMenuForm(Form f, bool isModal, bool isSingleInstance)
        {
            if (Application.OpenForms.Count > 1) { return DialogResult.None; }
            DialogResult result = DialogResult.None;
            if (isModal)
            {
                result = f.ShowDialog();
            }
            else
            {
                if (isSingleInstance)
                {
                    if (Program.IsFormOpen(f)) { return result; }
                }
                f.MdiParent = _mdiMainForm;
                f.Show();
            }
            return result;
        }
        public static DialogResult ShowForm(Form f, bool isModal, bool isSingleInstance)
        {
            // if (Application.OpenForms.Count > 1) { return DialogResult.None; }
            DialogResult result = DialogResult.None;
            if (isModal)
            {
                result = f.ShowDialog();
            }
            else
            {
                if (isSingleInstance)
                {
                    if (Program.IsFormOpen(f)) { return result; }
                }
                f.MdiParent = _mdiMainForm;
                f.Show();
            }
            return result;
        }
    }
}
