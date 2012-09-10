using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Amido.SystemEx.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Amido.PreProcessor.Cmd.Tests
{
    [Ignore]
    [TestClass]
    public class PreProcessRunnerTests
    {
        private PreProcessRunner preProcessorRunner;
        private Mock<IFileSystem> mockFSO;
        private Mock<IPropertyManager> mockPropertyManager;
        private Mock<ITokenisationRunner> mockTokeniser;

        private const string sourceFile = "someFile.xml";
        private const string destFile = "someOtherFile.xml";
        private const string propertiesFile = "somePropertiesFile.xml";
        private const string overridesFile = "someOverridesFile.xml";
        private const string manifestFile = "someManifestFile.xml";

        private string manifestXmlMask = "<PreProcessorManifest>"
                                        + "<Groups>"
                                               + "{0}"
                                        + "</Groups>"
                                + "</PreProcessorManifest>";

        private string groupXmlMask = "<Group Name=\"{1}\">"
                                        + "<Commands>"
                                               + "{0}"
                                        + "</Commands>"
                                + "</Group>";

        private string commandXmlMask = "<Command Source=\"{0}\" Destination=\"{1}\"/>";

        [TestInitialize]
        public void TestSetUp()
        {
            mockFSO = new Mock<IFileSystem>(MockBehavior.Strict);
            mockPropertyManager = new Mock<IPropertyManager>(MockBehavior.Strict);
            mockTokeniser = new Mock<ITokenisationRunner>(MockBehavior.Strict);

            this.preProcessorRunner = new PreProcessRunner(mockFSO.Object, mockPropertyManager.Object, mockTokeniser.Object);
        }

        [TestCleanup]
        public void TestTeardown()
        {
            this.mockFSO.VerifyAll();
            this.mockPropertyManager.VerifyAll();
            this.mockTokeniser.VerifyAll();
        }

        [TestMethod]
        public void TestRun_WithNullArgs_ExpectException()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() => this.preProcessorRunner.Run(null));
        }

        [TestMethod]
        public void TestRun_WithInvalidArgs_ExpectException()
        {
            var args = new string[] { "/SomeArg=SomeValue" };
            Xunit.Assert.Throws<ArgumentException>(() => this.preProcessorRunner.Run(args));

            args = new string[] { "/SomeArg=SomeValue", "/SourceFile=someFile.xml", "/StaticPropertiesFile=somePropertiesFile.xml" };
            Xunit.Assert.Throws<ArgumentException>(() => this.preProcessorRunner.Run(args));

            args = new string[] { "/SomeArg=SomeValue", "/SourceFile=someFile.xml", "/StaticPropertiesFile=somePropertiesFile.xml", "/OverridesFile=someOverridesFile.xml" };
            Xunit.Assert.Throws<ArgumentException>(() => this.preProcessorRunner.Run(args));

            args = new string[] { "/SourceFile=someFile.xml", "/StaticPropertiesFile=somePropertiesFile.xml" };
            Xunit.Assert.Throws<ArgumentException>(() => this.preProcessorRunner.Run(args));

            args = new string[] { "/DestinationFile=someOtherFile.xml", "/StaticPropertiesFile=somePropertiesFile.xml" };
            Xunit.Assert.Throws<ArgumentException>(() => this.preProcessorRunner.Run(args));

            args = new string[] { "/SourceFile=someFile.xml", "/StaticPropertiesFile=somePropertiesFile.xml", "/ManifestFile=someManifestFile.xml" };
            Xunit.Assert.Throws<ArgumentException>(() => this.preProcessorRunner.Run(args));

            args = new string[] { "/DestinationFile=someOtherFile.xml", "/StaticPropertiesFile=somePropertiesFile.xml", "/ManifestFile=someManifestFile.xml" };
            Xunit.Assert.Throws<ArgumentException>(() => this.preProcessorRunner.Run(args));

            args = new string[] { "/SourceFile=someFile.xml", "/DestinationFile=someOtherFile.xml", "/StaticPropertiesFile=somePropertiesFile.xml", "/ManifestFile=someManifestFile.xml" };
            Xunit.Assert.Throws<ArgumentException>(() => this.preProcessorRunner.Run(args));
        }


        [TestMethod]
        public void TestRun_SingleRun_WithValidPropertiesWithoutOverrides()
        {
            IDictionary<string, string> properties = new Dictionary<string, string>() { { "token1", "value1" } };
            
            this.mockPropertyManager.Setup(x => x.LoadProperties(propertiesFile, null)).Returns(properties);

            this.mockTokeniser.Setup(x => x.ProcessSingleTemplate(properties, sourceFile, destFile));
           
            var args = new string[] { "/SourceFile=" + sourceFile, "/StaticPropertiesFile=" + propertiesFile, "/DestinationFile=" + destFile };

            this.preProcessorRunner.Run(args);
        }

       
        [TestMethod]
        public void TestRun_SingleRun__WithValidOverrides()
        {
            IDictionary<string, string> properties = new Dictionary<string, string>() { { "token1", "value1" } };

            this.SetUpMocks(propertiesFile, overridesFile, sourceFile, destFile, properties);

            var args = new string[] { "/SourceFile=" + sourceFile, "/DestinationFile=" + destFile, "/StaticPropertiesFile=" + propertiesFile, "/OverridesFile=" + overridesFile };
            
            this.preProcessorRunner.Run(args);
        }

        private void SetUpMocks(string propertiesFile, string overridesFile, string sourceFile, string destFile, IDictionary<string, string> properties)
        {
            this.mockPropertyManager.Setup(x => x.LoadProperties(propertiesFile, overridesFile)).Returns(properties);

            this.mockTokeniser.Setup(x => x.ProcessSingleTemplate(properties, sourceFile, destFile));
        }

        private string GetManifestXml(IList<string> groups)
        {
            StringBuilder groupsXml = new StringBuilder();
            foreach (var item in groups)
            {
                groupsXml.AppendLine(item);
            }

            return string.Format(manifestXmlMask, groupsXml.ToString());
        }

        private string GetGroupXml(string name, IDictionary<string, string> commands)
        {
            StringBuilder commandsXml = new StringBuilder();
            foreach (var item in commands)
            {
                commandsXml.AppendLine(string.Format(commandXmlMask, item.Key, item.Value));
            }

            return string.Format(groupXmlMask, commandsXml.ToString(), name);
        }
    }
}
