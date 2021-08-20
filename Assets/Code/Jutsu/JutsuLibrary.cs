using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JutsuLibrary : MonoBehaviour
{
    public static JutsuLibrary Instance;

    [Header("JutsuList")]
    [SerializeField] private List<Jutsu> Jutsus;

    #region PRIVATE

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public bool ExistActivation(HandSeal[] seals, out Jutsu activated)
    {
        foreach (Jutsu spell in Jutsus)
            if (spell.IsSubSubsequent(seals))
            {
                activated = spell;
                return true;
            }
        activated = null;
        return false;
    }

}
