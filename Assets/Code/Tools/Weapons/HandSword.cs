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
        RaycastHit[] hits = Physics.BoxCastAll(hitPos, Vector3.one / 2, body.transform.forward * 1.5F, Quaternion.identity, 0.75F);
        foreach (RaycastHit hit in hits)
        {
            IDamagable entity = hit.collider.GetComponentInParent<IDamagable>();
            if (!ReferenceEquals(entity, null))
                if(entity != (IDamagable) owner)
                    entity.Damage(owner, Damage);           
        }
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.SwordPref;
    }
}
