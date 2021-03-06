using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IronIngot : Item, ICraftable
{
    private const string ID = "ingot_iron";
    private const float CRAFT_TIME = 5;

    public override string GetID()
    {
        return ID;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().IronIngot;
    }

    public Recipe GetRecipe()
    {
        Recipe recipe = Recipe.Create(1);
        recipe.Add(ItemLibrary.Instance.IronOrePref, 3);
        return recipe;
    }

    public Crafter GetCrafterPrefhab()
    {
        return World.Loaded.FurcanePref;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.IronIngotPref;
    }

    public float GetTime()
    {
        return CRAFT_TIME;
    }

    public override ItemQuality GetQuality()
    {
        return ItemQuality.RARE;
    }
}

