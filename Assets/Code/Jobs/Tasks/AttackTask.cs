using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AttackTask : TargetedTask
{
    private readonly bool attackInPlace;

    public AttackTask(Ninja owner, LivingEntity target, bool attackInPlace) : base(owner, target)
    {
        this.attackInPlace = attackInPlace;
    }

    private float stay = 0;

    protected override void DoInit()
    {
        if (CheckDead()) return;

        Weapon weapon = owner.GetArms().GetWeaponInHand();
        if (ReferenceEquals(weapon, null))
        {
            Fail();
        }
        else 
        {
            owner.GetArms().Attack();

            if (attackInPlace)
                stay = weapon.GetSpeed() + .1F;
            else
                Succeed();
        }
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();

        stay -= Time.deltaTime;
        if (stay < 0)
            Succeed();

    }
}

