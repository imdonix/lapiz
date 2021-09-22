using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Axe : Tool, ICraftable
{
    private const string ID = "axe";

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.AxePref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Axe;
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
        recipe.Add(ItemLibrary.Instance.StickPref, 2);
        recipe.Add(ItemLibrary.Instance.BackstonePref, 1);
        return recipe;
    }

    public float GetTime()
    {
        return 10F;
    }

}

