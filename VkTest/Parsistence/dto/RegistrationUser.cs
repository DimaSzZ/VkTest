namespace VkTest.Parsistence.dto;

public record RegistrationUser(
    string Login,
    string Password, 
    string GroupCode,
    string ?GroupDescription,
    string ?StateDescription
);