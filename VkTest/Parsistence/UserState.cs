using Newtonsoft.Json;

namespace VkTest.Parsistence;

public class UserState
{
    public UserState(string code, string? description)
    {
        Code = code;
        Description = description;
    }

    public int Id { get; set; }
    public string Code { get; set; }
    public string? Description { get; set; }
    [JsonIgnore] public User User { get; set; }
}