using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BaseSword : Tool, ICraftable
{
    private const string ID = "sword";

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.SwordPref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Sword;
    }

    public Crafter GetCrafterPrefhab()
    {
        return World.Loaded.WorkstationPref;
    }

    public Recipe GetRecipe()
    {
        Recipe recipe = Recipe.Create(1);
        recipe.Add(ItemLibrary.Instance.IronIngotPref, 2);
        return recipe;
    }

    public float GetTime()
    {
        return 5F;
    }

    public override ToolType GetToolType()
    {
        return ToolType.Sword;
    }

    public override ItemQuality GetQuality()
    {
        return ItemQuality.COMMON;
    }
}

