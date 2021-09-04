
public class Pair<T, K>
{
    public static Pair<Item, byte> NULL = new Pair<Item, byte>(null, 0);

    public readonly T key;
    public K value;

    public Pair(T key, K value)
    {
        this.key = key;
        this.value = value;
    }
}
