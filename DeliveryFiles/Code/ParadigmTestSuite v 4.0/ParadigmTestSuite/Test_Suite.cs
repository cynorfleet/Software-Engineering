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
            tabForms.Visible = false;
        }

        //Purpose: Opens a dialog so the user can select a file of choice
        //Requires: out string safeFileName
        //Returns: the file path of the selected file and the safeFileName
        //as an out argument
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

        //Purpose: When a child form activates, associate it with a new tab page
        //and by tag. 
        //Requires: object sender, EventArgs e
        //Returns: nothing 
        private void Test_SuiteChildMDIActivate(object sender,
                                 EventArgs e)
        {

            if (this.ActiveMdiChild == null)
                tabForms.Visible = false;
            // If no any child form, hide tabControl 
            else
            {
                this.ActiveMdiChild.WindowState =
                FormWindowState.Maximized;
                // Child form always maximized 

                // If child form is new and no has tabPage, 
                // create new tabPage 
                if (this.ActiveMdiChild.Tag == null)
                {
                    // Add a tabPage to tabControl with child 
                    // form caption 
                    TabPage tp = new TabPage(this.ActiveMdiChild
                                             .Text);
                    tp.Tag = this.ActiveMdiChild;
                    tp.Parent = tabForms;
                    tabForms.SelectedTab = tp;

                    //Associate the child form via tag with the tab page
                    this.ActiveMdiChild.Tag = tp;

                    //Add an event to the active mdi child form
                    //That will close the tab page when the 
                    //associated child form is closed
                    this.ActiveMdiChild.FormClosed +=
                        new FormClosedEventHandler(
                                        ActiveMdiChild_FormClosed);
                }

                if (!tabForms.Visible) tabForms.Visible = true;

            }
        }

        //Purpose: Closes the correspond tab page with the child form
        //Requires: object sender, EventArgs e
        //Returns: nothing 
        private void ActiveMdiChild_FormClosed(object sender,
                                    FormClosedEventArgs e)
        {
            ((sender as Form).Tag as TabPage).Dispose();
        }

        //Purporse: Activate a tab pages corresponding child form when selected
        //Requires: object sender, EventArgs e
        //Returns: nothing 
        private void tabForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabForms.SelectedTab != null) && (tabForms.SelectedTab.Tag != null))
            {
                (tabForms.SelectedTab.Tag as Form).Select();
            }
        }


        //Purpose: Terminates application
        //Requires: object sender, EventArgs e
        //Returns: nothing 
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Purpose: Opens a file
        //Requires: object sender, EventArgs e
        //Returns: nothing 
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string safeFileName = "";
            string filePath = openSource(out safeFileName);

            //create new Test_ChildForm with the file and set its parent to this
            Test_ChildForm childForm = new Test_ChildForm(filePath, safeFileName);
            childForm.MdiParent = this;

            childForm.Show();
        }

        //Purporse: Open the UserManual using the Systems default pdf viewer
        //Requires: object sender, EventArgs e
        //Returns: nothing
        private void userManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ProcessStartInfo startInfo = new ProcessStartInfo(@"UserManual\TestSuiteUserManual.pdf");
                //Process.Start(startInfo);
                String openPDFFile = System.IO.Directory.GetCurrentDirectory() + @"\TestSuiteUserManual.pdf";
                System.IO.File.WriteAllBytes(openPDFFile, Properties.Resources.TestSuiteUserManual);
                System.Diagnostics.Process.Start(openPDFFile);
            }
            catch(Exception ex)
            {
                MessageBox.Show("UserManual could not be opened!\n" + ex.ToString(), "User Manual Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Purpose: Brings up a message box telling the user about the applications
        //Reuqires: object sender, EventArgs e
        //Returns: nothing
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string about = "Paradigm Test Suite\n\n";
            about += "Company: Paradigm \n\n";
            about += "Programmers: Cavaughn Browne, Damien Moeller, Christian Norfleet, ";
            about += "and Aimee Phillips\n\n";
            about += "Version 1.0";

            MessageBox.Show(about, "About Paradigm Test Suite", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
