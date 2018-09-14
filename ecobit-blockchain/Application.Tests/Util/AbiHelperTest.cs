using Application.Util;
using NUnit.Framework;

namespace Application.Tests.Util
{
    public class AbiHelperTest
    {
        [Test]
        public void ThatTransactionManagerAbiIsParsedCorrectly()
        {
            string abi = AbiHelper.GetTransactionManagerAbi();
            Assert.AreEqual(@"[]",abi);
        }
        
        [Test]
        public void ThatTransactionAbiIsParsedCorrectly()
        {
            string abi = AbiHelper.GetTransactionAbi();
            Assert.AreEqual(@"[]",abi);
        }
    }
}