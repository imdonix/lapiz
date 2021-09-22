using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NatureSword : Tool, ICraftable
{
    private const string ID = "naturesword";

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.NatureSwordPref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().NatureSword;
    }

    public override ToolType GetToolType()
    {
        return ToolType.Sword;
    }

    public Recipe GetRecipe()
    {
        Recipe recipe = Recipe.Create(3);
        recipe.Add(ItemLibrary.Instance.SwordPref, 1);
        recipe.Add(ItemLibrary.Instance.LapizIngotPref, 1);
        recipe.Add(ItemLibrary.Instance.BackstonePref, 1);
        return recipe;
    }

    public float GetTime()
    {
        return 30;
    }

    public Crafter GetCrafterPrefhab()
    {
        return World.Loaded.WorkstationPref;
    }
}

