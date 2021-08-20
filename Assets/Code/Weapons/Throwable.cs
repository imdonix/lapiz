using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Throwable : Weapon
{
    [Header("Throwable")]
    [SerializeField] protected float ProjectileSpeed;

    public override float GetDamageTime()
    {
        return 1;
    }

    public override float GetEquipTime()
    {
        return 0.15F;
    }

    public override float GetSpeed()
    {
        return 0.275F;
    }
}

