using Ecobit_Blockchain_Frontend.Models;

namespace Ecobit_Blockchain_Frontend.DataAccess.Interfaces
{
    public interface IUserDao
    {
        void Create(User user);

        User Read(string companyName);

        void Update(User user);

        void Delete(User user);

        bool UserExists(string companyName);
    }
}