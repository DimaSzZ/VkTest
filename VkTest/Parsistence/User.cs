using System.Text.Json.Serialization;
using VkTest.Parsistence.dto;

namespace VkTest.Parsistence;

public class User
{
    public User(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    
    public DateTime CreatedDate { get; set; }
    [JsonIgnore] public UserGroup UserGroup { get; set; }
    public int UserGroupId { get; set; }
    [JsonIgnore] public UserState UserState { get; set; }
    
    public int UserStateId { get; set; }

    public FullInformationUser ToDetail()
    {
        return new FullInformationUser(
            Login,
            Password,
            CreatedDate, 
            UserGroup.Code,
            UserGroup.Description,
            UserState.Code,
            UserState.Description
            );
    }
}