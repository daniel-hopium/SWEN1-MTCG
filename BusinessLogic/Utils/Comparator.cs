namespace BusinessLogic.Utils;
public static class Comparator
{
    public static bool Unique<T>(IEnumerable<T> collection)
    {
        HashSet<T> uniqueItems = new HashSet<T>();

        foreach (var item in collection)
        {
            if (!uniqueItems.Add(item))
            {
                // The item is already in the HashSet, so it's not unique
                return false;
            }
        }

        // All items are unique
        return true;
    }
}

