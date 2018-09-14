var EcobitBlockchain = artifacts.require("./EcobitBlockchain.sol");

module.exports = function(deployer) {
  deployer.deploy(EcobitBlockchain);
};
