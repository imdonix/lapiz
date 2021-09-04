using System.Collections.Generic;

public class Recipe
{
    private readonly Pair<Item, byte>[] ingredients;
    private int i;

    private Recipe(int piece)
    {
        this.ingredients = new Pair<Item, byte>[piece];
        this.i = 0;
    }

    public void Add(Item ingredient, byte amount)
    {
        ingredients[i++] = new Pair<Item, byte>(ingredient, amount);
    }

    public bool IsMatch(List<Pair<Item, byte>> items)
    {
        foreach (Pair<Item, byte> ingredient in ingredients)
        {
            bool exist = false;

            foreach (Pair<Item, byte> item in items)
                if (ingredient.key.Equals(item.key) && item.value >= ingredient.value)
                {
                    exist = true;
                    break;
                }

            if (!exist) return false;
        }
        return true;
    }

    public static Recipe Create(int piece)
    {
        return new Recipe(piece);
    }

}


