using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace ParadigmTestSuite
{
    public struct Function
    {
        
        public Function(string type, string fName)
        {
            parameterList = new List<string>();
            returnType = type;
            name = fName;
        }

        public string returnType;
        public string name;
        public List<string> parameterList;
    }


    public class TestDriver
    {
        private string className = "";
        private List<Function> functions;
        private string pythonOutfile;
        Random rand;

        public TestDriver()
        {
            functions = new List<Function>();
            pythonOutfile = "data";
            rand = new Random();
        }

        public void readFile(string sourceFileName)
        {
            List<String> usr_functs = new List<String>();
            List<String> usr_declarations = new List<String>();
            List<String> usr_inputs = new List<String>();

            runPythonScript(sourceFileName, usr_functs, usr_declarations, usr_inputs);
            
            //parse function data
            parseMethods(usr_functs);
        }

      

        //Purpose:Runs a python script that will parse the source file and write data about inputs,
        //and functions to an output file called data.dat.
        //Requires: The source file name 
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
            className = scope.GetVariable("className");

            snatchData(functions, usr_functs);
            snatchData(declarations, usr_declarations);
            snatchData(userinputs, usr_inputs);
        }

        //Purpose:Runs a python script that will parse the source file and write data about inputs,
        //and functions to an output file called data.dat.
        //Requires: The source file name, List<String> usr_functs,
        //  List<String> usr_declarations, List<String> usr_inputs
        //Returns: nothing
        private void snatchData(IronPython.Runtime.List raw_data, List<String> new_data)
        {
            foreach (var i in raw_data)
                new_data.Add(i.ToString());
        }


        private void parseMethods(List<String> usr_functs)
        {
            Regex reg = new Regex(@"\w+");
            Function f = new Function("", "");
            int start;
            //list for storing regex matchesof words in usr_functs elements
            List<string> l = new List<string>();


            functions.Clear(); // clears list 

            f.name = className;
            functions.Add(f);

            foreach(string s in usr_functs)
            {
                foreach (Match m in reg.Matches(s))
                {
                    l.Add(m.Value);
                    
                }

                if (l[0] == "void" || l[0] == "char" || l[0] == "int" ||
                    l[0] == "string" || l[0] == "double" || l[0] == "float")
                {
                    f = new Function(l[0], l[1]);
                    start = 2;
                }
                else
                {
                    f = new Function("", l[0]);
                    start = 1;
                }
                
                for(int i = start; i < l.Count; i++)
                {
                    if (l[i] == "char" || l[i] == "int" ||
                        l[i] == "string" || l[i] == "double" || l[i] == "float")
                    {
                        f.parameterList.Add(l[i]);
                    }
                }

                functions.Add(f);
                l.Clear();

            } // end foreach

        }

        //Purpose: Generates test driver to be suggested to the user
        //Requires:
        //Returns: a string - a test driver script
        public string generateDriver()
        {
            string driver = "#include <iostream>\n";
            driver += "#include <string>\n";
            driver += String.Format("#include\"{0}.h\" \n", className);

            driver +=
            @"using namepace std;
            
int main()
{
";
            string vars;
            // instantiate an appropriate object
            vars = className + " classObject;\n";
            driver += vars;

            bool first;
            int varCount = 0;
            //create a call for each class method
            foreach (Function f in functions)
            {
                //create parameterized constructor object
                if (f.name == className && f.parameterList != null && f.parameterList.Count != 0)
                {                   
                    vars = className + " classObject(";

                    first = true;
                    //generate and concatenate a value for each parameter 
                    foreach (string s in f.parameterList)
                    {
                        if (!first)
                            vars += ", ";

                        vars += randomDatGen(s);
                        first = false;

                    }
                    vars += ");\n";
                    driver += vars;
                  
                }//end if

                //f is not a copy constructor, construct a call for that method
                if(f.name != className)
                {
                    //first figure out what type the method returns
                    //and create a variable to hold the returned value
                    if (f.returnType == "int")
                    {
                        vars = "int var" + varCount + ";\n";
                        driver += vars;
                        varCount++;
                    }
                    else if (f.returnType == "char")
                    {
                        vars = "char var" + varCount + ";\n";
                        driver += vars;
                        varCount++;
                    }
                    else if (f.returnType == "double" || f.returnType == "float")
                    {
                        vars = "double var" + varCount + ";\n";
                        driver += vars;
                        varCount++;
                    }

                    else if (f.returnType == "string")
                    {
                        vars = "string var" + varCount + ";\n";
                        driver += vars;
                        varCount++;
                    }


                    vars = "";
                    //if the return type isnt void, construct the method call
                    //and have it return to the right variable
                    if (f.returnType != "void")
                    {
                        vars = "var" + Convert.ToString(varCount - 1) + " = ";
                                     
                    }

                    vars += ("classObject." + f.name + "(");

                    //gernerate random data to pass as arguments into the method
                    first = true; // boolean to mark first pass through loop
                    foreach (string m in f.parameterList)
                    {
                        if (!first)
                            vars += ", ";
                      
                        //quotes go around strings and chars
                        if (m == "string")
                            vars += "\"" + randomDatGen(m).ToString() + "\"";
                        else if(m == "char")
                            vars += "\'" + randomDatGen(m).ToString() + "\'";
                        else
                            vars += randomDatGen(m).ToString();

                        first = false; //set to false indicating the first pass 
                    }

                    vars += ");\n";
                    driver += vars;
                }//endif

            }// end foreach

            vars = @"return 0;
}";
            driver += vars;

            return driver;
        }

        //Purpose: Generates random data
        //Requires: string type
        //Returns: string 
        private string randomDatGen(string type)
        {
            string randomDat = "";
         
            if (type == "int")
            {
                int randomInt;
              
                //gets a random integer in the range
                randomInt = rand.Next(-10, 11);
                randomDat = randomInt.ToString();
                                  
            }
            else if (type == "char")
            {
                char randomChar;

                //generate random char of ascii values between 32 and 127
                randomChar = Convert.ToChar(rand.Next(32, 128));
                randomDat = randomChar.ToString();
            }
            else if (type == "double" || type == "float")
            {
                double randomFloat;

                //generate a number of random floating point numbers                   
                randomFloat = rand.NextDouble() * (11 - (-10)) + (-10);
                randomDat = randomFloat.ToString();                               
            }

            else if (type == "string")
            {
                string randomString = "";
                char randomChar;
                int sLength;

                  
                //get a random string length
                sLength = rand.Next(0, 11);

                //generate a random string by generating sLength random chars
                for (int l = 0; l < sLength; l++)
                {
                    //generate random char of ascii values between 32 and 127
                    randomChar = Convert.ToChar(rand.Next(65, 127));
                    randomString += randomChar; ;
                }
                 randomDat = randomString;

            }
            return randomDat;
        }

        //Property that returns the list of source code functions
        public List<Function> SourceFunctions
        {
            get { return functions; }
        }

    }
}
