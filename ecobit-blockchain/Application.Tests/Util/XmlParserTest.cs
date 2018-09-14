using System;
using System.IO;
using Application.Exceptions;
using Application.Util;
using NUnit.Framework;

namespace Application.Tests.Util
{
    public class XmlParserTest
    {
        private const string HappyXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <ecobit_blockchain>
                <transaction>
                    <batch_id>3241</batch_id>
                    <quantity>150</quantity>
                    <order_time>2018-05-30T15:00:00</order_time>
                    <item_price>3.5</item_price>
                    <from_owner>Smartie Phones B.V.</from_owner>
                    <to_owner>Groothandel B.V.</to_owner>
                    <transport>
                        <transporter>UPS Arnhem</transporter>
                        <pickup_date>2018-05-31T15:00:00</pickup_date>
                        <deliver_date>2018-06-01T16:00:00</deliver_date>
                    </transport>
                </transaction>
            </ecobit_blockchain>";
        
        private const string HappyCompleteUpdateXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <ecobit_blockchain>
                <transport_update>
                    <transaction_id>1234</transaction_id>
                    <transport>
                        <transporter>UPS Arnhem</transporter>
                        <pickup_date>2018-05-21T15:00:00</pickup_date>
                        <deliver_date>2018-05-22T16:00:00</deliver_date>
                    </transport>
                </transport_update>
            </ecobit_blockchain>";
        
        private const string HappyIncompleteUpdateXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <ecobit_blockchain>
                <transport_update>
                    <transaction_id>1234</transaction_id>
                    <transport>
                        <deliver_date>2018-05-22T16:00:00</deliver_date>
                    </transport>
                </transport_update>
            </ecobit_blockchain>";

        private readonly XmlParser _parser = new XmlParser();

        [Test]
        public void ThatParsedXmlIsCorrectlyAndNotThrowsExceptions()
        {
            _parser.ParseTransactions(HappyXml);
            _parser.ParseTransportUpdates(HappyXml);
        }

        [Test]
        public void ThatThrowsExceptionWhenXmlIsInvalid()
        {
            const string brokenXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <ecobit_blockchain>
                <transaction>
                </thisisabrokentag>
            </ecobit_blockchain>";

            Assert.Throws<ParserException>(delegate { _parser.ParseTransactions(brokenXml); });
        }

        [Test]
        public void ThatTransactionPropertiesParsedCorrectly()
        {
            var transactions = _parser.ParseTransactions(HappyXml);

            var firstTransaction = transactions[0];
            
            Assert.AreEqual(3241, firstTransaction.BatchId);
            Assert.AreEqual(150, firstTransaction.Quantity);
            Assert.AreEqual(new DateTime(2018,5,30,15,00,00), firstTransaction.OrderTime);
            Assert.AreEqual(3.5, firstTransaction.ItemPrice);
            Assert.AreEqual("Smartie Phones B.V.", firstTransaction.From);
            Assert.AreEqual("Groothandel B.V.", firstTransaction.To);
        }

        [Test]
        public void ThatTransactionWithTransportParsedCorrectly()
        {
            IParser parser = new XmlParser();

            var transactions = parser.ParseTransactions(HappyXml);

            var transport = transactions[0].Transport;
            
            Assert.NotNull(transport);
            Assert.AreEqual("UPS Arnhem", transport.Transporter);
            Assert.AreEqual(new DateTime(2018,05,31,15,00,00), transport.PickupDate);
            Assert.AreEqual(new DateTime(2018,06,01,16,00,00), transport.DeliverDate);
        }

        [Test]
        public void ThatTransportUpdateParsedCorrectly()
        {
            var updates = _parser.ParseTransportUpdates(HappyCompleteUpdateXml);

            var update = updates[0];
            
            Assert.NotNull(update);
            Assert.AreEqual("1234", update.TransactionId);
            Assert.NotNull(update.Transport);
            Assert.AreEqual("UPS Arnhem", update.Transport.Transporter);
            Assert.AreEqual(new DateTime(2018,05,21,15,00,00), update.Transport.PickupDate);
            Assert.AreEqual(new DateTime(2018,05,22,16,00,00), update.Transport.DeliverDate);
        }

        [Test]
        public void ThatIncompleteTransportUpdateParsedCorrectly()
        {
            var updates = _parser.ParseTransportUpdates(HappyIncompleteUpdateXml);

            var update = updates[0];
            
            Assert.NotNull(update);
            Assert.AreEqual("1234", update.TransactionId);
            Assert.NotNull(update.Transport);
            Assert.Null(update.Transport.Transporter);
            Assert.Null(update.Transport.PickupDate);
            Assert.AreEqual(new DateTime(2018,05,22,16,00,00), update.Transport.DeliverDate);
        }

        [Test]
        public void ThatParsedTransactionsInTheTestFile()
        {
            var xml = ReadTestFile();

            var transactions = _parser.ParseTransactions(xml);
            
            Assert.NotNull(transactions[0]);
            Assert.NotNull(transactions[0].Transport);
        }
        
        [Test]
        public void ThatParsedTransportUpdatesInTheTestFile()
        {
            var xml = ReadTestFile();

            var updates = _parser.ParseTransportUpdates(xml);
            
            Assert.NotNull(updates[0]);
            Assert.NotNull(updates[0].Transport);
        }


        private static string ReadTestFile()
        {
            var stream = typeof(XmlParserTest).Assembly.GetManifestResourceStream("Application.Tests.Util.Xml.TestData.xml");
            Assert.NotNull(stream, "Embedded Resource unavailable");
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}