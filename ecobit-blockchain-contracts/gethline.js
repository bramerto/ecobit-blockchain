#!/usr/bin/node

var transactionstore = require('./build/contracts/TransactionManager');

var abi = JSON.stringify(transactionstore.abi);
var address = transactionstore.networks["5777"].address;

var line = `var ecobit = eth.contract(${abi}).at('${address}')`;
console.log(line);
