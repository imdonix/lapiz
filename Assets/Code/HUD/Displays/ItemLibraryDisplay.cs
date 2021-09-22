using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemLibraryDisplay : Display
{
    [SerializeField] private ItemComp ItemCompPref;

    protected override void OnInit()
    {
        List<Item> items = ItemLibrary.Instance.GetAll();
        int n = items.Count;
        int q = NearestSqrNumber(n);
        float size = ItemCompPref.GetSize();
        float shift = (size * (q-1)) / 2;

        for (int i = 0; i < q; i++)
            for (int j = 0; j < q && (i * q) + j < n; j++)
            {
                ItemComp comp = Instantiate(ItemCompPref, transform).GetComponent<ItemComp>();
                comp.SetItem(items[((i * q) + j)]);
                comp.SetPosition(new Vector2(j * size - shift, i * size - shift));
            }
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        HUD.Instance.Craft.ResetStack();
    }

    private static int NearestSqrNumber(int n)
    {
        int i = 1;
        while (n / i >= i)
            i++;
        return i;
    }
}
