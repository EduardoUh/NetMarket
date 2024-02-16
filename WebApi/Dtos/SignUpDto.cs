namespace WebApi.Dtos
{
    public class SignUpDto
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
