using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ItemComp : MonoBehaviour
{
    public const float SIZE = 225;
    public const float BORDER = 45;

    [SerializeField] private Text nameText;
    [SerializeField] private Image image;
    [SerializeField] private Text ammount;
    [SerializeField] private GameObject holder;

    private RectTransform trans;

    private void Awake()
    {
        trans = GetComponent<RectTransform>();
        trans.sizeDelta = Vector2.one * (SIZE - BORDER);
    }

    public void SetItem(Item item)
    {
        nameText.text = item.GetName();
        image.sprite = item.GetIcon();
        holder.SetActive(false);
    }

    public void SetItem(ItemStack stack)
    {
        SetItem(stack.Prefab);
        holder.SetActive(true);
        ammount.text = stack.Count.ToString();
    }

    public float GetSize()
    {
        return SIZE;
    }

    public void SetPosition(Vector2 p)
    {
        trans.anchoredPosition = p;
    }
}
