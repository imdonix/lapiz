using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class HandTool : Weapon
{
    private const float SPEED = 1F;

    public override void Attack(Ninja owner, Vector3 look)
    {
        Body body = owner.GetBody();
        Vector3 hitPos = body.transform.position + body.transform.forward;
        RaycastHit[] hits = Physics.BoxCastAll(hitPos, Vector3.one / 2, body.transform.forward, Quaternion.identity, 0.75F);
        foreach (RaycastHit hit in hits)
        {
            IDamagable entity = hit.collider.GetComponentInParent<IDamagable>();
            IHarvestable harvestable = hit.collider.GetComponentInParent<IHarvestable>();

            if (!ReferenceEquals(entity, null))
                if (entity != (IDamagable) owner)
                    entity.Damage(owner, Damage);

            if (!ReferenceEquals(harvestable, null))
            {
                Item item;
                harvestable.Harvest(owner, this, out item);
            }
        }
    }

    public override float GetDamageTime()
    {
        return 1F;
    }

    public override float GetEquipTime()
    {
        return SPEED;
    }
}

