using Microsoft.AspNetCore.Identity;

namespace ChatMessagesApp.Infrastructure.Identity.Entities
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public string TenantId { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }
}