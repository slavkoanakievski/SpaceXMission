using Microsoft.AspNetCore.Identity;

namespace SpaceXMission.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
