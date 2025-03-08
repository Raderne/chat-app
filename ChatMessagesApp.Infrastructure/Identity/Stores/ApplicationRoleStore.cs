using ChatMessagesApp.Infrastructure.Context;
using ChatMessagesApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChatMessagesApp.Infrastructure.Identity.Stores;

public class ApplicationRoleStore : RoleStore<ApplicationRole, IdentityContext, string, ApplicationUserRole, ApplicationRoleClaim>
{
    public ApplicationRoleStore(IdentityContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
    {
    }
}
