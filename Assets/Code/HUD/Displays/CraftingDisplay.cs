using System.Collections.Generic;
using UnityEngine;


public class CraftingDisplay : Display
{
    [SerializeField] private ItemComp ItemCompPref;
    [SerializeField] private Sprite PlusSpritePref;
    [SerializeField] private Sprite EqSpritePref;

    private ICraftable craftable = null;
    private List<ItemComp> cache = new List<ItemComp>();
    private Stack<ICraftable> stack = new Stack<ICraftable>();

    public void Render(ICraftable craftable)
    {
        this.craftable = craftable;
        stack.Push(this.craftable);
    }

    public void ResetStack()
    {
        stack.Clear();
    }

    protected override void OnOpen()
    {
        if (ReferenceEquals(null, this.craftable)) return;

        Recipe recipe = craftable.GetRecipe();
        var ing = recipe.GetIngredients();
        var size = ItemComp.SIZE;
       
        int n = ing.Length + (ing.Length - 1) + 1;
        int index = 0;
        float half = (size * n) / 2;
        for (int i = 0; i < ing.Length; i++)
        {
            var item = Component();
            item.SetPosition(Vector2.right * (index++ * size - half));
            item.SetItem(ing[i].key, ing[i].value);

            if (i + 1 < ing.Length)
            {
                var plus = Component();
                plus.SetPosition(Vector2.right * (index++ * size - half));
                plus.SetSprite(PlusSpritePref);
            }
        }

        var eq = Component();
        eq.SetPosition(Vector2.right * (index++ * size - half));
        eq.SetSprite(EqSpritePref, craftable.GetCrafterPrefhab().GetName());
        eq.SetBadge(Mathf.RoundToInt(craftable.GetTime()).ToString());

        var res = Component();
        res.SetPosition(Vector2.right * (index++ * size - half));
        res.SetItem(craftable.GetItemPref());
    }

    protected override void OnClose()
    {
        foreach (ItemComp comp in this.cache)
            Destroy(comp.gameObject);
        this.cache.Clear();
    }

    protected override void OnBack()
    {
        if (stack.Count > 1)
        {
            stack.Pop();
            HUD.Instance.ShowCraftRecipe(stack.Pop());
        }
        else
            base.OnBack();
    }

    private ItemComp Component()
    {
        ItemComp comp = Instantiate(ItemCompPref, transform).GetComponent<ItemComp>();
        this.cache.Add(comp);
        return comp;
    }
}
