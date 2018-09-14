using Application.Models;

namespace Application.DataAbstraction
{
    public interface ITransportUpdateDao
    {
        /// <summary>
        /// Executes a TransportUpdate
        /// </summary>
        /// <param name="update">the update to execute</param>
        void ExecuteTransportUpdate(TransportUpdate update);
    }
}