using VkTest.Parsistence.dto;

namespace VkTest.Logic.Domain;

public interface IUserRepository
{
    public Task<string> AddUser(RegistrationUser user, CancellationToken cancellationToken);
    public Task<FullInformationUser>GetUser(int id, CancellationToken cancellationToken);
    public Task<List<FullInformationUser>>GetAllUsers(int countUsers,CancellationToken cancellationToken);
    public Task<string>DeleteUser(int id, CancellationToken cancellationToken);
    public Task<string> AuthorizationUser(string login, string password, CancellationToken cancellationToken);
}