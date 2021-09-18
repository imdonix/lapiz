using System.Collections;
using UnityEngine;


public class Stick : Item
{
    public override string GetID()
    {
        return "stick";
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.StickPref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Stick;
    }
}
