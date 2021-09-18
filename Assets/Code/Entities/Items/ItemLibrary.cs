using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemLibrary : MonoBehaviour
{

    public static ItemLibrary Instance;
    private static List<Item> cache;

    private void Awake()
    {
        Instance = this;
        InitItemList();
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    [Header("ItemLibrary")]
    [SerializeField] public Lapiz LapizPref;
    [SerializeField] public IronOre IronOrePref;
    [SerializeField] public IronIngot IronIngotPref;
    [SerializeField] public LapizIngot LapizIngotPref;
    [SerializeField] public ChakraMoon ChakraMoonPref;
    [SerializeField] public HighBlade HighBladePref;
    [SerializeField] public NatureSword NatureSwordPref;
    [SerializeField] public ShurikenItem ShurikenPref;
    [SerializeField] public BaseSword SwordPref;
    [SerializeField] public Axe AxePref;
    [SerializeField] public PickAxe PickaxePref;
    [SerializeField] public Stick StickPref;
    

    private void InitItemList()
    {
        ItemLibrary.cache = new List<Item>();
        foreach (var field in typeof(ItemLibrary).GetFields())
        {
            object fieldValue = field.GetValue(this);
            if(fieldValue is Item)
                ItemLibrary.cache.Add((Item)fieldValue);
        }            
    }

    public List<ICraftable> GetCraftablePrefs(Crafter crafter) 
    {
        List<ICraftable> craftables = new List<ICraftable>();
        foreach (Item item in ItemLibrary.cache)
        {
            if (item is ICraftable)
            {
                ICraftable craftable = (ICraftable) item;
                if(craftable.GetCrafterPrefhab().Equals(crafter))
                    craftables.Add((ICraftable) item);
            }
        }
        return craftables;
    }

    public Item FindByID(string itemID)
    {
        foreach (Item item in ItemLibrary.cache)
            if (item.GetID().Equals(itemID))
                return item;
        Debug.LogWarning(string.Format("Item not found with {0}", itemID));
        return null;
    }

}

