using Photon.Pun;

public class Lapiz : Item, IConsumable, ICraftable
{
    private const string ID = "lapiz";
    private const int CHAKRA_AMOUNT = 10;

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Lapiz;
    }

    public string GetReward()
    {
        return Manager.Instance.GetLanguage().Chakra;
    }

    public void Consume(Ninja source)
    {
        source.IncreaseMaxChakra(CHAKRA_AMOUNT);
        DestroyItem();
    }

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.LapizPref;
    }

    public Recipe GetRecipe()
    {
        Recipe recipe = Recipe.Create(3);
        recipe.Add(ItemLibrary.Instance.StickPref, 1);
        recipe.Add(ItemLibrary.Instance.IronOrePref, 1);
        recipe.Add(ItemLibrary.Instance.BackstoneOrePref, 1);
        return recipe;
    }

    public Crafter GetCrafterPrefhab()
    {
        return World.Loaded.FurcanePref;
    }

    public float GetTime()
    {
        return 5F;
    }
}
