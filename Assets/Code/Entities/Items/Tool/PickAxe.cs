using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PickAxe : Tool, ICraftable
{
    private const string ID = "pickaxe";

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.PickaxePref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().PickAxe;
    }

    public override ToolType GetToolType()
    {
        return ToolType.Tool;
    }

    public Crafter GetCrafterPrefhab()
    {
        return World.Loaded.WorkstationPref;
    }

    public Recipe GetRecipe()
    {
        Recipe recipe = Recipe.Create(2);
        recipe.Add(ItemLibrary.Instance.StickPref, 1);
        recipe.Add(ItemLibrary.Instance.BackstonePref, 2);
        return recipe;
    }

    public float GetTime()
    {
        return 10F;
    }

    public override ItemQuality GetQuality()
    {
        return ItemQuality.COMMON;
    }
}

