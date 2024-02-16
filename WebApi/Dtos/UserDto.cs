namespace WebApi.Dtos
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool Admin { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
