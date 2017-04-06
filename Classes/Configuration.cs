using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadigmTestSuite
{
   public class Configuration
    {
        private int testType;
        private int testIntensity;
        
        public Configuration()
        {
            testType = 0;
            testIntensity = 0;
        }

        //Purpose: Saves configuration details to a file
        //to be loaded later
        //Requires: nothing
        //Returns: nothing
        public void saveConfiguration()
        {

        }

        //Purpose: Loads previously save configuration details from a file
        //to be used for a test
        //Requires: nothing
        //Returns: nothing
        public void loadConfiguration()
        {

        }

        public int TestType
        {
            get { return testType; }
            set { testType = value; }
        }

        public int TestIntensity
        {
            get { return testIntensity; }
            set { testIntensity = value; }
        }
    }
}
