using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Application.Validation
{
    public interface IValidator<in T> where T : class
    {
        void Validate(T entity);
        ValidationResults GetResults();
    }
}