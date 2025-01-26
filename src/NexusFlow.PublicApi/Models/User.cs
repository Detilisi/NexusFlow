namespace NexusFlow.PublicApi.Models
{
    public class User
    {
        public int Code { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
