using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Amido.SystemEx.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Amido.PreProcessor.Cmd.Tests
{
    [TestClass]
    public class PropertyManagerTests
    {
        private PropertyManager propertyManager;
        private Mock<IFileSystem> mockFSO;

        private string propertyConfigXmlMask = "<PreProcessorConfig>"
                                        + "<Properties>"
                                               + "{0}"
                                        + "</Properties>"
                                + "</PreProcessorConfig>";

        private string propertyMask = "<Property Name=\"{0}\" Value=\"{1}\"/>";

        [TestInitialize]
        public void TestSetUp()
        {
            this.mockFSO = new Mock<IFileSystem>(MockBehavior.Strict);
      
            this.propertyManager = new PropertyManager(this.mockFSO.Object);
        }

        [TestCleanup]
        public void TestTeardown()
        {
            this.mockFSO.VerifyAll();

        }

        [TestMethod]
        public void TestLoadProperties_WithNullArgs()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() => this.propertyManager.LoadProperties(null, null));
        }

        [TestMethod]
        public void TestLoadProperties_PropertiesFileNotFound_ExceptionExpected()
        {
            var propertiesFileName = "somePropertiesFile.xml";

            this.mockFSO.Setup(x => x.FileExists(propertiesFileName)).Returns(false);
            Xunit.Assert.Throws<FileNotFoundException>(() => this.propertyManager.LoadProperties(propertiesFileName, null));
        }


        [TestMethod]
        public void TestLoadProperties_ZeroOrMorePropertyWithoutDynamicProperties()
        {
            IDictionary<string, string> staticPropertiesDictionary = new Dictionary<string, string>();

            this.TestLoadProperties(staticPropertiesDictionary, null, staticPropertiesDictionary);

            staticPropertiesDictionary.Add("tokenName1", "tokenValue1");
            this.TestLoadProperties(staticPropertiesDictionary, null, staticPropertiesDictionary);

            staticPropertiesDictionary.Add("tokenName2", "duplicateTokenValue");
            this.TestLoadProperties(staticPropertiesDictionary, null, staticPropertiesDictionary);

            staticPropertiesDictionary.Add("tokenName3", "duplicateTokenValue");
            this.TestLoadProperties(staticPropertiesDictionary, null, staticPropertiesDictionary);
        }

    
        [TestMethod]
        public void TestLoadProperties_DuplicatePropertyKeys_ExceptionExpected()
        {
            StringBuilder propertiesConfig = new StringBuilder();
            propertiesConfig.AppendLine(string.Format(this.propertyMask, "tokenName1", "tokenValue1"));
            propertiesConfig.AppendLine(string.Format(this.propertyMask, "tokenName1", "tokenValue2"));

            var xml = string.Format(this.propertyConfigXmlMask, propertiesConfig.ToString());
            Xunit.Assert.Throws<InvalidOperationException>(() => this.ExecTestLoadProperties(xml, null));

            xml = string.Format(this.propertyConfigXmlMask, string.Format(this.propertyMask, "computer.name", "someComputer"));
            Xunit.Assert.Throws<InvalidOperationException>(() => this.ExecTestLoadProperties(xml, null));

        }

        [TestMethod]
        public void TestLoadProperties_DuplicateOverrideKeys_ExceptionExpected()
        {
            StringBuilder propertiesConfig = new StringBuilder();
            propertiesConfig.AppendLine(string.Format(this.propertyMask, "tokenName1", "tokenValue1"));
            var propertiesXml = string.Format(this.propertyConfigXmlMask, propertiesConfig.ToString());

            StringBuilder overrideConfig = new StringBuilder();
            overrideConfig.AppendLine(string.Format(this.propertyMask, "tokenName1", "overrideTokenValue1"));
            overrideConfig.AppendLine(string.Format(this.propertyMask, "tokenName1", "overrideTokenValue2"));

            var overrideXml = string.Format(this.propertyConfigXmlMask, overrideConfig.ToString());

            Xunit.Assert.Throws<InvalidOperationException>(() => this.ExecTestLoadPropertiesAndOverrides(propertiesXml, overrideXml));

        }

        [TestMethod]
        public void TestLoadProperties_OverrideWithOZeroOverridesWithoutDynamicProperties()
        {
            IDictionary<string, string> propertiesDictionary = new Dictionary<string, string>();
            IDictionary<string, string> emptyDictionary = new Dictionary<string, string>();

            this.TestLoadPropertiesAndOverrides(propertiesDictionary, emptyDictionary, propertiesDictionary);

            propertiesDictionary.Add("tokenName1", "tokenValue1");
            this.TestLoadPropertiesAndOverrides(propertiesDictionary, emptyDictionary, propertiesDictionary);

            propertiesDictionary.Add("tokenName2", "duplicateTokenValue");
            this.TestLoadPropertiesAndOverrides(propertiesDictionary, emptyDictionary, propertiesDictionary);

            propertiesDictionary.Add("tokenName3", "duplicateTokenValue");
            this.TestLoadPropertiesAndOverrides(propertiesDictionary, emptyDictionary, propertiesDictionary);
        }

        [TestMethod]
        public void TestLoadProperties_OverrideWithOneorMoreOverridesWithoutDynamicProperties()
        {
            IDictionary<string, string> propertiesDictionary = new Dictionary<string, string>();
            IDictionary<string, string> overridesDictionary = new Dictionary<string, string>();
            IDictionary<string, string> expectedPropertiesDictionary = new Dictionary<string, string>();
            

            propertiesDictionary.Add("tokenName1", "tokenValue1");
            propertiesDictionary.Add("tokenName2", "duplicateTokenValue");
            propertiesDictionary.Add("tokenName3", "duplicateTokenValue");
            expectedPropertiesDictionary = MergeDictionaries(propertiesDictionary, overridesDictionary);
            this.TestLoadPropertiesAndOverrides(propertiesDictionary, overridesDictionary, expectedPropertiesDictionary);

            overridesDictionary.Add("tokenName1", "overrideTokenValue1");
            expectedPropertiesDictionary = MergeDictionaries(propertiesDictionary, overridesDictionary);
            this.TestLoadPropertiesAndOverrides(propertiesDictionary, overridesDictionary, expectedPropertiesDictionary);

            overridesDictionary.Add("tokenName2", "overrideTokenValue");
            expectedPropertiesDictionary = MergeDictionaries(propertiesDictionary, overridesDictionary);
            this.TestLoadPropertiesAndOverrides(propertiesDictionary, overridesDictionary, expectedPropertiesDictionary);

            overridesDictionary.Add("tokenName3", "overrideTokenValue");
            expectedPropertiesDictionary = MergeDictionaries(propertiesDictionary, overridesDictionary);
            this.TestLoadPropertiesAndOverrides(propertiesDictionary, overridesDictionary, expectedPropertiesDictionary);
        }


        private IDictionary<string, string> ExecTestLoadProperties(string staticPropertiesXml, string dynamicProperties)
        {
            var propertiesFileName = "somePropertiesFile.xml";

            using (var stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(staticPropertiesXml)))
            {
                this.mockFSO.Setup(x => x.FileExists(propertiesFileName)).Returns(true);
                this.mockFSO.Setup(x => x.OpenFile(propertiesFileName, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(stream);

                return this.propertyManager.LoadProperties(propertiesFileName, dynamicProperties);
            }
        }

        private IDictionary<string, string> ExecTestLoadPropertiesAndOverrides(string staticPropertiesXml, string overridesXml)
        {
            var propertiesFileName = "somePropertiesFile.xml";
            var overridesFileName = "someOverridesFile.xml";

            using (var propertiesStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(staticPropertiesXml)))
            {
                this.mockFSO.Setup(x => x.FileExists(propertiesFileName)).Returns(true);
                this.mockFSO.Setup(x => x.OpenFile(propertiesFileName, FileMode.Open, FileAccess.Read, FileShare.Read)).
                    Returns(propertiesStream);

                using (var overridesStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(overridesXml)))
                {
                    this.mockFSO.Setup(x => x.FileExists(overridesFileName)).Returns(true);
                    this.mockFSO.Setup(
                        x => x.OpenFile(overridesFileName, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(
                            overridesStream);

                    return this.propertyManager.LoadProperties(propertiesFileName, overridesFileName);
                }
            }
        }

        private void TestLoadPropertiesAndOverrides(IDictionary<string, string> staticPropertiesDictionary, IDictionary<string, string> overrideDictionary, IDictionary<string, string> expectedPropertiesDictionary)
        {
            var propertiesXml = this.GetXml(staticPropertiesDictionary);
            var overrideXml = this.GetXml(overrideDictionary);

            var actualProperties = this.ExecTestLoadPropertiesAndOverrides(propertiesXml, overrideXml);

            this.AssertProperties(expectedPropertiesDictionary, actualProperties);
        }

        private void TestLoadProperties(IDictionary<string, string> staticPropertiesDictionary, string dynamicProperties, IDictionary<string, string> expectedPropertiesDictionary)
        {
            var propertiesXml = this.GetXml(staticPropertiesDictionary);
            
            var actualProperties = this.ExecTestLoadProperties(propertiesXml, dynamicProperties);

            this.AssertProperties(expectedPropertiesDictionary, actualProperties);
        }

        private void AssertProperties(IDictionary<string, string> expectedProperties, IDictionary<string, string> actualProperties)
        {
            foreach(var item in expectedProperties)
            {
                Assert.IsTrue(actualProperties.ContainsKey(item.Key));
                Assert.AreEqual(item.Value, actualProperties[item.Key]);
                    
            }

            Assert.IsTrue(actualProperties.ContainsKey("computer.name"));
            Assert.AreEqual(Environment.MachineName, actualProperties["computer.name"]);
        }

        private string GetXml(IDictionary<string, string> dictionary)
        {
            StringBuilder propertiesConfig = new StringBuilder();
            foreach(var item in dictionary)
            {
                propertiesConfig.AppendLine(string.Format(propertyMask, item.Key, item.Value));
            }

            return string.Format(propertyConfigXmlMask, propertiesConfig.ToString());
           
        }

        private IDictionary<string, string> MergeDictionaries(IDictionary<string, string> propertiesDictionary, IDictionary<string, string> overrideDictionary)
        {
            IDictionary<string, string> mergeDictionary = new Dictionary<string, string>(overrideDictionary);
            foreach (var item in propertiesDictionary)
            {
                if (!mergeDictionary.ContainsKey(item.Key))
                {
                    mergeDictionary.Add(item.Key, item.Value);    
                }
                
            }

            return mergeDictionary;
        }
    }
}
