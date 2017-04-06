/*
 Programmers: Cavaughn Browne, Damien Moeller, Christian Norfleet and Aimee Phillips

 TestCase.cs parses source code and generates test cases for the source
 code. 
*/ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace ParadigmTestSuite
{
    public struct Variable
    {
        public string type;
        public string identifier;
    }
    
   public class TestCase
    {
      
       private List<Variable> inputs;
       private string pythonOutfile;

       public TestCase()
        {
            inputs = new List<Variable>();
            pythonOutfile = "data";
        }

       public void readFile(string sourceFileName)
        {
            List<String> usr_functs = new List<String>();
            List<String> usr_declarations = new List<String>();
            List<String> usr_inputs = new List<String>();

            runPythonScript(sourceFileName, usr_functs, usr_declarations, usr_inputs);
            parseInputs(usr_inputs, usr_declarations);
          //  readPyScriptOutput();

        }

        //Purpose: Generates test cases for each input, according to the configurations
        //set by the user
        //Requires: Configuration config; Configurations for the test
        //Returns: an ArrayList of ArrayLists; containing test cases 
        //for each input
        public ArrayList generateTest(Configuration config)
        {
            ArrayList testCases = new ArrayList();
            int numTests = 10;

            switch (config.TestType)
            {
                case 2: //If testType is 0, then random test cases are generated for each input
                    testCases = randomTestGen(numTests);

                    break;

                default:
                    //default is random testing
                    testCases = randomTestGen(numTests)
                        ;
                     break;
             
            }

            return testCases;

        }

        //Purpose: Generates numTests random test case inputs for each input
        //Requires: int numTests
        //Returns: an ArrayList of ArrayLists; containing test cases 
        //for each input
        private ArrayList randomTestGen(int numTests)
        {
            ArrayList testCases = new ArrayList();
            Random rand = new Random();

            foreach (Variable v in inputs)
            {
                if (v.type == "int")
                {
                    ArrayList t = new ArrayList();
                    int randomInt;
                    int one;

                    // generate a number of negative and non-negative
                    // random integers
                    for (int n = 0; n < numTests; n++)
                    {
                        //generate 1 or -1 randomly
                        one = rand.Next(-1, 2);
                        while (one == 0)
                            one = rand.Next(-1, 1);

                        //gets a random integer (can be negative)
                        randomInt = rand.Next(0, 11) * one;
                        t.Add(randomInt);
                    }

                    testCases.Add(t);
                }
                else if (v.type == "char")
                {
                    ArrayList t = new ArrayList();
                    char randomChar;

                    for (int c = 0; c < numTests; c++)
                    {
                        //generate random char of ascii values between 32 and 127
                        randomChar = Convert.ToChar(rand.Next(32, 128));
                        t.Add(randomChar);
                    }

                    testCases.Add(t);
                }
                else if (v.type == "double" || v.type == "float")
                {
                    ArrayList t = new ArrayList();
                    double randomFloat;

                    //generate a number of random floating point numbers
                    for (int f = 0; f < numTests; f++)
                    {
                        randomFloat = rand.NextDouble() * (double.MaxValue - double.MinValue) + double.MinValue;

                        t.Add(randomFloat);
                    }

                    testCases.Add(t);
                }

                else if (v.type == "string")
                {
                    ArrayList t = new ArrayList();
                    string randomString = "";
                    char randomChar;
                    int sLength;

                    for (int c = 0; c < numTests; c++)
                    {
                        //get a random string length
                        sLength = rand.Next(0, 10);

                        //generate a random string by generating sLength random chars
                        for (int l = 0; l < sLength; l++)
                        {
                            //generate random char of ascii values between 32 and 127
                            randomChar = Convert.ToChar(rand.Next(32, 127));
                            randomString += randomChar; ;
                        }
                        t.Add(randomString);
                    }

                    testCases.Add(t);
                }

            }
            return testCases;
        }

        //Purpose:Runs a python script that will parse the source file and write data about inputs,
        //and functions to an output file called data.dat.
        //Requires: The source file name, List<String> usr_functs,
        //  List<String> usr_declarations, List<String> usr_inputs
        //Returns: nothing
        private void runPythonScript(string sourceFileName, List<String> usr_functs,
        List<String> usr_declarations, List<String> usr_inputs)
        {
            List<string> argv = new List<string>();
            IronPython.Runtime.List functions = new IronPython.Runtime.List();
            IronPython.Runtime.List declarations = new IronPython.Runtime.List();
            IronPython.Runtime.List userinputs = new IronPython.Runtime.List();

            argv.Add(sourceFileName);
            argv.Add(pythonOutfile);

            var engine = Python.CreateEngine(); // Extract Python language engine from their grasp
            var scope = engine.GetSysModule();
            scope.SetVariable("argv", argv);
            ScriptSource source = engine.CreateScriptSourceFromFile(@"testcase.py"); // Load the script
            object result = source.Execute(scope);

            //gets specific variables from the python script
            functions.extend(scope.GetVariable("functions"));
            declarations.extend(scope.GetVariable("datatype"));
            userinputs.extend(scope.GetVariable("userinputs"));

            snatchData(functions, usr_functs);
            snatchData(declarations, usr_declarations);
            snatchData(userinputs, usr_inputs);
            foreach (var item in userinputs)
                Console.WriteLine(item.ToString());

        }

        private void snatchData(IronPython.Runtime.List raw_data, List<String> new_data)
        {
            foreach (var i in raw_data)
                new_data.Add(i.ToString());
        }

        //Purpose: Parse the strings the python script returned
        //to get the inputs 
        //Requires: List<string> usr_inputs, List<string> usr_declarations
        //Returns: nothing
        private void parseInputs(List<string> usr_inputs, List<string> usr_declarations)
        {
            Variable v;
           
            for(int s = 0; s < usr_inputs.Count; s++)
            {
                v.identifier = usr_inputs[s];
                v.type = usr_declarations[s];
                inputs.Add(v);
            }

        }//end parseInputs

        //Purpose: Reads the output file generated by the python script that parses the source code
        //and gets all the input data within the file.
        //Requires: nothing
        //Returns: nothing
        private void readPyScriptOutput()
        {
            string file = pythonOutfile + ".dat";
           
            if (File.Exists(file)) 
            {
                using (StreamReader fs = new StreamReader(file))
                {
                    string line;
                    Variable v = new Variable();

                    //while its not the end of the file keep reading
                    line = fs.ReadLine();
                    while (line != null)
                    {
                         //To store the declarations in the file
                        List<Variable> declarations = new List<Variable>();

                        //This block is for reading in all of the 
                        //inputs in the file with their data types
                        //and store them as a Variable in a List inputs
                        if (line == "Type:")
                        {
                            line = fs.ReadLine();
                            while (line != "--------------------------------------------")
                            {
                                string[] subStrings;
                                char[] delimeter = { ' '}; 

                                subStrings = line.Split(delimeter);

                                v.type = subStrings[0];
                                v.identifier = subStrings[1];

                                inputs.Add(v);

                                line = fs.ReadLine();
                            }
                        }

                        if (line == "Inputs:")
                        {
                            line = fs.ReadLine();
                            while (line != null)
                            {
                                //find the declaration that matches
                                //'line' and add it to inputs with its type
                                foreach(Variable vaR in declarations)
                                {
                                    if (vaR.identifier == line) 
                                    {
                                        inputs.Add(vaR);
                                    }
                                }

                                line = fs.ReadLine();
                            }
                        }
                        line = fs.ReadLine();
                    }
                }
            }
            else 
            {
               MessageBox.Show("Source Code Data File Error", "Source code parse data file does not exist.",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        //Property that returns the list of source code inputs
        public List<Variable> SourceInputs
        {
            get { return inputs; }
        }
    }
}
