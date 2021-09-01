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
}

