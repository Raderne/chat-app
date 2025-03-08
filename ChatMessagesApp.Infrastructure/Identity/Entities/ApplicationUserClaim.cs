using Microsoft.AspNetCore.Identity;

namespace ChatMessagesApp.Infrastructure.Identity.Entities
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual AppUser User { get; set; }
    }
}