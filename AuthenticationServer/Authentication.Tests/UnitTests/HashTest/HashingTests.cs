using System;
using NUnit.Framework;

namespace Dijits.Authentication.Tests.UnitTests.HashTest
{
    [TestFixture]
    public class HashingTests
    {
        private string _secretKey = string.Empty;

        [SetUp]
        public void SetUpTest()
        {

        }

        /// <summary>
        /// The test text hashing.
        /// </summary>
        [Test]
        public void TestNonConfidentialTextHashing()
        {
            this._secretKey = Helper.Helper.GetHash("SajjadSofia");
            if (_secretKey == null)
            {
                Assert.Fail();
            }

            Console.WriteLine(_secretKey);
        }

        /// <summary>
        /// The test text hashing.
        /// </summary>
        [Test]
        public void TestConfidentialTextHashing()
        {
            _secretKey = Helper.Helper.GetHash("RahimZaraRaees");
            if (_secretKey == null)
            {
                Assert.Fail();
            }

            Console.WriteLine(_secretKey);
        }
    }
}
