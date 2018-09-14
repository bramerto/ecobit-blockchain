using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;

namespace Ecobit_Blockchain_Frontend.DataAccess.NethereumAbstraction
{
    public interface IFunction
    {
        Task<TReturn> CallAsync<TReturn>(params object[] functionInput);

        Task<string> SendTransactionAsync(string from, HexBigInteger gas, HexBigInteger value,
            params object[] functionInput);
        
        Task<TReturn> CallDeserializingToObjectAsync<TReturn>(params object[] functionInput) where TReturn : new();
    }
}