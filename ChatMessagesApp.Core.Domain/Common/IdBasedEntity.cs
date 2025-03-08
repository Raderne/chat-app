namespace ChatMessagesApp.Core.Domain.Common;

public abstract class IdBasedEntity : IdBasedEntity<Guid>
{
}

public abstract class IdBasedEntity<TId>
{
    public Guid? Uuid { get; set; }
    public virtual TId Id { get; set; }

    public override bool Equals(object? obj)
    {
        if (this == obj)
            return true;
        if (obj == null)
            return false;
        if (GetType() != obj.GetType())
            return false;
        var other = (IdBasedEntity<TId>)obj;
        if (Id == null)
        {
            if (other.Id != null)
                return false;
        }
        else if (!Id.Equals(other.Id))
        {
            return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        var prime = 31;
        var result = 1;

        return prime * result + (Id == null ? 0 : Id.GetHashCode());
    }
}
