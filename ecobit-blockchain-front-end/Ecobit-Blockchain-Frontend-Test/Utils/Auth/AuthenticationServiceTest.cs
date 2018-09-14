using System;
using System.Web.Script.Serialization;
using System.Web.Security;
using Ecobit_Blockchain_Frontend.DataAccess.Interfaces;
using Ecobit_Blockchain_Frontend.Exceptions;
using Ecobit_Blockchain_Frontend.Models;
using Ecobit_Blockchain_Frontend.Utils;
using Ecobit_Blockchain_Frontend.Utils.Auth;
using Ecobit_Blockchain_Frontend.Wrappers;
using Moq;
using NUnit.Framework;

namespace Ecobit_Blockchain_Frontend_Test.Utils.Auth
{
    [TestFixture]
    public class AuthenticationServiceTest
    {
        private IAuthenticationService _authService;
        private Mock<IFormsAuthentication> _formsMock;

        [SetUp]
        public void Setup()
        {
            var daoMock = new Mock<IUserDao>();
            _formsMock = new Mock<IFormsAuthentication>();
            _authService = new AuthenticationService(daoMock.Object, _formsMock.Object);

            daoMock
                .Setup(dao => dao.Read("admin"))
                .Returns(new User
                {
                    Companyname = "admin",
                    Password = EncryptionUtil.GenerateHash("test")
                });
            
            daoMock
                .Setup(dao => dao.Read("something"))
                .Returns((User) null);

            _formsMock
                .Setup(mock => mock.Encrypt(It.IsAny<FormsAuthenticationTicket>()))
                .Returns("SomeEncryptedString");
            
            _formsMock
                .Setup(mock => mock.Decrypt(It.IsAny<string>()))
                .Returns(
                    new FormsAuthenticationTicket(
                        1,
                        "admin",
                        DateTime.Now,
                        DateTime.Now.AddYears(1),
                        false,
                        new JavaScriptSerializer().Serialize(new User {Companyname = "admin"})
                    )
                );
        }

        [Test]
        public void TestRightCredentials()
        {
            Assert.IsTrue(_authService.AreCredentialsCorrect("admin", "test"));
        }

        [Test]
        public void TestWrongCredentials()
        {
            Assert.IsFalse(_authService.AreCredentialsCorrect("admin", "123456"));
        }

        [Test]
        public void TestLogin()
        {
            const string username = "admin";
            const string password = "test";

            var cookie = _authService.Login(username, password);
            var userData = _authService.GetUserData(cookie);
            Assert.AreEqual(username, userData.Companyname);
        }

        [Test]
        public void TestUnknownUser()
        {
            const string username = "something";
            const string password = "test";

            Assert.Throws<LoginException>(delegate { _authService.Login(username, password); });
        }

        [Test]
        public void TestInvalidCredentials()
        {
            const string username = "admin";
            const string password = "123456";

            Assert.Throws<LoginException>(delegate { _authService.Login(username, password); });
        }

        [Test]
        public void LogoutTest()
        {
            _authService.Logout();
            _formsMock.Verify(mock => mock.SignOut());
        }
    }
}