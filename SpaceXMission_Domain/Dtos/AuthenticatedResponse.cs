namespace SpaceXMission_Domain.Dtos
{
    public class AuthenticatedResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
