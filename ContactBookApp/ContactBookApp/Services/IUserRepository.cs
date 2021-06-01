using ContactBookApp.Models;
using System.Threading.Tasks;

namespace ContactBookApp.Services
{
    public interface IUserRepository
    {
        Task<ManagerResponse> RegisterUserAsync(User model);
        Task<ManagerResponse> LoginUserAsync(Login Model);
    }
}