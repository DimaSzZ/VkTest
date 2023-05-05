using Microsoft.EntityFrameworkCore;
using VkTest.Jwt;
using VkTest.Logic.Domain;
using VkTest.Parsistence;
using VkTest.Parsistence.DbContext;
using VkTest.Parsistence.dto;

namespace VkTest.Logic.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PgDbContext _db;
    private readonly JwtAuthorization _jwtAuthorization;
    public UserRepository(PgDbContext db,JwtAuthorization jwtAuthorization)
    {
        _db = db;
        _jwtAuthorization = jwtAuthorization;
    }
    public async Task<string> AddUser(RegistrationUser user, CancellationToken cancellationToken)
    {
        if(_db.Users.Count(x => x.UserGroup.Code == "Admin")==1 && user.GroupCode =="Admin")
            throw new("Only one admin can exist");
        
        var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Login == user.Login && (DateTime.UtcNow - u.CreatedDate).TotalSeconds < 5, cancellationToken);
        if(existingUser != null)
            throw new Exception("User with this login was created less than 5 seconds ago");
        
        
        var userState = new UserState("Active", user.StateDescription);
        await _db.UsersState.AddAsync(userState,cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        
        var userGroup = new UserGroup(user.GroupCode, user.GroupDescription);
        await _db.UsersGroup.AddAsync(userGroup,cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        var cleanUser = new User(user.Login, user.Password);
        cleanUser.CreatedDate = DateTime.UtcNow;
        cleanUser.UserGroupId = userGroup.Id;
        cleanUser.UserStateId = userState.Id;
        await _db.Users.AddAsync(cleanUser,cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        
        return "User Added";
    }
    public async Task<FullInformationUser> GetUser(int id, CancellationToken cancellationToken)
    {
        var user = await _db.Users.Include(x => x.UserState)
            .Include(x => x.UserGroup)
            .FirstAsync(x => x.Id == id, cancellationToken);
        if (user.UserState.Code == "Blocked")
            throw new("User with this id has been deleted");
        return user.ToDetail();
    }
    public async Task<List<FullInformationUser>> GetAllUsers(int countUsers,CancellationToken cancellationToken)
    {
        var users = await _db.Users
            .Include(x => x.UserState)
            .Include(x => x.UserGroup)
            .Take(countUsers).Where(x=>x.UserState.Code != "Blocked").ToListAsync(cancellationToken);
        return  users.Select(x=>x.ToDetail()).ToList();
    }
    public async Task<string> DeleteUser(int id, CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .Include(x => x.UserState)
            .Include(x => x.UserGroup)
            .FirstAsync(x=>x.Id == id,cancellationToken);
        user.UserState.Code = "Blocked";
        _db.Users.Update(user);
        await _db.SaveChangesAsync(cancellationToken);
        return "User has deleted";
    }

    public async Task<string> AuthorizationUser(string login, string password, CancellationToken cancellationToken)
    {
        if (!await _db.Users.AnyAsync(x => x.Login == login && x.Password == password, cancellationToken))
                throw new ("User don't exist");
        var result = _jwtAuthorization.Authenticate(login)!;
        return result;
    }
}