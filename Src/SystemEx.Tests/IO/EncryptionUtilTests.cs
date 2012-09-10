using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Amido.Common.Utility;
using Amido.SystemEx.IO;
using Amido.SystemEx.Net;
using Amido.Testing.System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Amido.SystemEx.Tests.IO
{
    [TestClass]
    public class EncryptionUtilTests
    {
        [TestInitialize]
        public void Setup()
        {

        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        private const string ciphertext = "S9tsr8LYAduKeMgLr/76UcMx3y8ZbjQlSohHqeRLa4s=";
        private const string plaintext = "This is some plain text";
        private const string passPhrase = "password";

        [TestMethod]
        public void EncryptString_EncryptsStringValue()
        {
            var newciphertext = EncryptionUtil.EncryptString(plaintext, passPhrase);
            Assert.AreEqual(ciphertext, newciphertext);
        }
        
        [TestMethod]
        public void DecryptString_DecryptsStringValue()
        {
            var newplaintext = EncryptionUtil.DecryptString(ciphertext, passPhrase);
            Assert.AreEqual(newplaintext, plaintext);
        }

        [TestMethod]
        public void EncryptThenDecryptIsCorrect()
        {
            var ciphertext = EncryptionUtil.EncryptString(plaintext, passPhrase);
            var newplaintext = EncryptionUtil.DecryptString(ciphertext, passPhrase);
            Assert.AreEqual(plaintext, newplaintext);
        }

        [TestMethod]
        [Ignore]
        public void EncryptUsernameAndPassword()
        {
            var username = "CLEO";
            var password = "CLEO";
            var livePassPhrase = "0nc3 upon a 4 tim3...";
            var saltValue = "E86D599F915616C4271281073862ACD5FD47389685A09256CEC75F513B02D356";
            var ciphertextUsername = EncryptionUtil.EncryptString(username, livePassPhrase, saltValue);
            var ciphertextPassword = EncryptionUtil.EncryptString(password, livePassPhrase, saltValue);
            Debug.WriteLine(saltValue);
            Debug.WriteLine(ciphertextUsername);
            Debug.WriteLine(ciphertextPassword);
        }
    }
}