using UnityEngine;
using Photon.Pun;

public class HandShuriken : Throwable
{
    [Header("Shuriken")]
    [SerializeField] private Shuriken ShurikenPref;

    public override void Attack(Ninja owner, Vector3 look)
    {
        Shuriken projctile = PhotonNetwork.Instantiate(ShurikenPref.name, transform.position, Quaternion.identity).GetComponent<Shuriken>();
        projctile.AttachProperties(owner, Damage, ProjectileSpeed);
        projctile.Shoot(look);
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.ShurikenPref;
    }
}
