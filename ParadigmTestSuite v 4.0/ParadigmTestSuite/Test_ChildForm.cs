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
    public partial class Test_ChildForm : Form
    {
        TestCase testGen = new TestCase();
        TestDriver testDriver = new TestDriver();

        public Test_ChildForm(string filePath, string safeFileName)
        {
            InitializeComponent();

            //Prevent  menu strip from being merged with any other
            menuBar.AllowMerge = false;

            //Keep users from using functionality not yet accessable
            generateDriverButton.Enabled = false;
            generateTestButton.Enabled = false;
            saveConfigButton.Enabled = false;
            saveReportButton.Enabled = false;
            generateDriverToolStripMenuItem.Enabled = false;
            generateTestToolStripMenuItem.Enabled = false;
            saveConfigToolStripMenuItem.Enabled = false;
            saveReportToolStripMenuItem.Enabled = false;
            lowerLBox.Enabled = false;
            upperLBox.Enabled = false;
            enterLimitButton.Enabled = false;
            clearButton.Enabled = false;

            fileNameL.Text = safeFileName;
            this.Text = safeFileName;

            //Disable testing methods that arent implemented
           for(int i = 0; i < testMethod.Items.Count; i++)
            {
                string it = testMethod.Items[i].ToString();

                if(it.ToString() == "Loop"|| it.ToString() == "Functional" || it.ToString() == "Path")
                {
                    testMethod.SetItemCheckState(i, CheckState.Indeterminate);
                }
            }

            if (filePath.Length != 0)
            {
                testGen.readFile(filePath);
                testDriver.readFile(filePath);

                //Populate the variable list
                populateVariableBox();
            }

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
           
            string it = testMethod.Items[e.Index].ToString();
            
            //Keep the values Indeterminate for the testing methods that arent implemented
            if(e.NewValue != CheckState.Indeterminate && (it == "Loop" || it == "Path" || it == "Functional"))
            {
                e.NewValue = CheckState.Indeterminate;
            }

            if (e.NewValue == CheckState.Checked)
            {              
                for (int x = 0; x < testMethod.Items.Count; x++)
                {
                    if (e.Index != x)
                        testMethod.SetItemChecked(x, false);
                }
              
            }
        }

        //Purpose: populates the variable box with the input variables
        //Requires: nothing
        //Returns: nothing
        private void populateVariableBox()
        {
            List<Variable> testInputs = testGen.SourceInputs;
            string i = "";

            variableBox.Items.Clear();
            foreach (Variable v in testInputs)
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
            string levelAdjective = "";
            if (testLevel.SelectedItem.Equals(1))
            {
                levelAdjective = "no";
            }
            else if (testLevel.SelectedItem.Equals(2))
            {
                levelAdjective = "some";
            }
            else if (testLevel.SelectedItem.Equals(3))
            {
                levelAdjective = "thorough";
            }
            else
            {
                levelAdjective = "error";
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (generateTestButton.Enabled == true)
                {
                    saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.RestoreDirectory = true;

                    String[] lines = { "Testing report for file " + fileNameL.Text + '.',
                        "Testing " + languageBox.GetItemText(languageBox.SelectedItem) + " program using the " +
                        testMethod.GetItemText(testMethod.SelectedItem) + " testing method. The program was tested assuming " +
                        levelAdjective + " exception handling.\n"};

                    List<string> variPairs = new List<string>();
                    int j = 0;

                    foreach (string i in variableBox.Items)
                    {
                        variPairs.Add("\nInput variable: ");
                            variPairs.Add(i);
                            variPairs.Add("\nTested using values: ");
                            variPairs.Add(inputBox.Items[j].ToString());
                            j++;
                    }
                    variPairs.ToArray<string>();

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.FileName))
                    {
                        foreach (string line in lines)
                        {
                            file.WriteLine(line);
                        }
                        foreach (string vline in variPairs)
                        {
                            file.WriteLine(vline);
                        }
                    }
                }
                if (generateDriverButton.Enabled == true)
                {
                    saveFileDialog1.Filter = "cpp (*.cpp)|*.cpp";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.RestoreDirectory = true;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.FileName))
                    {
                        foreach (string vline in inputBox.Items)
                        {
                            file.WriteLine(vline);
                        }
                    }
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

                //Enables and disables elements for boundary testing
                if (testMethod.CheckedItems[0].ToString() == "Boundary")
                {
                    lowerLBox.Enabled = true;
                    upperLBox.Enabled = true;
                    clearButton.Enabled = true;

                }
                else
                {
                    lowerLBox.Enabled = false;
                    upperLBox.Enabled = false;
                    enterLimitButton.Enabled = false;
                    clearButton.Enabled = false;
                    //clear the limit boxes
                    lowerLBox.Clear();
                    upperLBox.Clear();

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
                lowerLBox.Enabled = false;
                upperLBox.Enabled = false;
                enterLimitButton.Enabled = false;
                clearButton.Enabled = false;
            }
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
            if(testMethod.CheckedIndices.Count != 0)
                Properties.Settings.Default.TestMeth = testMethod.CheckedIndices[0];
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

            if(testMethod.CheckedIndices.Count != 0)
                 testConfig.TestType = testMethod.CheckedIndices[0];

             testConfig.TestIntensity = testLevel.SelectedIndex;
            
            ArrayList testCases;
            List<string> lowerLimits = new List<string>(), upperLimits = new List<string>();

            if (testConfig.TestType == 1 )
            {
                foreach (string s in limitsLBoxL.Items)
                    lowerLimits.Add(s);

                foreach (string s in limitsLBoxU.Items)
                    upperLimits.Add(s);

                //Make sure limits for all the inputs have been specified
                if (lowerLimits.Count == testGen.SourceInputs.Count && 
                    upperLimits.Count == testGen.SourceInputs.Count)
                {
                    //Make the test variables
                    testCases = testGen.generateTest(testConfig, lowerLimits, upperLimits);

                    //Populate the input box with the generated test cases
                    populateInputBox(testCases);
                }
                else
                {
                    string message = @"Each Input has to be assigned appropriate limits. 
                        Please do so then try again.";
                    //Message Box
                    MessageBox.Show(message, "Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                testCases = testGen.generateTest(testConfig, lowerLimits, upperLimits);

                //Populate the input box with the generated test cases
                populateInputBox(testCases);
            }                            
         
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
                foreach (var i in l)
                {
                    s += (i.ToString() + "\t");
                }
                inputBox.Items.Add(s);
                s = "";
            }
        }

        private void generateDriverButton_Click(object sender, EventArgs e)
        {
            List<string> driverList;

            driverList = testDriver.generateDriver();
            inputBox.Items.Clear();

            foreach (string s in driverList)
                inputBox.Items.Add(s);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Paradigm Test Suite" + "\n" + "Version 1" + "\n" + "Test case generator", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void userManualToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
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
                if (ProgramResults.Items.Count >= HandResults.Items.Count)
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

        private void enterLimitButton_Click(object sender, EventArgs e)
        {
            int sel;

            if (limitsLBoxL.SelectedIndex != -1 || limitsLBoxU.SelectedIndex != -1)
            {
                //set sel to the index that isn't -1
               sel =  limitsLBoxL.SelectedIndex != -1 ?limitsLBoxL.SelectedIndex :  limitsLBoxU.SelectedIndex;

                limitsLBoxL.Items[sel] = lowerLBox.Text;
                limitsLBoxU.Items[sel] = upperLBox.Text;

                //clear the selections out after
                limitsLBoxL.ClearSelected();
                limitsLBoxU.ClearSelected();
            }
            else if (variableBox.Items.Count > limitsLBoxL.Items.Count)
            {
                limitsLBoxL.Items.Add(lowerLBox.Text);
                limitsLBoxU.Items.Add(upperLBox.Text);

            }

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            if (limitsLBoxL.SelectedIndex == -1 && limitsLBoxU.SelectedIndex == -1)
            {
                //Clear both boxes completely if none is selected
                limitsLBoxL.Items.Clear();
                limitsLBoxU.Items.Clear();
            }
            else
            {
                //Clears the selected item if one is selected
                if(limitsLBoxL.SelectedIndex != -1)
                    limitsLBoxL.Items[limitsLBoxL.SelectedIndex] = "";

                if (limitsLBoxU.SelectedIndex != -1)
                    limitsLBoxU.Items[limitsLBoxU.SelectedIndex] = "";
            }
        }

        private void limitBox_TextChanged(object sender, EventArgs e)
       {

            int result, result2;
            bool lower = false;

            //check if the integer in the lowerLBox is less than
            //the integer in the upperLBox
            Int32.TryParse(lowerLBox.Text, out result);
            Int32.TryParse(upperLBox.Text, out result2);

            if (result < result2)
                lower = true;

            //Disable enter button if the boxes are empty
            if (lowerLBox.Text == "" || upperLBox.Text == "" || !lower)
            {
                enterLimitButton.Enabled = false;
            }
            else
            {

                //Only enable the enter button when the lowerLBox and upperLBox have integers in them
                //and the number in the lower limit box is less than the number in the upper limit box
                if (Int32.TryParse(lowerLBox.Text, out result) && 
                    Int32.TryParse(upperLBox.Text, out result2) && lower)
                {
                    enterLimitButton.Enabled = true;
                }           
               
            }
        }
    }
}
