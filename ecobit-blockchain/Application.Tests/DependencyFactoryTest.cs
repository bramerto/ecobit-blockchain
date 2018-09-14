using Application.Util;
using NUnit.Framework;

namespace Application.Tests
{
    public class DependencyFactoryTest
    {
        [Test]
        public void ThatDependencyIsResolved()
        {

            var dependency = DependencyFactory.Resolve<ILogger>();
            
            Assert.IsTrue(dependency.GetType() == typeof(ConsoleLogger));
        }

    }
}