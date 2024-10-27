namespace SpaceXMission_Domain.Dtos
{
    public class SpaceXMissionResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int FlightNumber { get; set; }
        public DateTime DateUtc { get; set; }
        public DateTime DateLocal { get; set; }
        public bool Success { get; set; }
        public string? Rocket { get; set; }
        public List<CrewResponseModel>? Crew { get; set; }
        public string? Webcast { get; set; }
        public string? Wikipedia { get; set; }
        public string? SmallImageUrl { get; set; }
        public string? LargeImageUrl { get; set; }
    }
}
