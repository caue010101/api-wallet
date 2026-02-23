namespace minhaApi.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string AcessToken { get; set; } = string.Empty;
        public DateTime ExpireToken { get; set; }
    }
}
