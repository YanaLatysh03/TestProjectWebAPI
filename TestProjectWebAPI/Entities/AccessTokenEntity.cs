namespace DocumentService.Entities
{
    public class AccessTokenEntity
    {
        public Guid Id { get; set; }
        public required string AccessToken { get; set; }
        public required DateTime ExpireAt { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }

    }
}