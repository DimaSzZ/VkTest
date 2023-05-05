using Microsoft.EntityFrameworkCore;
using VkTest.Jwt;
using VkTest.Logic.Repositories;
using VkTest.Parsistence;
using VkTest.Parsistence.DbContext;
using VkTest.Parsistence.dto;

namespace Units;

public class AddUserUnitTest
{
    [Fact]
    public async Task AddUser_WithUniqueLogin_CreatesUser()
    {

        var user = new RegistrationUser("testuser", "password", "User", "Regular User", "Active");

        var dbContextOptions = new DbContextOptionsBuilder<PgDbContext>()
            .UseInMemoryDatabase(databaseName: "AddUser_WithUniqueLogin_CreatesUser")
            .Options;
        var dbContext = new PgDbContext(dbContextOptions);
        var service = new UserRepository(dbContext, new JwtAuthorization("very+very+very_$ecretKey"));


        var result = await service.AddUser(user, CancellationToken.None);


        Assert.Equal("User Added", result);
        Assert.Single(dbContext.Users);
    }

    [Fact]
    public async Task AddUser_WithExistingLoginAnd5SecondsPassed_CreatesUser()
    {
        var user1 = new User("testuser", "password") { CreatedDate = DateTime.UtcNow.AddSeconds(-6) };
        var user2 = new RegistrationUser("testuser", "password", "User", "Regular User", "Active");

        var dbContextOptions = new DbContextOptionsBuilder<PgDbContext>()
            .UseInMemoryDatabase(databaseName: "AddUser_WithExistingLoginAnd5SecondsPassed_CreatesUser")
            .Options;
        var dbContext = new PgDbContext(dbContextOptions);
        await dbContext.Users.AddAsync(user1);
        await dbContext.SaveChangesAsync();
        var service = new UserRepository(dbContext, new JwtAuthorization("very+very+very_$ecretKey"));

        var result = await service.AddUser(user2, CancellationToken.None);

        Assert.Equal("User Added", result);
        Assert.Equal(2, dbContext.Users.Count());
    }
    
}