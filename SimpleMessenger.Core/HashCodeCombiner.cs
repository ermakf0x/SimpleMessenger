namespace SimpleMessenger.Core;

public static class HashCodeCombiner
{
    public static int Combine<T>(IEnumerable<T> items)
    {
        if (items is null) return 0;

        using var enumerator = items.GetEnumerator();
        if (enumerator.MoveNext())
        {
            T first = enumerator.Current;
            var counter = 1;
            var hash = new HashCode();
            while (enumerator.MoveNext())
            {
                hash.Add(enumerator.Current);
                counter++;
            }

            if (counter == 1)
            {
                if (first is null) return 0;
                return first.GetHashCode();
            }
            return hash.ToHashCode();
        }
        return 0;
    }

    public static int Combine<T>(ICollection<T> items)
    {
        if (items is null || items.Count == 0) return 0;
        if (items.Count == 1)
        {
            var first = items.First();
            if(first is null) return 0;
            return first.GetHashCode();
        }

        var hash = new HashCode();
        foreach (var item in items)
            hash.Add(item);
        return hash.ToHashCode();
    }

    public static int Combine<T>(IList<T> items)
    {
        if (items is null || items.Count == 0) return 0;
        if (items.Count == 1)
        {
            var first = items[0];
            if (first is null) return 0;
            return first.GetHashCode();
        }

        var hash = new HashCode();
        foreach (var item in items)
            hash.Add(item);
        return hash.ToHashCode();
    }
}
