using ChatMessagesApp.Core.Domain.Common;

namespace ChatMessagesApp.Core.Domain.DataObj;

public class User : IDeletableEntity
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    public string UserName { get; set; } = null!;
    public string NormalizedUserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public bool? EmailConfirmed { get; set; }
    public bool? PhoneNumberConfirmed { get; set; }

    public List<string> RoleNames { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? Created { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public bool? IsDeleted { get; set; }
}
