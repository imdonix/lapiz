using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IronIngot : Item, ICraftable
{
    private const string ID = "ingot_iron";

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

    public Item GetItemPref()
    {
        return ItemLibrary.Instance.IronIngotPref;
    }
}

