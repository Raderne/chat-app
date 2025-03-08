using ChatMessagesApp.Core.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace ChatMessagesApp.Infrastructure.Identity.Entities;

public class AppUser : IdentityUser<string>, IAuditableEntity<string>, IDeletableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

    public bool? IsDeleted { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
}
