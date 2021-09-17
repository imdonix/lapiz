using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveStateDisplay : MonoBehaviour
{
    [SerializeField] public StatusBar Health;
    [SerializeField] public StatusBar Chakra;

    public void UpdateStatus(float hp, float hpMax, float chakra, float chakraMax)
    {
        Health.UpdateStatu(hp, hpMax);
        Chakra.UpdateStatu(chakra, chakraMax);
    }
}
