using Photon.Pun;
using UnityEngine;

public class FireballJutsu : Jutsu
{
    [Header("Fireball")]
    [SerializeField] private float damage;
    [SerializeField] private float burnTime;
    [SerializeField] private float Range;

    [SerializeField] private Fireball Fireball;

    protected override void OnCast(Ninja caster, Vector3 from, Vector3 direction)
    {
        object[] args = new object[2] { direction, Range };
        Fireball fire = PhotonNetwork.Instantiate(Fireball.name, from, Quaternion.identity, 0, args).GetComponent<Fireball>();
        fire.DoDamage(caster, Range, direction, damage, burnTime);

        
    }
}

