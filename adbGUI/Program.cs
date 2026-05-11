// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace adbGUI
{
    using System;
    using System.Windows.Forms;
    using Forms;
    using Methods;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Dependencies.Check();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Application.Run(new MainForm());
        }
    }
}