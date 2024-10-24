namespace SpaceXMission_Domain.Dtos
{
    public class FailureResponseModel
    {
        public int Time { get; set; }
        public int? Altitude { get; set; }
        public string Reason { get; set; }
    }
}
