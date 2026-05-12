using System.Diagnostics;
using System.Windows.Forms;

namespace adbGUI.Forms
{
    public class ExtForm : Form
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    Debug.WriteLine("Keypress detected. Closing ExtForm...");
                    Close();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
