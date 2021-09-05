
using System.Collections.Generic;

public class Pair<T, K>
{
    public static Pair<Item, List<Item>> NULL = new Pair<Item, List<Item>>(null, null);

    public readonly T key;
    public K value;

    public Pair(T key, K value)
    {
        this.key = key;
        this.value = value;
    }
}
