using Microsoft.EntityFrameworkCore;
using VkTest.Jwt;
using VkTest.Logic.Repositories;
using VkTest.Parsistence;
using VkTest.Parsistence.DbContext;
using VkTest.Parsistence.dto;

namespace Units;

public class GetUserUnitTest
{
    private readonly DbContextOptions<PgDbContext> _dbContextOptions = new DbContextOptionsBuilder<PgDbContext>()
        .UseInMemoryDatabase(databaseName: "GetUserTestDatabase")
        .Options;

    [Fact]
    public async Task GetUser_WithValidId_ReturnsFullInformationUser()
    {
        // Arrange
        int userId = 1;
        CancellationToken cancellationToken = CancellationToken.None;

        User user = new User("testuser", "password")
        {
            Id = userId,
            CreatedDate = DateTime.UtcNow,
            UserGroupId = 1,
            UserGroup = new UserGroup("Group1", "Group 1 Description"),
            UserStateId = 1,
            UserState = new UserState("Active", "Active User")
        };

        using (var context = new PgDbContext(_dbContextOptions))
        {
            await context.Users.AddAsync(user,cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        var userRepository = new UserRepository(new PgDbContext(_dbContextOptions), new JwtAuthorization("very+very+very_$ecretKey"));
        
        FullInformationUser result = await userRepository.GetUser(userId, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Login, result.Login);
        Assert.Equal(user.Password, result.Password);
        Assert.Equal(user.CreatedDate, result.CreateDate);
        Assert.Equal(user.UserGroup.Description, result.GroupDescription);
        Assert.Equal(user.UserState.Code, result.StateCode);
        Assert.Equal(user.UserState.Description, result.StateDescription);
    }
}