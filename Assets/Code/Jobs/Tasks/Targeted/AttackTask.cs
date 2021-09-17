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

        target.Alert(owner);

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

        if (CheckDead()) return;

        stay -= Time.deltaTime;
        if (stay < 0)
            Succeed();

        Vector3 highlessMe = new Vector3(owner.transform.position.x, 0, owner.transform.position.z);
        Vector3 highlessTarget = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        Vector3 direction = (highlessTarget - highlessMe).normalized;

        owner.MoveBodyOnly(direction);
    }
}

