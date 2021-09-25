using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Fireball : Entity, IPunInstantiateMagicCallback
{
    private const float FORCE = 8F;

    private ParticleSystem particle;
    private float lifeTime = 0;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();

        particle = GetComponent<ParticleSystem>();
    }

    protected override void Update()
    {
        base.Update();

        lifeTime += Time.deltaTime;
        if (lifeTime > 2) Destroy(gameObject);
    }

    #endregion

    public override bool HasTag()
    {
        return false;
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Vector3 direction = (Vector3) info.photonView.InstantiationData[0];
        float range = (float) info.photonView.InstantiationData[1];
        var system = particle.forceOverLifetime;
        system.x = direction.x * range * FORCE;
        system.y = direction.y * range * FORCE;
        system.z = direction.z * range * FORCE;
    }

    public void DoDamage(Ninja owner, float range, Vector3 direction, float damage, float burnTime)
    {
        Head head = owner.GetHead();
        Vector3 hitPos = head.transform.position + head.transform.forward;
        RaycastHit[] hits = Physics.BoxCastAll(hitPos, Vector3.one, head.transform.forward, Quaternion.identity, range);
        foreach (RaycastHit hit in hits)
        {
            LivingEntity entity = hit.collider.GetComponentInParent<LivingEntity>();
            if (!ReferenceEquals(entity, null))
                if (entity != owner)
                {
                    entity.Damage(owner, damage);
                }

        }


    }

}

