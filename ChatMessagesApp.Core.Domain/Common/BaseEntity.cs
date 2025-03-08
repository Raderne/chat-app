using System.Text.Json.Serialization;

namespace ChatMessagesApp.Core.Domain.Common;

public class BaseEntity<TId> : IdBasedEntity<TId>, IAuditableEntity<string>, IDeletableEntity
{
    public virtual DateTime Created { get; set; }
    public virtual DateTime? LastModified { get; set; }
    public virtual string CreatedBy { get; set; }
    public virtual string LastModifiedBy { get; set; }
    public virtual bool? IsDeleted { get; set; }

    [JsonIgnore]
    public byte[]? RowVersion { get; set; }
}
