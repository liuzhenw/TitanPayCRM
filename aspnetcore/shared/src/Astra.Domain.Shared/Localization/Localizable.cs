namespace Astra.Localization;

public class Localizable
    : Dictionary<string, LocalizableProperties>, ICloneable
{
    public Localizable() : base(StringComparer.InvariantCultureIgnoreCase) { }

    public Localizable(Dictionary<string, LocalizableProperties> source) :
        base(source, StringComparer.InvariantCultureIgnoreCase) { }

    public object Clone()
    {
        var clone = this.ToDictionary(
            x => x.Key,
            x => new LocalizableProperties(x.Value.ToDictionary(y => y.Key, y => y.Value)));
        return new Localizable(clone);
    }

    public override bool Equals(object? obj)
    {
        return obj is Localizable other && Equals(other);
    }

    public override int GetHashCode()
    {
        return this.Aggregate(
            0, (a, x) =>
                HashCode.Combine(a, HashCode.Combine(x.Key.GetHashCode(), x.Value.GetHashCode())));
    }

    protected bool Equals(Localizable other)
    {
        if (other.Count != Count)
            return false;

        foreach (var item in other)
        {
            if (!ContainsKey(item.Key))
                return false;

            if (!this[item.Key].Equals(item.Value))
                return false;
        }

        return true;
    }
}

public class LocalizableProperties : Dictionary<string, string>, ICloneable
{
    public LocalizableProperties() : base(StringComparer.InvariantCultureIgnoreCase) { }

    public LocalizableProperties(IDictionary<string, string> source) : base(
        source, StringComparer.InvariantCultureIgnoreCase) { }

    public object Clone()
    {
        var clone = this.ToDictionary(
            x => x.Key,
            x => x.Value);
        return new LocalizableProperties(clone);
    }

    public override int GetHashCode()
    {
        return this.Aggregate(
            0, (a, x) => HashCode.Combine(a, x.Key.GetHashCode(), x.Value.GetHashCode()));
    }

    public override bool Equals(object? obj)
    {
        return obj is LocalizableProperties other && Equals(other);
    }

    protected bool Equals(LocalizableProperties other)
    {
        if (other.Count != Count)
            return false;

        foreach (var item in other)
        {
            if (!ContainsKey(item.Key))
                return false;

            if (!this[item.Key].Equals(item.Value))
                return false;
        }

        return true;
    }
}