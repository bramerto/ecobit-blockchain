using System;
using System.IO;
using Application.Util;
using NUnit.Framework;

namespace Application.Tests.Util
{
    public class ConsoleLoggerTest
    {
        [Test]
        public void ThatOutputIsPrintedToConsole()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger =  new ConsoleLogger();
                logger.Log("this should be printed to the console");
                
                Assert.AreEqual("this should be printed to the console" + Environment.NewLine, sw.ToString());
            }
          
        }

    }
}