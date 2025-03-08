using Microsoft.AspNetCore.Identity;

namespace ChatMessagesApp.Infrastructure.Identity.Entities;

public class ApplicationRole : IdentityRole<string>
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
}
