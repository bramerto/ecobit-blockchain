using Ecobit_Blockchain_Frontend.Utils;
using NUnit.Framework;

namespace Ecobit_Blockchain_Frontend_Test.Utils
{
    [TestFixture]
    public class EncryptionUtilTest
    {

        [Test]
        public void TestCorrectPassword()
        {
            var hash = EncryptionUtil.GenerateHash("123456");
            Assert.IsTrue(EncryptionUtil.Verify("123456", hash));
        }

        [Test]
        public void TestWrongPassword()
        {
            var hash = EncryptionUtil.GenerateHash("123456");
            Assert.IsFalse(EncryptionUtil.Verify("12345", hash));
        }
        
    }
}