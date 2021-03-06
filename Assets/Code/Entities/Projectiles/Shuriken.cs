using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Shuriken : Projectile
{
    private float damage;
    private float projectileSpeed;
    private LivingEntity owner;

    private Rigidbody rigid;

    private float rotStatus = 0;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();

        rigid = GetComponent<Rigidbody>();

        if (!photonView.IsMine) Destroy(rigid);
    }

    protected override void Update()
    {
        base.Update();

        if (!photonView.IsMine || impacted) return;

        transform.rotation = Quaternion.Euler(0, rotStatus % 360, 90F);
        rotStatus += Time.deltaTime * 360;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        IDamagable target = other.GetComponent<IDamagable>();
        if (!ReferenceEquals(target, null)) 
        {
            target.Damage(owner, damage);
            DestroyProjectile();
        } 

        impacted = true;
        Destroy(rigid);
    }

    #endregion


    public void AttachProperties(LivingEntity owner, float damage, float projectileSpeed)
    {
        this.owner = owner;
        this.damage = damage;
        this.projectileSpeed = projectileSpeed;
    }

    public void Shoot(Vector3 look)
    {
        rigid.AddForce(look * projectileSpeed);
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }

}
