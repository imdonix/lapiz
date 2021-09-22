using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HighBlade : Tool, ICraftable
{
    private const string ID = "highblade";


    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.HighBladePref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().HighBlade;
    }

    public override ToolType GetToolType()
    {
        return ToolType.Sword;
    }

    public Crafter GetCrafterPrefhab()
    {
        return World.Loaded.FurcanePref;
    }

    public Recipe GetRecipe()
    {
        Recipe recipe = Recipe.Create(2);
        recipe.Add(ItemLibrary.Instance.SwordPref, 1);
        recipe.Add(ItemLibrary.Instance.BackstonePref, 3);
        return recipe;
    }

    public float GetTime()
    {
        return 20F;
    }
}

