using System.Web.Mvc;
using Ecobit_Blockchain_Frontend.Controllers;
using Ecobit_Blockchain_Frontend.Exceptions;
using Ecobit_Blockchain_Frontend.DataAccess.Interfaces;
using Ecobit_Blockchain_Frontend.Models;
using Moq;
using NUnit.Framework;

namespace Ecobit_Blockchain_Frontend_Test.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {

        private UserController _userController;
        private Mock<IUserDao> _mock;
        private User _user;
        private User _user2;

        [SetUp]
        public void Setup()
        {
            _user = new User
            {
                Companyname = "Company",
                Password = "123456",
                Email = "test@ecobit.nl",
                Contact = "Adres in Nederland"
            };
            
            _user2 = new User
            {
                Companyname = _user.Companyname,
                Password = "654321",
                Email = "test@madelief.nl",
                Contact = "Adres in Arnhem"
            };
            
            _mock = new Mock<IUserDao>();
            _mock.Setup(dao => dao.Read(_user.Companyname)).Returns(_user);
            _mock.Setup(dao => dao.Create(_user2)).Throws(new UserException("User already exists"));
            
            _userController = new UserController(_mock.Object);
        }

        [Test]
        public void UpdatePageTest()
        {
            _userController.Update(_user.Companyname);
            
            _mock.Verify(dao => dao.Read(_user.Companyname));
        }

        [Test]
        public void CreateUserTest()
        {
            _userController.Create(_user);
            _mock.Verify(dao => dao.Create(_user));
        }

        [Test]
        public void CreateUserThatExistsTest()
        {
            var result = (ViewResult) _userController.Create(_user2);
            Assert.AreEqual(result.ViewName, "Error");
        }

        [Test]
        public void ReadUserTest()
        {
            _userController.Profile(_user.Companyname);
            _mock.Verify(dao => dao.Read(_user.Companyname));
        }

        [Test]
        public void DeleteUser()
        {
            _userController.Delete(_user.Companyname);
            _mock.Verify(dao => dao.Delete(_user));
        }

        [Test]
        public void UpdateUser()
        {
            _userController.Update(_user2);
            _mock.Verify(dao => dao.Update(_user2));
        }
    }
}