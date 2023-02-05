using System;

namespace Application.Auth.Account.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = string.Empty;
}