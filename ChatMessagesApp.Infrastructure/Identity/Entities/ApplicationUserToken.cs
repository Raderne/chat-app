using Microsoft.AspNetCore.Identity;

namespace ChatMessagesApp.Infrastructure.Identity.Entities
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public virtual AppUser User { get; set; }
    }
}