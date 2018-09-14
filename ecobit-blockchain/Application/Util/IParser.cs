using System.Collections.Generic;
using Application.Models;

namespace Application.Util
{
    public interface IParser
    {
        /// <summary>
        /// Parse all the transactions within the input string and return a list of the objects.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<Transaction> ParseTransactions(string input);
        
        /// <summary>
        /// Parse all the transports within the input string and returns a list of transport update objects.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>a list of TransportUpdate objects</returns>
        List<TransportUpdate> ParseTransportUpdates(string input);
        
    }
    
}