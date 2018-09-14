namespace Ecobit_Blockchain_Frontend.DataAccess.NethereumAbstraction
{
    public interface IContract
    {
        IFunction GetFunction(string name);
    }
}