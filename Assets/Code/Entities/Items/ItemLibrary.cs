using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemLibrary : MonoBehaviour
{
    public static ItemLibrary Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    [Header("ItemLibrary")]
    [SerializeField] public Lapiz LapizPref;
    [SerializeField] public IronOre IronOrePref;
    [SerializeField] public IronIngot IronIngotPref;
    [SerializeField] public LapizIngot LapizIngotPref;


    public List<ICraftable> GetCraftablePrefs(Crafter crafter) 
    {
        List<ICraftable> craftables = new List<ICraftable>();
        foreach (var field in typeof(ItemLibrary).GetFields())
        {
            if (field.GetValue(this) is ICraftable)
            {
                ICraftable craftable = (ICraftable) field.GetValue(this);
                if(craftable.GetCrafterPrefhab().Equals(crafter))
                    craftables.Add((ICraftable) field.GetValue(this));
            }
        }
        return craftables;
    }

}

