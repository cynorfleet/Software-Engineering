using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Windows.Forms;


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
        private string driver;
        private List<Function> functions;
        private string pythonOutfile;

        public TestDriver()
        {
            // driver += "#include <iostream>" + Environment.NewLine;
            // driver += Environment.NewLine;
            // driver 
            driver = @"#include <iostream>

                        using namespace std;

                        int main()
                        {
	                        return 0;
                        }";

            functions = new List<Function>();
            pythonOutfile = "data";
        }

        public void readFile(string sourceFileName)
        {
            List<String> usr_functs = new List<String>();
            List<String> usr_declarations = new List<String>();
            List<String> usr_inputs = new List<String>();

            runPythonScript(sourceFileName, usr_functs, usr_declarations, usr_inputs);
            
            //parse function data
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

        }

        //Purpose: Generates test driver to be suggested to the user
        //Requires:
        //Returns:
        public void generateDriver()
        {

        }

        //Property that returns the list of source code functions
        public List<Function> SourceFunctions
        {
            get { return functions; }
        }

    }
}
