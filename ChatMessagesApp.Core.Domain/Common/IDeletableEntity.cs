namespace ChatMessagesApp.Core.Domain.Common;

public interface IDeletableEntity
{
    public bool? IsDeleted { get; set; }
}

