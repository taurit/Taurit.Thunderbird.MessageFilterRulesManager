using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Taurit.Thunderbird.MessageFilterRulesManager.Services;

namespace Taurit.Thunderbird.MessageFilterRulesManager.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void When_ConfigurationFileIsParsedAndThenSerialized_Expect_ContentToBeIdenticalWithOriginal()
        {
            // Arrange
            var parser = new FileParser();
            var writer = new FileWriter();
            var filePath =
                "d:\\ProgramData\\ApplicationData\\Thunderbird\\Profiles\\eaqnr9ie.default\\Mail\\Feeds\\msgFilterRules.dat";
            var originalFileContents = File.ReadAllText(filePath);

            // Act
            var fileModel = parser.Parse(filePath);
            var serializedModel = writer.SerializeModel(fileModel);
            File.WriteAllText("d:\\testxxx.txt", serializedModel);

            // Assert
            Assert.AreEqual(originalFileContents, serializedModel);
        }
    }
}
