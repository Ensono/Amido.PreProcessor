using System;
using System.Collections.Generic;
using System.IO;
using Amido.SystemEx.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Amido.PreProcessor.Cmd.Tests
{
    [TestClass]
    public class TokenisationRunnerTests
    {
        private TokenisationRunner tokenisationRunner;
        private Mock<IFileSystem> mockFSO;
        private Mock<ITokeniser> mockTokeniser;

        private string sourceFile = @"c:\aDir\someFile.xml";
        private string destFile = @"c:\anotherDir\someOtherFile.xml";
        private string destDir = @"c:\anotherDir\";
        private string backupFile = @"c:\anotherDir\someOtherFile.xml.bak";

        [TestInitialize]
        public void TestSetUp()
        {
            mockFSO = new Mock<IFileSystem>(MockBehavior.Strict);
            mockTokeniser = new Mock<ITokeniser>(MockBehavior.Strict);

            this.tokenisationRunner = new TokenisationRunner(mockFSO.Object, mockTokeniser.Object);
        }

        [TestCleanup]
        public void TestTeardown()
        {
            this.mockFSO.VerifyAll();
            this.mockTokeniser.VerifyAll();
        }

        [TestMethod]
        public void TestProcessSingleTemplate_WithNullArgs()
        {
            var properties = new Dictionary<string, string>();
            Xunit.Assert.Throws<ArgumentNullException>(() => this.tokenisationRunner.ProcessSingleTemplate(null, sourceFile, destFile));
            Xunit.Assert.Throws<ArgumentNullException>(() => this.tokenisationRunner.ProcessSingleTemplate(properties, null, destFile));
        }

        [TestMethod]
        public void TestProcessSingleTemplate_WithValidPropertiesWithoutOverrides()
        {
            const string template = "some tokenised <%token1%> text.";
            const string processedTemplate = "some tokenised value1 text.";

            var properties = new Dictionary<string, string>();
            properties.Add("token1", "value1");
            IList<string> tokensNotFound = new List<string>();

            this.mockFSO.Setup(x => x.FileExists(sourceFile)).Returns(true);
            this.mockFSO.Setup(x => x.FileExists(destFile)).Returns(false);
            this.mockFSO.Setup(x => x.ReadAllText(sourceFile)).Returns(template);

            this.mockTokeniser.Setup(x => x.TryReplace(template, properties, out tokensNotFound)).Returns(processedTemplate);

            this.mockFSO.Setup(x => x.GetDirectoryFullPath(destFile)).Returns(destDir);
            this.mockFSO.Setup(x => x.DirectoryExists(destDir)).Returns(true);

            this.mockFSO.Setup(x => x.WriteAllText(destFile, processedTemplate));

            this.tokenisationRunner.ProcessSingleTemplate(properties, sourceFile, destFile);
        }


        [TestMethod]
        public void TestProcessSingleTemplate_WithValidOverrides()
        {
            const string template = "some tokenised <%token1%> text.";
            const string processedTemplate = "some tokenised value1 text.";
            IList<string> tokensNotFound = new List<string>();

            TestProcessSingleTemplate(sourceFile, destDir, destFile, backupFile, template, processedTemplate, tokensNotFound, false, false, true, true);
        }

        [TestMethod]
        public void TestProcessSingleTemplate_WithValidOverrides_DestDirNotFound()
        {
            const string template = "some tokenised <%token1%> text.";
            const string processedTemplate = "some tokenised value1 text.";
            IList<string> tokensNotFound = new List<string>();

            TestProcessSingleTemplate(sourceFile, destDir, destFile, backupFile, template, processedTemplate, tokensNotFound, false, false, false, true);
        }

        [TestMethod]
        public void TestProcessSingleTemplate_UsingConventionForDestinationFilePath()
        {
            sourceFile = @"c:\aDir\someFile.xml.template";
            destFile = null;

            var derivedDestimationFile = @"c:\aDir\someFile.xml";


            destDir = @"c:\aDir\";
            backupFile = @"c:\aDir\someOtherFile.xml.bak";

            const string template = "some tokenised <%token1%> text.";
            const string processedTemplate = "some tokenised value1 text.";

            var properties = new Dictionary<string, string>();
            properties.Add("token1", "value1");
            IList<string> tokensNotFound = new List<string>();

            this.mockFSO.Setup(x => x.FileExists(sourceFile)).Returns(true);

            this.mockFSO.Setup(x => x.FileExists(derivedDestimationFile)).Returns(false);
            this.mockFSO.Setup(x => x.ReadAllText(sourceFile)).Returns(template);

            this.mockTokeniser.Setup(x => x.TryReplace(template, properties, out tokensNotFound)).Returns(processedTemplate);

            this.mockFSO.Setup(x => x.GetDirectoryFullPath(derivedDestimationFile)).Returns(destDir);
            this.mockFSO.Setup(x => x.DirectoryExists(destDir)).Returns(true);

            this.mockFSO.Setup(x => x.WriteAllText(derivedDestimationFile, processedTemplate));

            this.tokenisationRunner.ProcessSingleTemplate(properties, sourceFile, destFile);

        }

        [TestMethod]
        public void TestProcessSingleTemplate_UsingConventionForDestinationFilePath_InvalidSourceFile_ExceptionExpected()
        {
            sourceFile = @"c:\aDir\someFile.xml";
            destFile = null;

            var properties = new Dictionary<string, string>();
            
            this.mockFSO.Setup(x => x.FileExists(sourceFile)).Returns(true);

            Xunit.Assert.Throws<InvalidOperationException>(() => this.tokenisationRunner.ProcessSingleTemplate(properties, sourceFile, destFile));

        }

        [TestMethod]
        public void TestProcessSingleTemplate_WithValidPropertiesWithoutOverrides_MissingToken_ExceptionExpected()
        {
            const string template = "some tokenised <%tokenNotFound1%> <%tokenNotFound2%> text.";
            const string processedTemplate = template;
            IList<string> tokensNotFound = new List<string>() { "tokenNotFound1", "tokenNotFound2" };

            var properties = new Dictionary<string, string>();
            properties.Add("token1", "value1");

            try
            {
                this.mockFSO.Setup(x => x.FileExists(sourceFile)).Returns(true);
                this.mockFSO.Setup(x => x.FileExists(destFile)).Returns(false);

                this.mockFSO.Setup(x => x.ReadAllText(sourceFile)).Returns(template);
                this.mockTokeniser.Setup(x => x.TryReplace(template, properties, out tokensNotFound)).Returns(processedTemplate);

                this.tokenisationRunner.ProcessSingleTemplate(properties, sourceFile, destFile);

            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual(string.Format("Error occurred while processing template {0} and destination file {1}.  Please refer to inner exception for details.", sourceFile, destFile), ex.Message);
                Assert.AreEqual("2 token(s) has not been matched.  Unmatched strings: tokenNotFound1, tokenNotFound2.", ex.InnerException.Message);
            }

        }

        [TestMethod]
        public void TestProcessSingleTemplate_WithTemplateFileNotFound_ExpectException()
        {
            var properties = new Dictionary<string, string>();
            this.mockFSO.Setup(x => x.FileExists(sourceFile)).Returns(false);

            try
            {
                this.tokenisationRunner.ProcessSingleTemplate(properties, sourceFile, destFile);
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual(string.Format("Error occurred while processing template {0} and destination file {1}.  Please refer to inner exception for details.", sourceFile, destFile), ex.Message);
            }
        }

        [TestMethod]
        public void TestProcessSingleTemplate_WithDestinationFileAlreadyExists_BackUpExisits_ExpectBackup()
        {
            const string template = "some tokenised <%tokenNotFound1%> <%tokenNotFound2%> text.";
            const string processedTemplate = template;
            IList<string> tokensNotFound = new List<string>();

            TestProcessSingleTemplate(sourceFile, destDir, destFile, backupFile, template, processedTemplate, tokensNotFound, true, true, true, true);
        }

        [TestMethod]
        public void TestProcessSingleTemplate_WithDestinationFileAlreadyExists_BackDoesNotExisit_ExpectBackup()
        {
            const string template = "some tokenised <%tokenNotFound1%> <%tokenNotFound2%> text.";
            const string processedTemplate = template;
            IList<string> tokensNotFound = new List<string>();

            TestProcessSingleTemplate(sourceFile, destDir, destFile, backupFile, template, processedTemplate, tokensNotFound, true, false, true, true);

        }

        private void TestProcessSingleTemplate(string sourceFile, string destDir, string destFile, string backupFile, string template, string processedTemplate, IList<string> tokensNotFound, bool destinationExists, bool backupExists, bool destDirExists, bool willWriteToDestination)
        {
            var properties = new Dictionary<string, string>();
            properties.Add("token1", "value1");

            this.SetUpMocks(sourceFile, destDir, destFile, backupFile, template, processedTemplate, properties, tokensNotFound, destinationExists, backupExists, destDirExists, willWriteToDestination);

            this.tokenisationRunner.ProcessSingleTemplate(properties, sourceFile, destFile);
        }

        private void SetUpMocks(string sourceFile, string destDir, string destFile, string backupFile, string template, string processedTemplate, IDictionary<string, string> properties, IList<string> tokensNotFound, bool destinationExists, bool backupExists, bool destDirExists, bool willWriteToDestination)
        {
            this.mockFSO.Setup(x => x.FileExists(sourceFile)).Returns(true);
            this.mockFSO.Setup(x => x.FileExists(destFile)).Returns(destinationExists);

            if (destinationExists)
            {
                this.mockFSO.Setup(x => x.FileExists(backupFile)).Returns(backupExists);
                if (backupExists)
                {
                    this.mockFSO.Setup(x => x.DeleteFile(backupFile));
                }
                this.mockFSO.Setup(x => x.CopyFile(destFile, backupFile));
                this.mockFSO.Setup(x => x.DeleteFile(destFile));
            }

            this.mockFSO.Setup(x => x.ReadAllText(sourceFile)).Returns(template);
            this.mockTokeniser.Setup(x => x.TryReplace(template, properties, out tokensNotFound)).Returns(processedTemplate);

            this.mockFSO.Setup(x => x.GetDirectoryFullPath(destFile)).Returns(destDir);
            this.mockFSO.Setup(x => x.DirectoryExists(destDir)).Returns(destDirExists);
            if (!destDirExists)
            {
                this.mockFSO.Setup(x => x.CreateDirectory(destDir));
            }

            if (willWriteToDestination)
            {
                this.mockFSO.Setup(x => x.WriteAllText(destFile, processedTemplate));
            }

        }

    }
}
