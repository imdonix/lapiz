using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LapizIngot : Item, ICraftable
{
    private const string ID = "ingot_lapiz";
    private const float CRAFT_TIME = 15;

    public override string GetID()
    {
        return ID;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().LapizIngot;
    }

    public Recipe GetRecipe()
    {
        Recipe recipe = Recipe.Create(2);
        recipe.Add(ItemLibrary.Instance.IronIngotPref, 3);
        recipe.Add(ItemLibrary.Instance.LapizPref, 2);
        return recipe;
    }

    public Crafter GetCrafterPrefhab()
    {
        return World.Loaded.FurcanePref;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.LapizIngotPref;
    }

    public float GetTime()
    {
        return CRAFT_TIME;
    }
}

