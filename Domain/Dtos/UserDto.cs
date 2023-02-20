namespace Domain.Dtos;

public class UserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }

    public List<RoleDto> Roles { get; set; }
}