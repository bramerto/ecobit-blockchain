pragma solidity ^0.4.23;

import "./Transaction.sol";

contract EcobitBlockchain {
    mapping (uint => address[]) transactions;
    mapping (string => address[]) userTransactions;
    mapping (string => User) users;
    mapping (string => Transaction) transactionids;


    /** User **/
    struct User {
        string companyName;
        string password;
        string email;
        string contact;
    }
    
    uint public userCount = 0;

    function addUser(string companyName, string password, string email, string contact) public {
        if (doesUserExist(companyName)) {
            revert();
        } 
        users[companyName] = User(companyName, password, email, contact);
        userCount++;
    }

    function getUsersCount() public view returns(uint) {
        return userCount;
    }

    function getUserTransactions(string companyName) public view returns (address[]) {
        return userTransactions[companyName];
    }

    function getUserTransactionsLength(string companyName) public view returns (uint) {
        return userTransactions[companyName].length;
    }

    function getUser(string companyName) public view returns(string, string, string, string) {
        return (users[companyName].companyName, users[companyName].password,users[companyName].email, users[companyName].contact);
    }

    function removeUser(string index) public {
        delete users[index];
        userCount = userCount - 1;
    }

    function doesUserExist(string companyName) public view returns (bool) {
        if(userCount == 0) return false;
        return ( keccak256(users[companyName].companyName) == keccak256(companyName));
    }

    function updateUserEmail(string companyName, string email) public {
        users[companyName].email = email;
    }

    function updateUserContact(string companyName, string contact) public {
        users[companyName].contact = contact;
    }

    function updateUserPassword(string companyName, string password) public {
        users[companyName].password = password;
    }

    /** Transaction **/

    function getTransactions(uint batchid) public view returns (address[]) {
        return transactions[batchid];
    }

    function getTransaction(string transactionId) public view returns (Transaction) {
        return transactionids[transactionId];
    }

    function addTransaction(
        uint batchid,
        string transactionid,
        uint quantity,
        uint item_price,
        uint order_date,
        string fromOwner,
        string toOwner
    ) public returns (address) {
        Transaction transaction = new Transaction(
            batchid,
            transactionid,
            quantity,
            item_price,
            order_date,
            fromOwner,
            toOwner
        );
        transactions[batchid].push(transaction);
        transactionids[transactionid] = transaction; // This is for getting transactions from transactionId.
        userTransactions[fromOwner].push(transaction);
        return transaction;
    }
    
    function addTransactionWithTransport(
        uint batchid,
        string transactionid,
        uint quantity,
        uint item_price,
        uint order_date,
        string fromOwner,
        string toOwner,
        string transporter,
        uint pickup_date,
        uint deliver_date
    ) public returns (address) {
        Transaction transaction = new Transaction(
            batchid,
            transactionid,
            quantity,
            item_price,
            order_date,
            fromOwner,
            toOwner
        );
        
        transaction.setTransport(
            transporter,
            pickup_date,
            deliver_date
        );
        transactions[batchid].push(transaction);
        transactionids[transactionid] = transaction; 
        userTransactions[fromOwner].push(transaction);
        return transaction;
    }
    
    function updateTransport(string transactionId, string transporter, uint pickup_date, uint deliver_date) public {
         Transaction transaction = getTransaction(transactionId);
         transaction.setTransport(
            transporter,
            pickup_date,
            deliver_date
        );
    }
}
