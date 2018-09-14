using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ecobit_Blockchain_Frontend.Controllers;
using Ecobit_Blockchain_Frontend.Exceptions;
using Ecobit_Blockchain_Frontend.Models;
using Ecobit_Blockchain_Frontend.Utils.Auth;
using Moq;
using NUnit.Framework;

namespace Ecobit_Blockchain_Frontend_Test.Controllers
{
    [TestFixture]
    public class AccountControllerTest
    {
        private AccountController _controller;

        [SetUp]
        public void Init()
        {
            var authMock = new Mock<IAuthenticationService>();
            _controller = new AccountController(authMock.Object);

            //Mock response
            var responseMock = new Mock<HttpResponseBase>(MockBehavior.Strict);
            responseMock
                .Setup(x => x.ApplyAppPathModifier(It.IsAny<string>()))
                .Returns((string url) => url);
            
            var sessionMock = new Mock<HttpSessionStateBase>();
            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context
                .SetupGet(x => x.Response)
                .Returns(responseMock.Object);
            context
                .SetupGet(x => x.Response.Cookies)
                .Returns(new HttpCookieCollection());
            context
                .SetupGet(x => x.Session)
                .Returns(sessionMock.Object);
                
            
            var rc = new RequestContext(context.Object, new RouteData());
            _controller.ControllerContext = new ControllerContext(rc, _controller);

            authMock
                .Setup(mock => mock.Login("admin", "test"))
                .Returns(new HttpCookie(AuthenticationFilterAttribute.CookieName, ""));
            authMock
                .Setup(mock => mock.Login("admin_not_working", "test_not_working"))
                .Throws(new LoginException("Invalid username or password"));
        }

        [Test]
        public void TestIndex()
        {
            //Act
            var result = _controller.Index() as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        [Test]
        public void TestLoginValidCredentials()
        {
            // Arrange
            var model = new Login
            {
                Username = "admin",
                Password = "test"
            };


            //Act
            var result = _controller.Login(model) as RedirectToRouteResult;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Batch", result.RouteValues["controller"]);
        }

        [Test]
        public void TestLoginInValidCredentials()
        {
            // Arrange
            var model = new Login
            {
                Username = "admin_not_working",
                Password = "test_not_working"
            };


            //Act
            var result = _controller.Login(model) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public void TestLoguout()
        {
            //Act
            var result = _controller.LogOut() as RedirectToRouteResult;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Account", result.RouteValues["controller"]);
        }
    }
}