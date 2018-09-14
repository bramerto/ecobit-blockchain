using System;
using System.Threading.Tasks;
using Ecobit_Blockchain_Frontend.DataAccess;
using Ecobit_Blockchain_Frontend.Exceptions;
using Ecobit_Blockchain_Frontend.DataAccess.NethereumAbstraction;
using Ecobit_Blockchain_Frontend.Models;
using Ecobit_Blockchain_Frontend.Utils;
using Moq;
using Nethereum.Hex.HexTypes;
using NUnit.Framework;

namespace Ecobit_Blockchain_Frontend_Test.DataAccess
{
    [TestFixture]
    public class EthereumUserDaoTest
    {
        private EthereumUserDao _dao;

        private Mock<IFunction> _addUserMock;
        private Mock<IFunction> _getUserMock;
        private Mock<IFunction> _updateUserEmailMock;
        private Mock<IFunction> _updateUserContactMock;
        private Mock<IFunction> _updateUserPasswordMock;
        private Mock<IFunction> _removeUserMock;
        private Mock<IFunction> _userExistsMock;
        
        [SetUp]
        public void Init()
        {
            _dao = new EthereumUserDao();

            var contractMock = new Mock<IContract>();
            
            _dao.Contract = contractMock.Object;

            _addUserMock = new Mock<IFunction>();
            contractMock.Setup(m => m.GetFunction("addUser")).Returns(_addUserMock.Object);
            
            _getUserMock = new Mock<IFunction>();
            contractMock.Setup(m => m.GetFunction("getUser")).Returns(_getUserMock.Object);
            
            _updateUserEmailMock = new Mock<IFunction>();
            contractMock.Setup(m => m.GetFunction("updateUserEmail")).Returns(_updateUserEmailMock.Object);
            
            _updateUserContactMock = new Mock<IFunction>();
            contractMock.Setup(m => m.GetFunction("updateUserContact")).Returns(_updateUserContactMock.Object);
            
            _updateUserPasswordMock = new Mock<IFunction>();
            contractMock.Setup(m => m.GetFunction("updateUserPassword")).Returns(_updateUserPasswordMock.Object);
            
            _removeUserMock = new Mock<IFunction>();
            contractMock.Setup(m => m.GetFunction("removeUser")).Returns(_removeUserMock.Object);

            _userExistsMock = new Mock<IFunction>();
            contractMock.Setup(m => m.GetFunction("doesUserExist")).Returns(_userExistsMock.Object);
        }

        [Test]
        public void ThatUserIsAdded()
        {
            _userExistsMock.Setup(m => m.CallAsync<Boolean>()).Returns(Task.FromResult(false));
            
            var user = new User();

            user.Companyname = "My company";
            user.Password = "password";
            user.Email = "email";
            user.Contact = "contact";

            _dao.Create(user);
            
            _addUserMock.Verify(m => m.SendTransactionAsync(It.IsAny<string>(),
                It.IsAny<HexBigInteger>(),
                It.IsAny<HexBigInteger>(),
                "My company",
                It.IsAny<string>(),
                "email",
                "contact"
                ));
            
            Assert.IsTrue(EncryptionUtil.Verify("password", user.Password));
        }

        [Test]
        public void ThatExceptionIsThrownWhenUserExists()
        {
            _userExistsMock.Setup(m => m.CallAsync<bool>(It.IsAny<string>())).Returns(Task.FromResult(true));
            Assert.Throws<UserException>(delegate { _dao.Create(new User()); });
        }

        [Test]
        public void ThatUserCanBeRead()
        {
            var user = new User();
            
            _userExistsMock.Setup(m => m.CallAsync<bool>(It.IsAny<string>())).Returns(Task.FromResult(true));
            _getUserMock.Setup(m => m.CallDeserializingToObjectAsync<User>("test company")).Returns(Task.FromResult(user));

            var userFromDao = _dao.Read("test company");

            Assert.AreSame(user, userFromDao);
        }

        [Test]
        public void ThatUserIsUpdated()
        {
            var user = new User();

            user.Companyname = "My company";
            user.Password = "password";
            user.Email = "email";
            user.Contact = "contact";

            _dao.Update(user);
            
            _updateUserContactMock.Verify(m => m.SendTransactionAsync(It.IsAny<string>(),
                It.IsAny<HexBigInteger>(),
                It.IsAny<HexBigInteger>(),
                "My company",
                "contact"
            ));
            
            _updateUserEmailMock.Verify(m => m.SendTransactionAsync(It.IsAny<string>(),
                It.IsAny<HexBigInteger>(),
                It.IsAny<HexBigInteger>(),
                "My company",
                "email"
            ));
            
            _updateUserPasswordMock.Verify(m => m.SendTransactionAsync(It.IsAny<string>(),
                It.IsAny<HexBigInteger>(),
                It.IsAny<HexBigInteger>(),
                "My company",
                It.IsAny<string>()
            ));
            
            Assert.IsTrue(EncryptionUtil.Verify("password", user.Password));
        }

        [Test]
        public void ThatUserIsDeleted()
        {
            var user = new User();
            user.Companyname = "My company";
            user.Password = "password";
            user.Email = "email";
            user.Contact = "contact";

            _dao.Delete(user);
            
            _removeUserMock.Verify(m => m.SendTransactionAsync(It.IsAny<string>(),
                It.IsAny<HexBigInteger>(),
                It.IsAny<HexBigInteger>(),
                "My company"
            ));
        }

    }
}