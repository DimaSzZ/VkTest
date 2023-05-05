using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using VkTest.Jwt;
using VkTest.Logic.Repositories;
using VkTest.Parsistence;
using VkTest.Parsistence.DbContext;

namespace Units;

public class GetAllUsersUnitTests
{
    [Fact]
    public async Task GetAllUsers_ReturnsAllActiveUsers()
    {
        var users = new List<User>
        {
            new User("user1", "password1") { UserState = new UserState("Active", "Active") },
            new User("user2", "password2") { UserState = new UserState("Active", "Active") },
            new User("user3", "password3") { UserState = new UserState("Blocked", "Blocked") },
            new User("user4", "password4") { UserState = new UserState("Active", "Active") }
        };

        var dbContextOptions = new DbContextOptionsBuilder<PgDbContext>()
            .UseInMemoryDatabase(databaseName: "GetAllUsers_ReturnsAllActiveUsers")
            .Options;
        using var dbContext = new PgDbContext(dbContextOptions);
        await dbContext.Users.AddRangeAsync(users);
        await dbContext.SaveChangesAsync();

        var repository = new UserRepository(dbContext, new JwtAuthorization("very+very+very_$ecretKey"));


        var result = await repository.GetAllUsers(5, CancellationToken.None);


        Assert.Equal(3, result.Count);
        Assert.True(result.All(x => x.StateCode == "Active"));

    }

[Fact]
public async Task GetAllUsers_ReturnsEmptyListWhenNoUsersFound()
{

    var dbContextOptions = new DbContextOptionsBuilder<PgDbContext>()
        .UseInMemoryDatabase(databaseName: "GetAllUsers_ReturnsEmptyListWhenNoUsersFound")
        .Options;
    var dbContext = new PgDbContext(dbContextOptions);
    var repository = new UserRepository(dbContext, new JwtAuthorization("very+very+very_$ecretKey"));

 
    var result = await repository.GetAllUsers(10, CancellationToken.None);

  
    Assert.Empty(result);
}
}