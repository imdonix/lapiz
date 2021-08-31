using System.Collections;
using UnityEngine;


public class HandSword : Weapon
{
    [Header("Properties")]

    [SerializeField] protected float Speed;
    [SerializeField] protected float DamageApplied;
    [SerializeField] protected float Equip;


    public override float GetSpeed()
    {
        return Speed;
    }

    public override float GetDamageTime()
    {
        return DamageApplied;
    }

    public override float GetEquipTime()
    {
        return Equip;
    }

    public override void Attack(Ninja owner, Vector3 look)
    {
        Body body = owner.GetBody();
        Vector3 hitPos = body.transform.position + body.transform.forward;
        RaycastHit[] hits = Physics.BoxCastAll(hitPos, Vector3.one / 2, body.transform.forward, Quaternion.identity, 0.75F);
        foreach (RaycastHit hit in hits)
        {
            LivingEntity entity = hit.collider.GetComponentInParent<LivingEntity>();
            if (!ReferenceEquals(entity, null))
                if(entity != owner)
                    entity.Damage(owner, Damage);           
        }
    }
}
