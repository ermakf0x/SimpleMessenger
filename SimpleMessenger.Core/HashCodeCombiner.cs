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
            var hash = ComputeHash(0, first);
            while (enumerator.MoveNext())
            {
                hash = ComputeHash(hash, enumerator.Current);
                counter++;
            }

            if (counter == 1)
            {
                if (first is null) return 0;
                return first.GetHashCode();
            }
            return hash;
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

        var hash = 0;
        foreach (var item in items)
            hash = ComputeHash(hash, item);
        return hash;
    }

    public static int ComputeHash<T1>(T1 a)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + a?.GetHashCode() ?? 0;
            return hash;
        }
    }
    public static int ComputeHash<T1, T2>(T1 a, T2 b)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + a?.GetHashCode() ?? 0;
            hash = hash * 31 + b?.GetHashCode() ?? 0;
            return hash;
        }
    }
}
