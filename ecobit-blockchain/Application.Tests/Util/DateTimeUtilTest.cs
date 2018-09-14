using System;
using Application.Util;
using NUnit.Framework;

namespace Application.Tests.Util
{
    public class DateTimeUtilTest
    {
     
        [Test]
        public void ThatDateTimeIsConvertedToTimeStamp()
        {
            var date = DateTimeUtil.ConvertToTimestamp(new DateTime(2018, 10, 1));
            Assert.AreEqual(1538352000, date);
        }
        
        [Test]
        public void ThatNullDateTimeIsConvertedToTimeStamp()
        {
            var date = DateTimeUtil.ConvertToTimestamp(null);
            Assert.AreEqual(0, date);
        }
    }
}