
public interface ICraftable : IItemable
{
    public Recipe GetRecipe();

    public Crafter GetCrafterPrefhab();

    public float GetTime();
}
