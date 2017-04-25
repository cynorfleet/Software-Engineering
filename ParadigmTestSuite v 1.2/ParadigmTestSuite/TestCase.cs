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
        private Random rand;

       public TestCase()
        {
            inputs = new List<Variable>();
            pythonOutfile = "data";
            rand = new Random();
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
        //Requires: Configuration config,  List<string> lower, List<string> upper
        //Returns: an ArrayList of ArrayLists; containing test cases 
        //for each input
        public ArrayList generateTest(Configuration config, List<string> lower, List<string> upper)
        {
            ArrayList testCases = new ArrayList();
            int numTests = 10;

            switch (config.TestType)
            {
                case 0:
                    break;
                case 1:
                    testCases = boundaryTestGen(config.TestIntensity, lower, upper);
                    break;
                case 2: //If testType is 0, then random test cases are generated for each input

                    testCases = randomTestGen(numTests, config.TestIntensity);

                    break;

                default:
                    //default is random testing
                    testCases = randomTestGen(numTests, config.TestIntensity);

                     break;
             
            }

            return testCases;

        }

        //Purpose: Generates numTests random test case inputs for each input
        //Requires: int numTests
        //Returns: an ArrayList of ArrayLists; containing test cases 
        //for each input
        private ArrayList randomTestGen(int numTests, int level)
        {
            ArrayList testCases = new ArrayList();
            ArrayList t;

            switch (level)
            {
                case 0:
                    foreach (Variable v in inputs)
                    {
                        t = randomDataGen(numTests, v.type);
                        testCases.Add(t);
                    }

                    break;
                case 1:
                    //Increased max value of ints, doubles as well as length of strings generated
                    foreach (Variable v in inputs)
                    {
                        t = randomDataGen(numTests, v.type, Int32.MaxValue);
                        testCases.Add(t);
                    }
                    break;
                case 2:
                    //Increased max value of ints, doubles as well as length of strings generated.
                    //The types of data generated for each input is random
                    string[] s = new string[] {"int", "double", "float", "char", "string"};
                    int r;

                    foreach (Variable v in inputs)
                    {
                        r = rand.Next(0, 5);
                        t = randomDataGen(numTests, s[r], Int32.MaxValue);
                        testCases.Add(t);
                    }
                    break;

                default:
                    foreach (Variable v in inputs)
                    {
                        t = randomDataGen(numTests, v.type);
                        testCases.Add(t);
                    }

                    break;
          }
            return testCases;
        }

        //Purpose: generates a number of random values for a specific type
        //Requires: int num, string type, maxUnsignedVal
        //Returns: ArrayList t with values for type
        ArrayList randomDataGen(int num, string type, int maxUnsignedVal = 11)
        {
            ArrayList t = new ArrayList();
                      
            if (type == "int")
            {
                int randomInt;
                // generate a number of negative and non-negative
                // random integers
                for (int n = 0; n < num; n++)
                {
                    //gets a random integer (can be negative)
                    randomInt = rand.Next(-maxUnsignedVal, maxUnsignedVal);
                    t.Add(randomInt);
                }

            }
            else if (type == "char")
            {
                char randomChar;

                for (int c = 0; c < num; c++)
                {
                    //generate random char of ascii values between 32 and 127
                    randomChar = Convert.ToChar(rand.Next(32, 128));
                    t.Add(randomChar);
                }

            }
            else if (type == "double" || type == "float")
            {
                double randomFloat;
                //generate a number of random floating point numbers
                for (int f = 0; f < num; f++)
                {

                    randomFloat = rand.NextDouble() * (maxUnsignedVal/2 - (-maxUnsignedVal / 2)) + (-maxUnsignedVal / 2);

                    t.Add(randomFloat.ToString("0.##"));
                }

            }

            else if (type == "string")
            {
                string randomString = "";
                char randomChar;
                int sLength;

                for (int c = 0; c < num; c++)
                {
                    //get a random string length not exceeding 20
                    sLength = rand.Next(0, maxUnsignedVal % 21);

                    //generate a random string by generating sLength random chars
                    for (int l = 0; l < sLength; l++)
                    {
                        //generate random char of ascii values between 32 and 127
                        randomChar = Convert.ToChar(rand.Next(32, 127));
                        randomString += randomChar; ;
                    }
                    t.Add(randomString);
                }
            }                
              
            return t;
        }

        //Purpose: generates a number of test case values around the boundaries
        //of the input: Lower and upper. 
        //Requires: Configuration config, List<string> lower, List<string> upper
        //Returns: ArrayList of ArrayLists containing test cases for each input
        private ArrayList boundaryTestGen(int level, List<string> lower, List<string> upper)
        {
            ArrayList testCases = new ArrayList();
           
            int count = 0;

            foreach (Variable v in inputs)
            {
                ArrayList t = new ArrayList();

                //create test cases on and around the boundaries
                if (v.type == "int")
                {
                    int value;
                
                        value = Int32.Parse(lower[count]) - 1;
                        t.Add(value);

                        value = Int32.Parse(lower[count]);
                        t.Add(value);

                        value = Int32.Parse(lower[count]) + 1;
                        t.Add(value);
              
                        value = Int32.Parse(upper[count]) - 1;
                        t.Add(value);

                        value = Int32.Parse(upper[count]);
                        t.Add(value);

                        value = Int32.Parse(upper[count]) + 1;
                        t.Add(value);
                 
                
                        //take a nominal value
                        value = (Int32.Parse(lower[count]) + Int32.Parse(upper[count])) / 2;
                        t.Add(value);

                        if (level > 0)
                        {
                            t.Add(Int32.MaxValue);
                            t.Add(Int32.MinValue);
                        }

                }
                else if (v.type == "char")
                {
                    int value;

                        //generate ints on and around the boundaries
                        value = Char.Parse(lower[count]) - 1;
                        t.Add(value);

                        value = Char.Parse(lower[count]);
                        t.Add(value);

                        value = Char.Parse(lower[count]) + 1;
                        t.Add(value);
                 
                  
                        value = Char.Parse(upper[count]) - 1;
                        t.Add(value);

                        value = Char.Parse(upper[count]);
                        t.Add(value);

                        value = Char.Parse(upper[count]) + 1;
                        t.Add(value);                  
                
                        //take a nominal value
                        value = (Char.Parse(lower[count]) + Char.Parse(upper[count])) / 2;
                        t.Add(value);

                        if (level > 0)
                        {
                            t.Add(Int32.MaxValue);
                            t.Add(Int32.MinValue);
                        }

                }
                else if (v.type == "double" || v.type == "float")
                {
                    double value;
                   
                        //generate floating points on and around the boundaries
                        value = Double.Parse(lower[count]) - rand.NextDouble();
                        t.Add(value.ToString("0.##"));

                        value = Double.Parse(lower[count]);
                        t.Add(value.ToString("0.##"));

                        value = Double.Parse(lower[count]) + rand.NextDouble();
                        t.Add(value.ToString("0.##"));                  
                   
                        value = Double.Parse(upper[count]) - rand.NextDouble();
                        t.Add(value.ToString("0.##"));

                        value = Double.Parse(upper[count]);
                        t.Add(value.ToString("0.##"));

                        value = Double.Parse(upper[count]) + rand.NextDouble();
                        t.Add(value.ToString("0.##"));                   
                
                        //take a nominal value
                        value = (Double.Parse(lower[count]) + Double.Parse(upper[count])) / 2.0;
                        t.Add(value.ToString("0.##"));

                        if (level > 0)
                        {
                            t.Add(Double.MaxValue);
                            t.Add(Double.MinValue);
                        }
                }

                else if (v.type == "string")
                {
                    string randomString = "";
                    int sLength;

                    //The length of the string is the important factor here
               
                    if (Int32.Parse(lower[count]) >= 0)
                    {
                        sLength = Int32.Parse(lower[count]) - 1;
                        randomString = randomStringGen(sLength);
                        t.Add(randomString);

                        sLength = Int32.Parse(lower[count]);
                        randomString = randomStringGen(sLength);
                        t.Add(randomString);


                        sLength = Int32.Parse(lower[count]) + 1;
                        randomString = randomStringGen(sLength);
                        t.Add(randomString);
                    }
                   
                        sLength = Int32.Parse(upper[count]) - 1;
                        randomString = randomStringGen(sLength);
                        t.Add(randomString);

                        sLength = Int32.Parse(upper[count]);
                        randomString = randomStringGen(sLength);
                        t.Add(randomString);

                        sLength = Int32.Parse(upper[count]) + 1;
                        randomString = randomStringGen(sLength);
                        t.Add(randomString);                 
                 
                        //take a nominal value
                        sLength = (Int32.Parse(lower[count]) + Int32.Parse(upper[count])) / 2;
                        randomString = randomStringGen(sLength);
                        t.Add(randomString);

                        if (level > 0)
                        {
                            sLength = Int32.MaxValue;
                            randomString = randomStringGen(sLength);
                            t.Add(randomString);
                           
                        }

                }

                testCases.Add(t);
                count++;
            }

            return testCases;
        }

        //Purpose: Generates a random string of a specified length
        //Requires: int length
        //Returns:: the generated string
        private string randomStringGen(int length)
        {
            char randomChar;
            string randomString = "";
            //generate a random string by generating sLength random chars
            for (int l = 0; l < length; l++)
            {
                //generate random char of ascii values between 32 and 127
                randomChar = Convert.ToChar(rand.Next(32, 127));
                randomString += randomChar; ;
            }

            return randomString;
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
            

        }

        //Purpose: Takes data from the IronPython list and stores it in a List of strings
        //Requires: IronPython.Runtime.List raw_data, List<String> new_data
        //Returns: nothing
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
            inputs.Clear(); //clear inputs
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
