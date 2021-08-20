using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] protected float Damage;

    /// <summary>
    /// Store the attack interpolation rotation in pair (start, end)
    /// </summary>
    [SerializeField] protected Vector3[] AttackRotations;


    protected int state;
    protected int animCount;

    #region UNITY

    private void Awake()
    {
        Assert();
        this.state = 0;
        this.animCount = AttackRotations.Length / 2;
    }

    #endregion

    public (Vector3, Vector3) GetNext()
    {
        
        int i = (state % animCount) * 2;
        state++;
        return (AttackRotations[i], AttackRotations[i + 1]);
    }


    public Vector3 GetFirstPos()
    {
        state = 0;
        return AttackRotations[0];
    }

    /// <summary>
    /// Return the full animation time; (Slower is faster)
    /// </summary>
    public abstract float GetSpeed();

    /// <summary>
    /// Return the time when the damage should be applied in the animation;
    /// 0 at the begginig, 1 at the end
    /// </summary>
    public abstract float GetDamageTime();

    /// <summary>
    /// The animation time to take when equiping;
    /// </summary>
    /// <returns></returns>
    public abstract float GetEquipTime();

    /// <summary>
    /// The attack;
    /// </summary>
    /// <returns></returns>
    public abstract void Attack(LivingEntity owner, Vector3 look);


    #region PRIVATE

    private void Assert()
    {
        if (AttackRotations.Length % 2 == 1) throw new Exception(name + " Position are not in pair");
        if (AttackRotations.Length < 2) throw new Exception( name + " dont have at least one positions");
    }

    #endregion

}

