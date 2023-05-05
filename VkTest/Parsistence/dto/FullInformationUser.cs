namespace VkTest.Parsistence.dto;

public record FullInformationUser(
    string Login,
    string Password,
    DateTime CreateDate,
    string GroupCode,
    string ?GroupDescription,
    string StateCode,
    string ?StateDescription
    );