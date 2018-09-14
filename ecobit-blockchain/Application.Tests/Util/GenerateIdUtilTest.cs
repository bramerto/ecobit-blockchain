using Application.Util;
using NUnit.Framework;

namespace Application.Tests.Util
{
    [TestFixture]
    public class GenerateIdUtilTest
    {
        [Test]
        public void ShouldProduceAString()
        {
            var id = GenerateIdUtil.GenerateUniqueId();
            
            Assert.IsInstanceOf<string>(id);
        }

        [Test]
        public void ShouldProduceAStringOf36Length()
        {
            var id = GenerateIdUtil.GenerateUniqueId();
            
            Assert.AreEqual(36, id.Length);
        }

        [Test]
        public void ShouldProduceAStringOnlyContainingCertainCharacters()
        {
            var id = GenerateIdUtil.GenerateUniqueId();
            
            StringAssert.IsMatch("^[a-fA-F0-9\\-]+$", id);
        }
    }
}