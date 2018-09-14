pragma solidity ^0.4.23;

import "truffle/Assert.sol";
import "truffle/DeployedAddresses.sol";
import "../contracts/EcobitBlockchain.sol";
import "../contracts/Transaction.sol";

contract TestUser {

    function testGetUserTransactionsAmount() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());
        
        // this is the second transaction for Smartphone Fabriek B.V.
        manager.addTransactionWithTransport(100, "62FA647C-AD54-4BCC-A860-E5A2664B019D", 100, 250, 1526545766, "Smartphone Fabriek B.V.", "Smartphone Groothandel", "DHL", 101234, 201234);
       
        uint a = manager.getUserTransactionsLength("Smartphone Fabriek B.V.");
       
        Assert.equal(a, 1, "Amount of transaction for the user should be 2");
    }

    function testGetUserTransactions() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());

        address a = manager.getUserTransactions("Smartphone Fabriek B.V.")[0];
        Transaction t = Transaction(a);

        Assert.equal(t.getBatchID(), uint256(100), "ID should be 700");
        Assert.equal(t.getTransactionID(), "62FA647C-AD54-4BCC-A860-E5A2664B019D", "TransactionId should be 62FA647C-AD54-4BCC-A860-E5A2664B019D");
        Assert.equal(t.getQuantity(), uint256(100), "Quantity should be 100");
        Assert.equal(t.getItemPrice(), uint(250), "Item price should be 350");
        Assert.equal(t.getOrderDate(), uint(1526545766), "Order date should be 1526545766");
        Assert.equal(t.getFromOwner(), "Smartphone Fabriek B.V.", "From should be Smartphone Fabriek B.V.");
        Assert.equal(t.getToOwner(), "Smartphone Groothandel", "To should be Smartphone Groothandel");
    }


    function testIsUserRemoved() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());
        
        manager.addUser("company1", "1234", "test1@test.nl", "test1");
        manager.addUser("company2", "4321", "test2@test.nl", "test2");

        Assert.equal(manager.getUsersCount(), 2, "Length should be 2");

        manager.removeUser("company1");
        var (companyName, password, email, contact) = manager.getUser("company1");

        // check if its actually deleted.
        Assert.equal(companyName, "", "companyName should be empty/not exist");
        Assert.equal(password, "", "password should be empty/not exist");
        Assert.equal(email, "", "email should be empty/not exist");
        Assert.equal(contact, "", "contact should be empty/not exist");
        Assert.equal(manager.getUsersCount(), 1, "Length should be 1");

        manager.removeUser("company2"); //Delete last user for next test
    }

    function testIsUserAdded() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());
        
        manager.addUser("Smartphone Fabriek B.V.", "1234", "test@test.nl", "Jurre");

        Assert.equal(manager.getUsersCount(), 1, "Length should be greater than 0");

    }

    function testAddUser() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());
        
        var (companyName, password, email, contact) = manager.getUser("Smartphone Fabriek B.V.");
        
        Assert.equal(companyName, "Smartphone Fabriek B.V.", "companyName should be Smartphone Fabriek B.V.");
        Assert.equal(password, "1234", "Password should be 1234");
        Assert.equal(email, "test@test.nl", "Email should be test@test.nl");
        Assert.equal(contact, "Jurre", "Contact should be Jurre");

        // Need to remove account for next test
        manager.removeUser("Smartphone Fabriek B.V.");
    }
    
    function testUpdateUserEmail() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());
        
        manager.addUser("old company", "old password", "oldEmail@test.nl", "old contact");
        manager.updateUserEmail("old company", "newEmail@test.nl");

        var (companyName, password, email, contact) = manager.getUser("old company");

        Assert.equal(email, "newEmail@test.nl", "Email should be newEmail@test.nl");
        
    }

    function testUpdateUser() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());
        manager.updateUserContact("company1", "new contact");

        var (companyName, password, email, contact) = manager.getUser("company1");

        Assert.equal(contact, "new contact", "Contact should be new contact");
    }

    function testUpdateUserPassword() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());   
        manager.updateUserPassword("company1", "new password");

        var (companyName, password, email, contact) = manager.getUser("company1");

        Assert.equal(password, "new password", "Password should be new password");
    }

    function testDoesUserExistWithoutUser() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());  
        
        Assert.equal(manager.doesUserExist("Smartphone Fabriek B.V."), false, "User should exist");
    }

    function testDoesUserExistWithUser() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());  
        manager.addUser("newuser", "1234", "test@test.nl", "Jurre");

        Assert.equal(manager.doesUserExist("newuser"), true, "User should exist");
    }
}