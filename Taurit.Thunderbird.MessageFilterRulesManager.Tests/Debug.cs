using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Taurit.Thunderbird.MessageFilterRulesManager.Tests
{
    [TestClass]
    public class Debug
    {
        [TestMethod]
        public void ImportRules()
        {
            // import rules
            var fileName = "d:\\ProgramData\\ApplicationData\\Thunderbird\\Profiles\\eaqnr9ie.default\\Mail\\Feeds\\msgFilterRules.dat";
            var file = new FileParser().Parse(fileName);

            // export to excel file
            foreach (var rule in file.Rules)
            {
                //rule.Condition
            }


        }
    }
}
