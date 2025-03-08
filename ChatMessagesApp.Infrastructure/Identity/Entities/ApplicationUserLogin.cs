using Microsoft.AspNetCore.Identity;

namespace ChatMessagesApp.Infrastructure.Identity.Entities
{
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public virtual AppUser User { get; set; }
    }
}