using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;

namespace Application.DataAbstraction.NethereumAbstraction
{
    public interface IFunction
    {
        Task<string> SendTransactionAsync(string from, HexBigInteger gas, HexBigInteger value, 
            params object[] functionInput);
    }
}    