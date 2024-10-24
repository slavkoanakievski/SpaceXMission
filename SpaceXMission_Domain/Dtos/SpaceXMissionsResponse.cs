using Newtonsoft.Json;

namespace SpaceXMission_Domain.Dtos
{
    public class SpaceXMissionResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string FlightNumber { get; set; }
        [JsonProperty("date_utc")]
        public DateTime? DateUtc { get; set; }
        [JsonProperty("date_local")]
        public DateTime? DateLocal { get; set; }
        public bool? Upcoming { get; set; }
        public bool? Success { get; set; }
        public string? Rocket { get; set; }
        public List<CrewResponse>? Crew { get; set; }
        public LinksResponse? Links { get; set; }
        public string? Launchpad { get; set; }
        public string? Details { get; set; }
        public List<CoreResponse>? Cores { get; set; }
    }

    public class CrewResponse
    {
        public string? Crew { get; set; }
        public string? Role { get; set; }
    }

    public class LinksResponse
    {
        public PatchLinksResponse? Patch { get; set; }
        public RedditResponse? Reddit { get; set; }
        public string? Webcast { get; set; }
        public string? Wikipedia { get; set; }
    }

    public class PatchLinksResponse
    {
        public string? Small { get; set; }
        public string? Large { get; set; }
    }

    public class RedditResponse
    {
        public string? Launch { get; set; }
    }

    public class CoreResponse
    {
        public string? Core { get; set; }
        public int? Flight { get; set; }
        public bool? Reused { get; set; }
        public bool? LandingAttempt { get; set; }
        public bool? LandingSuccess { get; set; }
        public string? LandingType { get; set; }
    }
}
