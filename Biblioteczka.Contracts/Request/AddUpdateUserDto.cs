namespace Biblioteczka.Contracts.Request
{
    public record AddUpdateUserDto
    {
        public int? Id { get; set; }
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
    }
}