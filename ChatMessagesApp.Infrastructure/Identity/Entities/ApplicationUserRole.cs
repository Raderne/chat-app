using Microsoft.AspNetCore.Identity;

namespace ChatMessagesApp.Infrastructure.Identity.Entities
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual AppUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}