using System;
using Ecobit_Blockchain_Frontend.Utils;
using NUnit.Framework;

namespace Ecobit_Blockchain_Frontend_Test.Utils
{
    [TestFixture]
    public class DateTimeUtilTest
    {
        [Test]
        public void ShouldConvertIntToDate1()
        {
            const int input = 1528103808;
            var expected = new DateTime(2018,06,04,09,16,48, DateTimeKind.Utc); 
            var actual = DateTimeUtil.ConvertToDate(input);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void ShouldConvertIntToDate2()
        {
            const int input = 1524375045;
            var expected = new DateTime(2018,04,22,05,30,45, DateTimeKind.Utc); 
            var actual = DateTimeUtil.ConvertToDate(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldConvertOneSecondAfterEpoch()
        {
            const int input = 1;
            var expected = new DateTime(1970,01,01,00,00,1, DateTimeKind.Utc);
            var actual = DateTimeUtil.ConvertToDate(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NullableShouldConvertZeroToNull()
        {
            const int input = 0;
            var actual = DateTimeUtil.ConvertToNullableDate(input);
            Assert.Null(actual);
        }

        [Test]
        public void NullableShouldNotConvertNonZeroToNull()
        {
            const int input = 1524204540;
            var expected = new DateTime(2018,04,20,06,09,00, DateTimeKind.Utc); 
            var actual = DateTimeUtil.ConvertToDate(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldConvertToTimestamp1()
        {
            var input = new DateTime(2018,06,04,09,16,48, DateTimeKind.Utc);
            var expected = 1528103808;
            var actual = DateTimeUtil.ConvertToTimestamp(input);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void ShouldConvertToTimestamp2()
        {
            var input = new DateTime(2018,04,22,05,30,45, DateTimeKind.Utc);
            var expected = 1524375045;
            var actual = DateTimeUtil.ConvertToTimestamp(input);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void ShouldReturnZeroIfDateHasNoValue()
        {
            var expected = 0;
            var actual = DateTimeUtil.ConvertToTimestamp(null);
            Assert.AreEqual(expected, actual);
        }
    }
}