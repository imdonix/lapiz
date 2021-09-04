using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface ICraftable
{
    public Recipe GetRecipe();

    public Item GetItemPref();

    public Crafter GetCrafterPrefhab();
}
