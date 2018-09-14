pragma solidity ^0.4.23;

contract Transaction {
    
    struct Transport {
        string transporter;
        uint pickup_date;
        uint deliver_date;
    }
    
    uint batchid;
    string transactionId;
    uint quantity;
    uint item_price;
    uint order_date;
    string fromOwner;
    string toOwner;
    Transport transport;
    
    constructor(
        uint _batchid,
        string _transactionId,
        uint _quantity,
        uint _item_price,
        uint _order_date,
        string _fromOwner,
        string _toOwner
    ) public {
        batchid = _batchid;
        transactionId = _transactionId;
        quantity = _quantity;
        item_price = _item_price;
        order_date = _order_date;
        fromOwner = _fromOwner;
        toOwner = _toOwner;
    }
    
    function getData() public view returns (uint, string, uint, uint, uint, string, string)  {
        return (
            batchid,
            transactionId,
            quantity,
            item_price,
            order_date,
            fromOwner,
            toOwner
        );
    }
    
    function getTransportData() public view returns (string, uint, uint) {
        return (
            transport.transporter,
            transport.pickup_date,
            transport.deliver_date
        );
    }

    function getBatchID() public view returns (uint) {
        return batchid;
    }

    function getTransactionID() public view returns (string) {
        return transactionId;
    }

    function getQuantity() public view returns(uint) {
        return quantity;
    }

    function getItemPrice() public view returns(uint) {
        return item_price;
    }

    function getOrderDate() public view returns(uint) {
        return order_date;
    }

    function getFromOwner() public view returns(string) {
        return fromOwner;
    }

    function getToOwner() public view returns(string) {
        return toOwner;
    }
    
    function setTransaction(uint _batchid, string _transactionId, uint _quantity, uint _item_price, uint _order_date, string _fromOwner, string _toOwner) public {
        batchid = _batchid;
        transactionId = _transactionId;
        quantity = _quantity;
        item_price = _item_price;
        order_date = _order_date;
        fromOwner = _fromOwner;
        toOwner = _toOwner;
    }

    function setTransport(string transporter, uint pickup_date, uint deliver_date) public {

        if (bytes(transport.transporter).length == 0) {
            transport.transporter = transporter;
        } 

        if (transport.pickup_date == 0) {
            transport.pickup_date = pickup_date;
        } 

        if (transport.deliver_date == 0) {
            transport.deliver_date = deliver_date;
        } 
    }

    function getTransporter() public view returns(string) {
        return transport.transporter;
    }

    function getTransportPickupDate() public view returns(uint) {
        return transport.pickup_date;
    }

    function getTransportDeliverDate() public view returns(uint) {
        return transport.deliver_date;
    }

}
