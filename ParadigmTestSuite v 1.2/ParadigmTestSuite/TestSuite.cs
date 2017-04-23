using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.IO;
using System.Collections;

namespace ParadigmTestSuite
{
    public partial class TestSuite : Form
    {
        TestCase testGen = new TestCase();
        TestDriver testDriver = new TestDriver();

        public TestSuite()
        {
            InitializeComponent();
            //Keep users from using functionality not yet accessable
            generateDriverButton.Enabled = false;
            generateTestButton.Enabled = false;
            saveConfigButton.Enabled = false;
            saveReportButton.Enabled = false;
            generateDriverToolStripMenuItem.Enabled = false;
            generateTestToolStripMenuItem.Enabled = false;
            saveConfigToolStripMenuItem.Enabled = false;
            saveReportToolStripMenuItem.Enabled = false;
        }

        private void languageBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user selects option other than C++ change option back to C++
            if (languageBox.SelectedIndex != 0)
            {
                languageBox.SelectedIndex = -1;
            }
            checkSelected();
        }

        private void testMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkSelected();        
        }

        private void testLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkSelected();
        }

        private void testMethod_ItemCheck(object sender, ItemCheckEventArgs e)
        {

            if(e.NewValue == CheckState.Checked)
            {
                for(int x = 0; x < testMethod.Items.Count; x++)
                {
                    if (e.Index != x)
                        testMethod.SetItemChecked(x, false);
                }
            }
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            String[] filePaths;
            filePaths = openSource();
            if (filePaths.Length != 0)
            {
                testGen.readFile(filePaths[0]);
                testDriver.readFile(filePaths[0]);
            }
        }

        //Purpose: populates the variable box with the input variables
        //Requires: nothing
        //Returns: nothing
        private void populateVariableBox()
        {
            List<Variable> testCases = testGen.SourceInputs;
            string i = "";

            variableBox.Items.Clear();
            foreach (Variable v in testCases)
            {
                i += v.type + " " + v.identifier;
                variableBox.Items.Add(i);
                i = "";
            }
        }

        private void saveReportButton_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveFile()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }
        }

        private void checkSelected()
        {
            if (languageBox.SelectedIndex != -1 && testMethod.CheckedItems.Count != 0 && testLevel.SelectedIndex != -1)
            {
                //Enable generating test driver if O-O testing is checked
                if (testMethod.CheckedItems[0].ToString() == "O-O")
                {
                    generateDriverButton.Enabled = true;
                    generateDriverToolStripMenuItem.Enabled = true;
                    generateTestButton.Enabled = false;
                    generateTestToolStripMenuItem.Enabled = false;
                }
                //Enable generating test cases if O-O testing is not checked
                else
                {
                    generateTestButton.Enabled = true;
                    generateTestToolStripMenuItem.Enabled = true;
                    generateDriverButton.Enabled = false;
                    generateDriverToolStripMenuItem.Enabled = false;
                }
                
                saveConfigButton.Enabled = true;
                saveReportButton.Enabled = true;           
                saveConfigToolStripMenuItem.Enabled = true;
                saveReportToolStripMenuItem.Enabled = true;
            }
            else
            {
                generateDriverButton.Enabled = false;
                generateTestButton.Enabled = false;
                saveConfigButton.Enabled = false;
                saveReportButton.Enabled = false;
                generateDriverToolStripMenuItem.Enabled = false;
                generateTestToolStripMenuItem.Enabled = false;
                saveConfigToolStripMenuItem.Enabled = false;
                saveReportToolStripMenuItem.Enabled = false;
            }
        }

        private string[] openSource()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "C++ files (*.cpp)|*.cpp|header files (*.h)|*.h|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();

            fileNameL.Text = openFileDialog1.SafeFileName;
            return openFileDialog1.FileNames;
        }

        private void openConfig()
        {
            languageBox.SelectedIndex = Properties.Settings.Default.TestLang;
            testMethod.SetItemCheckState(Properties.Settings.Default.TestMeth, CheckState.Checked);
            testLevel.SelectedIndex = Properties.Settings.Default.TestInt;
        } 

        private void saveConfig()
        {
            Properties.Settings.Default.TestLang = languageBox.SelectedIndex;
            Properties.Settings.Default.TestMeth = testMethod.SelectedIndex;
            Properties.Settings.Default.TestInt = testLevel.SelectedIndex;
            Properties.Settings.Default.Save();
        }
        
        private void loadConfigButton_Click(object sender, EventArgs e)
        {
            openConfig();
        }

        private void saveConfigButton_Click(object sender, EventArgs e)
        {
            saveConfig();
        }

        private void generateTest()
        {
            //Load the configuration into a configuration class
            Configuration testConfig = new Configuration();
            testConfig.TestType = testMethod.SelectedIndex;
            testConfig.TestIntensity = testLevel.SelectedIndex;

            ArrayList testCases;
            //Make the test variables
            testCases = testGen.generateTest(testConfig);
            //Populate the variable list
            populateVariableBox();
            //Populate the input box with the generated test cases
            populateInputBox(testCases);
        }

        private void generateTestButton_Click(object sender, EventArgs e)
        {
            generateTest();
        }

        private void populateInputBox(ArrayList testCases)
        {
           
            string s = "";

            inputBox.Items.Clear();
            foreach (ArrayList l in testCases)
            {
                foreach(var i in  l)
                {
                    s += (i.ToString() + "\t");
                }
                inputBox.Items.Add(s);
                s = "";
            }
        }

        private void generateDriverButton_Click(object sender, EventArgs e)
        {
            string driver;
            driver = testDriver.generateDriver();
            driverBox.Text = driver;
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Paradigm Test Suite" + "\n" + "Version 1" + "\n" + "Test case generator", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void userManualToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void HandEnter_KeyDown(object sender, KeyEventArgs e)
        {
            //Once the user has entered something and hit enter
            if (e.KeyCode == Keys.Enter && !String.IsNullOrWhiteSpace(HandEnter.Text))
            {
                //Add the item to the end of the list
                HandResults.Items.Add(HandEnter.Text);
                HandEnter.Clear();
                int thisIndex = HandResults.Items.Count - 1;
                //And compare it to the item in the other list if it has been entered.
                if(ProgramResults.Items.Count >= HandResults.Items.Count)
                {
                    if (HandResults.Items[thisIndex].ToString() == ProgramResults.Items[thisIndex].ToString())
                    {
                        PassFail.Items.Add('√');
                    }
                    else
                    {
                        PassFail.Items.Add('X');
                    }
                }
            }
        }

        private void ProgramEnter_KeyDown(object sender, KeyEventArgs e)
        {
            //Once the user has entered something and hit enter
            if (e.KeyCode == Keys.Enter && !String.IsNullOrWhiteSpace(ProgramEnter.Text))
            {
                //Add the item to the end of the list
                ProgramResults.Items.Add(ProgramEnter.Text);
                ProgramEnter.Clear();
                int thisIndex = ProgramResults.Items.Count - 1;
                //And compare it to the item in the other list if it has been entered.
                if (HandResults.Items.Count >= ProgramResults.Items.Count)
                {
                    if (HandResults.Items[thisIndex].ToString() == ProgramResults.Items[thisIndex].ToString())
                    {
                        PassFail.Items.Add('√');
                    }
                    else
                    {
                        PassFail.Items.Add('X');
                    }
                }
            }
        }

       
    }
}
