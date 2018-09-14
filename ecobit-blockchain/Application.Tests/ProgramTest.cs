using System;
using Application.Util;
using Moq;
using NUnit.Framework;

namespace Application.Tests
{
    public class ProgramTest
    {
        [Test]
        public void ThatProgramShowsFileNotSpecified()
        {
            var loggerMock = new Mock<ILogger>();

            Program.Main(new string[0], loggerMock.Object);
            
            loggerMock.Verify((m => m.Log("Welcome to the Ecobit Blockchain Connector")));
            loggerMock.Verify((m => m.Log("No existing file specified")));
        }
        
        [Test]
        public void ThatProgramFindsFile()
        {
            var loggerMock = new Mock<ILogger>();

            try
            {
                Program.Main(new[] {AppDomain.CurrentDomain.BaseDirectory + "/../../Util/Xml/TestData1.xml"},
                    loggerMock.Object);
            }
            catch (Exception)
            {
                //Catch null exception in controller we don't want to test that here
            }
            
            loggerMock.Verify((m => m.Log("No existing file specified")), Times.Never());

            loggerMock.Verify((m => m.Log("Welcome to the Ecobit Blockchain Connector")));
        }
    }
}