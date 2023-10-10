namespace DocumentService.Models.Response
{
    public class AccessTokenResponse
    {
        public required string AccessToken { get; set; }

        public Guid UserId { get; set; }
    }
}
