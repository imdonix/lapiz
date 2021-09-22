using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ItemComp : MonoBehaviour
{
    public const float SIZE = 225;
    public const float BORDER = 45;
    private Color[] colors = new Color[4] { Color.gray, Color.green, Color.blue, Color.yellow };

    [SerializeField] private Text nameText;
    [SerializeField] private Image image;
    [SerializeField] private Image quality;
    [SerializeField] private Text ammount;
    [SerializeField] private GameObject holder;

    private RectTransform trans;
    private Item showed;

    private void Awake()
    {
        trans = GetComponent<RectTransform>();
        trans.sizeDelta = Vector2.one * (SIZE - BORDER);
    }

    public void SetItem(Item item)
    {
        SetColor(item);
        SetSprite(item.GetIcon());
        nameText.text = item.GetName();
        holder.SetActive(false);
        this.showed = item;
    }

    public void SetItem(Item item, int n)
    {
        SetItem(item);
        SetBadge(n.ToString());
    }

    public void SetItem(ItemStack stack)
    {
        SetItem(stack.Prefab, stack.Items.Count);
    }

    public void SetSprite(Sprite sprite)
    {
        SetSprite(sprite, string.Empty);
    }

    public void SetSprite(Sprite sprite, string text)
    {
        this.showed = null;
        nameText.text = text;
        image.sprite = sprite;
        holder.SetActive(false);
    }

    public float GetSize()
    {
        return SIZE;
    }

    public void SetPosition(Vector2 p)
    {
        trans.anchoredPosition = p;
    }

    public void SetBadge(string text)
    {
        holder.SetActive(true);
        ammount.text = text;
    }

    private void SetColor(Item item)
    {
        int q = (int) item.GetQuality();
        quality.color = colors[q];
    }

    #region UI


    public void OnClick()
    {
        if (ReferenceEquals(showed, null)) return;

        if(showed is ICraftable)
            HUD.Instance.ShowCraftRecipe((ICraftable) showed);
    }


    #endregion
}
