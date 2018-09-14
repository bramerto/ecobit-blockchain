
namespace Application.DataAbstraction.NethereumAbstraction
{
    public interface IContract
    {
        IFunction GetFunction(string name);
    }
}