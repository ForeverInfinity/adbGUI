namespace adbGUI.Forms
{
    using Methods;
    using System;
    using System.Windows.Forms;

    public partial class Erase : ExtForm
    {
        public Erase()
        {
            InitializeComponent();
        }

        private void Btn_Erase_Click(object sender, EventArgs e)
        {
            if (txt_Erase.Text == "")
            {
                MessageBox.Show(@"Please specify a partition to erase!", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                var s = "fastboot erase " + txt_Erase.Text;
                HelperClass.Execute(s);
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            txt_Erase.Text = ((RadioButton)sender).Text.ToLower();
        }
    }
}
