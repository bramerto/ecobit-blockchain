pragma solidity ^0.4.23;

import "truffle/Assert.sol";
import "truffle/DeployedAddresses.sol";
import "../contracts/EcobitBlockchain.sol";
import "../contracts/Transaction.sol";

contract TestEcobitBlockchain {

    function testAddTransaction() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());

        manager.addTransaction(700, "62FA647C-AD54-4BCC-A860-E5A2664B019D", 100, 250, 1526545766, "Smartphone Fabriek B.V.", "Smartphone Groothandel");
        address a = manager.getTransactions(700)[0];
        Transaction t = Transaction(a);		

        Assert.equal(t.getBatchID(), uint256(700), "ID should be 700");
        Assert.equal(t.getTransactionID(), "62FA647C-AD54-4BCC-A860-E5A2664B019D", "TransactionId should be 62FA647C-AD54-4BCC-A860-E5A2664B019D");
        Assert.equal(t.getQuantity(), uint256(100), "Quantity should be 100");
        Assert.equal(t.getItemPrice(), uint(250), "Item price should be 350");
        Assert.equal(t.getOrderDate(), uint(1526545766), "Order date should be 1526545766");
        Assert.equal(t.getFromOwner(), "Smartphone Fabriek B.V.", "From should be Smartphone Fabriek B.V.");
        Assert.equal(t.getToOwner(), "Smartphone Groothandel", "To should be Smartphone Groothandel");
    }

    function testAddTransactionWithTransport() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());

        manager.addTransactionWithTransport(701, "62FA647C-AD54-4BCC-A860-E5A2664B019D", 100, 250, 12345, "Fabriek", "Groothandel", "DHL", 101234, 201234);
        address a = manager.getTransactions(701)[0];
        Transaction t = Transaction(a);

        Assert.equal(t.getTransporter(), "DHL", "Should be DHL");
        Assert.equal(t.getTransportPickupDate(), 101234, "Pickup date should be 101234");
        Assert.equal(t.getTransportDeliverDate(), 201234, "Deliver date should be 201234");
    }

    function testAddTransportToTransaction() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());

        manager.addTransaction(701, "62FA647C-AD54-4BCC-A860-E5A2664B019D", 100, 250, 12345, "Fabriek", "Groothandel");
        address a = manager.getTransactions(701)[0];
        Transaction t = Transaction(a);
        t.setTransport("DHL", 101234, 201234);

        Assert.equal(t.getTransporter(), t.getTransporter(), "Should be DHL");
        Assert.equal(t.getTransportPickupDate(), 101234, "Pickup date should be 101234");
        Assert.equal(t.getTransportDeliverDate(), 201234, "Deliver date should be 201234");		
    }

      function testAddTransportToTransactionWhichAlreadyHasBeenInput() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());

        address a = manager.getTransactions(701)[0];
        Transaction t = Transaction(a);
        t.setTransport("New DHL", 1, 2);

        Assert.equal(t.getTransporter(), "DHL", "Should be DHL");
        Assert.equal(t.getTransportPickupDate(), 101234, "Pickup date should be 101234");
        Assert.equal(t.getTransportDeliverDate(), 201234, "Deliver date should be 201234");		
    }

    function testGetExistingTransaction() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());

        Transaction t = manager.getTransaction("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        

        Assert.equal(t.getBatchID(), uint256(701), "ID should be 701");
        Assert.equal(t.getTransactionID(), "62FA647C-AD54-4BCC-A860-E5A2664B019D", "TransactionId should be 62FA647C-AD54-4BCC-A860-E5A2664B019D");
        Assert.equal(t.getQuantity(), uint256(100), "Quantity should be 100");
        Assert.equal(t.getItemPrice(), uint(250), "Item price should be 350");
        Assert.equal(t.getOrderDate(), uint(12345), "Order date should be 12345");
        Assert.equal(t.getFromOwner(), "Fabriek", "From should be Fabriek");
        Assert.equal(t.getToOwner(), "Groothandel", "To should be Groothandel");
    }

    function testUpdateTransactionTransport() public {
        EcobitBlockchain manager = EcobitBlockchain(DeployedAddresses.EcobitBlockchain());
        
        manager.addTransaction(705, "NJKDAS89-SDS6-4BCC-A860-E5A2664B019A", 100, 250, 12345, "Fabriek", "Groothandel");
        manager.updateTransport("NJKDAS89-SDS6-4BCC-A860-E5A2664B019A","New DHL", 101234, 201234);
        address a = manager.getTransactions(705)[0];
        Transaction t = Transaction(a);

        Assert.equal(t.getTransporter(), "New DHL", "Should be New DHL");
        Assert.equal(t.getTransportPickupDate(), 101234, "Pickup date should be 101234");
        Assert.equal(t.getTransportDeliverDate(), 201234, "Deliver date should be 201234");	
    }

}
