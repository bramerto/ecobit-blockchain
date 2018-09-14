using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Application.Exceptions;
using Application.Models;
using Application.Validation;

namespace Application.Util
{
    public class XmlParser : IParser
    {

        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
        private static IValidator<Transaction> TransactionValidator { get; set; }
        private static IValidator<TransportUpdate> TransportUpdateValidator { get; set; }

        public XmlParser(): this(
            DependencyFactory.Resolve<IValidator<Transaction>>(),
            DependencyFactory.Resolve<IValidator<TransportUpdate>>())
        {
        }
        
        public XmlParser(IValidator<Transaction> transactionValidator, IValidator<TransportUpdate> transportUpdateValidator)
        {
            TransactionValidator = transactionValidator;
            TransportUpdateValidator = transportUpdateValidator;
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Parses the given xml and returns a list of Transactions
        /// </summary>
        /// <param name="input">The xml in string format</param>
        /// <returns>a list of Transactions</returns>
        /// <exception cref="T:Application.Util.ParserException">If the parsing fails</exception>
        public List<Transaction> ParseTransactions(string input)
        {
            var transactions = new List<Transaction>();
            var rootNode = GetRootNode(input);

            foreach (XmlNode child in rootNode.ChildNodes)
            {
                if (child.Name.Equals("transaction"))
                {
                    transactions.Add(ParseTransactionNode(child));
                }
            }
            
            return transactions;
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Parses the given xml and returns a list of TransportUpdates
        /// </summary>
        /// <param name="input">The xml in string format</param>
        /// <returns>a list of TransportUpdates</returns>
        /// <exception cref="T:Application.Util.ParserException">If the parsing fails</exception>
        public List<TransportUpdate> ParseTransportUpdates(string input)
        {
            var updates = new List<TransportUpdate>();
            var rootNode = GetRootNode(input);

            foreach (XmlNode child in rootNode.ChildNodes)
            {
                if (child.Name.Equals("transport_update"))
                {
                    updates.Add(ParseTransportUpdateNode(child));
                }
            }

            return updates;
        }

        /// <summary>
        /// Parses the xml and returns the root node
        /// </summary>
        /// <param name="input">the xml in string format</param>
        /// <returns>The ecobit_blockchain root node</returns>
        /// <exception cref="ParserException">if the XML cannot be parsed or if the root node is not present</exception>
        private static XmlNode GetRootNode(string input)
        {
            var document = new XmlDocument();
            
            try
            {
                document.LoadXml(input);
            }
            catch (XmlException e)
            {
                throw new ParserException(e.Message);
            }

            if (document.DocumentElement == null)
            {
                throw new ParserException("Input could not be parsed");
            }

            var rootNode = document.DocumentElement.SelectSingleNode("/ecobit_blockchain");

            if (rootNode == null)
            {
                throw new ParserException("Root element could not be found");
            }

            return rootNode;
        }

        /// <summary>
        /// Parses the contents of the given node and returns a Transaction
        /// </summary>
        /// <param name="node">The xml node to be parsed</param>
        /// <returns>a Transaction</returns>
        private static Transaction ParseTransactionNode(XmlNode node)
        {
            var t = new Transaction
            {
                BatchId = Convert.ToInt32(FindProperty(node, "batch_id")),
                TransactionId = GenerateIdUtil.GenerateUniqueId(),
                Quantity = Convert.ToInt32(FindProperty(node, "quantity")),
                ItemPrice = Convert.ToDouble(FindProperty(node, "item_price"), Culture),
                OrderTime = Convert.ToDateTime(FindProperty(node, "order_time"), Culture),
                From = FindProperty(node, "from_owner"),
                To = FindProperty(node, "to_owner")
            };
            
            var transportNode = node.SelectSingleNode("transport");

            if (transportNode != null)
            {
                t.Transport = ParseTransportNode(transportNode);
            }

            var transactionValidator = TransactionValidator;
            
            transactionValidator.Validate(t);
            

            if (transactionValidator.GetResults().Count != 0)
            {
                foreach (var result in transactionValidator.GetResults())
                {
                    Console.WriteLine(result.Message);
                }

                throw new ParserException("Validator has detected an error.");
            }

            return t;
        }

        /// <summary>
        /// Parses the contents of the given node and returns a Transport
        /// </summary>
        /// <param name="node">The xml node to  be parsed</param>
        /// <returns>a Transport</returns>
        private static Transport ParseTransportNode(XmlNode node)
        {
            var transport = new Transport();
            var transporter = FindProperty(node, "transporter", true);
            var pickupDate = FindProperty(node, "pickup_date", true);
            var deliverDate = FindProperty(node, "deliver_date", true);

            if (transporter != null)
            {
                transport.Transporter = transporter;
            }

            if (pickupDate != null)
            {
                transport.PickupDate = Convert.ToDateTime(pickupDate, Culture);
            }

            if (deliverDate != null)
            {
                transport.DeliverDate = Convert.ToDateTime(deliverDate, Culture);
            }

            return transport;
        }

        /// <summary>
        /// Parses the contents of the given node and returns a TransportUpdate
        /// </summary>
        /// <param name="node">The xml node to be parsed</param>
        /// <returns>a TransportUpdate</returns>
        /// <exception cref="ParserException">If the transport element cannot be found</exception>
        private static TransportUpdate ParseTransportUpdateNode(XmlNode node)
        {
            var transportUpdate = new TransportUpdate
            {
                TransactionId = FindProperty(node, "transaction_id")
            };
            
            var transportNode = node.SelectSingleNode("transport");
            if (transportNode == null)
            {
                throw new ParserException("Transport element could not be found");
            }

            transportUpdate.Transport = ParseTransportNode(transportNode);


            var transportUpdateValidator = TransportUpdateValidator;
            
            transportUpdateValidator.Validate(transportUpdate);

            if (transportUpdateValidator.GetResults().Count != 0)
            {
                foreach (var result in transportUpdateValidator.GetResults())
                {
                    Console.WriteLine(result.Message);
                }

                throw new ParserException("Validator has detected an error.");
            }
            
            return transportUpdate;
        }

        /// <summary>
        /// Find a property that is contained within the node
        /// </summary>
        /// <param name="node">The node to be searched</param>
        /// <param name="name">The name of the property</param>
        /// <param name="optional">Whether an exception should be thrown if the property is not present</param>
        /// <returns>The property or null if not present and optional is true</returns>
        /// <exception cref="ParserException">If the property cannot be found and optional is false</exception>
        private static string FindProperty(XmlNode node, string name, bool optional = false)
        {
            var child = node.SelectSingleNode(name);

            if (child != null)
            {
                return child.InnerText;
            }

            if (optional)
            {
                return null;
            }

            throw new ParserException("Parameter not found: " + name);
        }
    }
}