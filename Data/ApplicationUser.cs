using Microsoft.AspNetCore.Identity;

namespace thekitchen_aspnetcore.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
