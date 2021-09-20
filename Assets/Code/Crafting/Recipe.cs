using System;
using System.Collections.Generic;

public class Recipe
{
    private readonly Pair<Item, int>[] ingredients;
    private int i;

    private Recipe(int piece)
    {
        this.ingredients = new Pair<Item, int>[piece];
        this.i = 0;
    }

    public void Add(Item ingredient, int amount)
    {
        if (i >= ingredients.Length) 
            throw new Exception("You cant add mote ingredient to this Recipe");

        ingredients[i++] = new Pair<Item, int>(ingredient, amount);
    }

    public bool TryCraft(List<ItemStack> items, out List<ItemStack> toBeConsumed)
    {
        toBeConsumed = new List<ItemStack>();
        foreach(Pair<Item, int> ingredient in ingredients)
        {
            bool exist = false;
            foreach (ItemStack item in items)
                if (ingredient.key.Equals(item.Prefab) && item.Count >= ingredient.value)
                {
                    exist = true;
                    toBeConsumed.Add(item.Split(ingredient.value));
                    break;
                }

            if (!exist) return false;
        }
        return true;
    }

    public Pair<Item, int>[] GetIngredients()
    {
        return ingredients;
    }

    public static Recipe Create(int piece)
    {
        return new Recipe(piece);
    }

}


