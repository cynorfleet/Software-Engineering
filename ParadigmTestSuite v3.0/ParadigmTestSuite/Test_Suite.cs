using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParadigmTestSuite
{
    public partial class Test_Suite : Form
    {

        public Test_Suite()
        {
            InitializeComponent();
        }

        private string openSource(out string safeFileName)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
           

            openFileDialog1.Filter = "C++ files (*.cpp)|*.cpp|header files (*.h)|*.h|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();

            safeFileName = openFileDialog1.SafeFileName;
            return openFileDialog1.FileName;
        }



        //Purpose: Terminates application
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Purpose: Opens a file
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string safeFileName = "";
            string filePath = openSource(out safeFileName);

            //create new Test_ChildForm with the file and set its parent to this
            Test_ChildForm childForm = new Test_ChildForm(filePath, safeFileName);
            childForm.MdiParent = this;

            childForm.Show();
        }

        //Open the UserManual using the Systems default pdf viewer
        private void userManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(@"UserManual\TestSuiteUserManual.pdf");
                Process.Start(startInfo);
            }
            catch(Exception ex)
            {
                MessageBox.Show("UserManual could not be opened!\n" + ex.ToString(), "User Manual Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
