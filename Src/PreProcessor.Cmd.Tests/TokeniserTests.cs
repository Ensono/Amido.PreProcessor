using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Amido.PreProcessor.Cmd.Tests
{
    [TestClass]
    public class TokeniserTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplace_NullTemplateArgument()
        {
            IList<string> tokensNotFound;
            var tokeniser = new Tokeniser();
            var text = tokeniser.TryReplace(null, new Dictionary<string, string>(), out tokensNotFound);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplace_EmptyTemplateArgument()
        {
            IList<string> tokensNotFound;
            var tokeniser = new Tokeniser();
            var text = tokeniser.TryReplace(string.Empty, new Dictionary<string, string>(), out tokensNotFound);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplace_NullDictionaryArgument()
        {
            IList<string> tokensNotFound;
            var tokeniser = new Tokeniser();
            var text = tokeniser.TryReplace("some string", null, out tokensNotFound);
        }

        [TestMethod]
        public void TestReplace_StringWithNoTokens()
        {
            var dictionary = new Dictionary<string, string>();
            var tokeniser = new Tokeniser();

            TestReplace(tokeniser, dictionary, "some text.", "some text.");
        }

        [TestMethod]
        public void TestReplace_StringWithOneToken_FoundInDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("tokenName", "tokenised");

            var tokeniser = new Tokeniser();

            TestReplace(tokeniser, dictionary, "some [%tokenName%] text.", "some tokenised text.");
            TestReplace(tokeniser, dictionary, "[%tokenName%]", "tokenised");
        }

        [TestMethod]
        public void TestReplace_StringWithTwoIdenticalTokens_FoundInDictionary()
        { 
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("tokenName", "tokenised");

            var tokeniser = new Tokeniser();

            TestReplace(tokeniser, dictionary, "some [%tokenName%] text [%tokenName%].", "some tokenised text tokenised.");
            TestReplace(tokeniser, dictionary, "some [%tokenName%] [%tokenName%].", "some tokenised tokenised.");
            TestReplace(tokeniser, dictionary, "some [%tokenName%][%tokenName%].", "some tokenisedtokenised.");
            TestReplace(tokeniser, dictionary, "[%tokenName%][%tokenName%]", "tokenisedtokenised");
        }

        [TestMethod]
        public void TestReplace_StringWithTwoTokens_FoundInDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("tokenName1", "tokenised1");
            dictionary.Add("tokenName2", "tokenised2");

            var tokeniser = new Tokeniser();

            TestReplace(tokeniser, dictionary, "some [%tokenName1%] text [%tokenName2%].", "some tokenised1 text tokenised2.");
            TestReplace(tokeniser, dictionary, "[%tokenName1%]", "tokenised1");
        }

        [TestMethod]
        public void TestReplace_StringWithEmbeddedToken_FoundInDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("tokenName", "tokenised");
            dictionary.Add("emdeddedToken", "en");

            var tokeniser = new Tokeniser();

            TestReplace(tokeniser, dictionary, "some [%tok[%emdeddedToken%]Name%] text.", "some tokenised text.");
            TestReplace(tokeniser, dictionary, "some [%tok[%emdeddedToken%]Name%] text [%tokenName%].", "some tokenised text tokenised.");
        }


        [TestMethod]
        public void TestReplace_StringWithMultiplesToken_SomeNotFoundInDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("tokenName1", "tokenised1");
            dictionary.Add("tokenName2", "tokenised2");

            var expectedTokensNotFound = new List<string>();
            expectedTokensNotFound.Add("unknownToken");

            var tokeniser = new Tokeniser();

            TestReplace(tokeniser, dictionary, "[%unknownToken%]", "[%unknownToken%]", expectedTokensNotFound);
            TestReplace(tokeniser, dictionary, "some [%unknownToken%] text.", "some [%unknownToken%] text.", expectedTokensNotFound);
            TestReplace(tokeniser, dictionary, "some [%unknownToken%] text [%tokenName1%].", "some [%unknownToken%] text tokenised1.", expectedTokensNotFound);
            TestReplace(tokeniser, dictionary, "[%tokenName2%] some [%unknownToken%] text [%tokenName1%].", "tokenised2 some [%unknownToken%] text tokenised1.", expectedTokensNotFound);
        }

        private void TestReplace(Tokeniser tokeniser, Dictionary<string, string> dictionary, string template, string expectedResult)
        {
            var expectedTokensNotFound = new List<string>();
            TestReplace(tokeniser, dictionary, template, expectedResult, expectedTokensNotFound);

        }

        private void TestReplace(Tokeniser tokeniser, Dictionary<string, string> dictionary, string template, string expectedResult, IList<string> expectedTokensNotFound)
        {
            IList<string> tokensNotFound;
            var text = tokeniser.TryReplace(template, dictionary, out tokensNotFound);
            Assert.AreEqual(expectedResult, text);
            Assert.AreEqual(expectedTokensNotFound.Count, tokensNotFound.Count);
            for (var i=0; i < expectedTokensNotFound.Count;i++ )
            {
                Assert.AreEqual(expectedTokensNotFound[i], tokensNotFound[i]);
            }
        }
    }

    
}
