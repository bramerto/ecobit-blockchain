using System.Numerics;
using System.Threading.Tasks;
using Ecobit_Blockchain_Frontend.Exceptions;
using Ecobit_Blockchain_Frontend.DataAccess.Interfaces;
using Ecobit_Blockchain_Frontend.Models;
using Ecobit_Blockchain_Frontend.Utils;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Ecobit_Blockchain_Frontend.DataAccess
{
    public class EthereumUserDao : IUserDao
    {
        private readonly int gas = 1000000;
        private readonly int gasPrice = 0;
        
        public dynamic Contract { get; set; }

        public EthereumUserDao()
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["ethUrl"];

            var webNode = url == null ? new Web3() : new Web3(url); 

            webNode.TransactionManager.DefaultGasPrice = new BigInteger(0);
            Contract = webNode.Eth.GetContract(AbiHelper.GetEcobitBlockchainAbi(), System.Configuration.ConfigurationManager.AppSettings["transactionManagerAddress"]);
        }
        
        public void Create(User user)
        {
            if (UserExists(user.Companyname))
            {
                throw new UserException("User already exists");             
            }

            user.Password = EncryptionUtil.GenerateHash(user.Password);
            
            dynamic function = Contract.GetFunction("addUser");
            
            Task<string> task = function.SendTransactionAsync(
                System.Configuration.ConfigurationManager.AppSettings["accountAddress"],
                new HexBigInteger(gas),
                new HexBigInteger(gasPrice), 
                user.Companyname,
                user.Password,
                user.Email,
                user.Contact);

            task.ContinueWith(response => response.Result);

            task.Wait();
        }

        public User Read(string companyName)
        {
            User user = null;
            
            if (UserExists(companyName))
            {
                dynamic function = Contract.GetFunction("getUser");

                Task<User> task = function.CallDeserializingToObjectAsync<User>(companyName);

                user = task.ContinueWith(response => response.Result).Result;

                task.Wait();
            }

            return user;
        }

        public void Update(User user)
        {
            UpdateUserContact(user);
            UpdateUserEmail(user);
            UpdateUserPassword(user);
        }

        private void UpdateUserEmail(User user)
        {
            dynamic function = Contract.GetFunction("updateUserEmail");
            
            Task<string> task = function.SendTransactionAsync(
                System.Configuration.ConfigurationManager.AppSettings["accountAddress"],
                new HexBigInteger(gas),
                new HexBigInteger(gasPrice), 
                user.Companyname,
                user.Email);

            task.ContinueWith(response => response.Result);

            task.Wait();
        }
        
        private void UpdateUserContact(User user)
        {
            dynamic function = Contract.GetFunction("updateUserContact");
            
            Task<string> task = function.SendTransactionAsync(
                System.Configuration.ConfigurationManager.AppSettings["accountAddress"],
                new HexBigInteger(gas),
                new HexBigInteger(gasPrice), 
                user.Companyname,
                user.Contact);

            task.ContinueWith(response => response.Result);

            task.Wait();
        }
        
        private void UpdateUserPassword(User user)
        {
            dynamic function = Contract.GetFunction("updateUserPassword");

            if (!user.Password.StartsWith("$2a$12$"))
            {
                user.Password = EncryptionUtil.GenerateHash(user.Password);
            }
            
            Task<string> task = function.SendTransactionAsync(
                System.Configuration.ConfigurationManager.AppSettings["accountAddress"],
                new HexBigInteger(gas),
                new HexBigInteger(gasPrice), 
                user.Companyname,
                user.Password);

            task.ContinueWith(response => response.Result);

            task.Wait();
        }

        public void Delete(User user)
        {
            dynamic function = Contract.GetFunction("removeUser");
            
            Task<string> task = function.SendTransactionAsync(
                System.Configuration.ConfigurationManager.AppSettings["accountAddress"],
                new HexBigInteger(gas),
                new HexBigInteger(gasPrice), 
                user.Companyname);

            task.ContinueWith(response => response.Result);

            task.Wait();
        }

        public bool UserExists(string companyName)
        {
            dynamic function = Contract.GetFunction("doesUserExist");
            
            Task<bool> task = function.CallAsync<bool>(companyName);

            bool result = task.ContinueWith(response => response.Result).Result;
            
            task.Wait();
            return result;
        }
    }
}