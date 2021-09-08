using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ChakraMoon : Tool, ICraftable
{
    private const string ID = "chakramoon";

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.ChakraMoonPref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().ChakraMoon;
    }

    public Crafter GetCrafterPrefhab()
    {
        return World.Loaded.FurcanePref;
    }

    public Recipe GetRecipe()
    {
        Recipe recipe = Recipe.Create(2);
        recipe.Add(ItemLibrary.Instance.LapizIngotPref, 3);
        recipe.Add(ItemLibrary.Instance.IronIngotPref, 1);
        return recipe;
    }

    public float GetTime()
    {
        return 30F;
    }

    public override ToolType GetToolType()
    {
        return ToolType.Sword;
    }
}

