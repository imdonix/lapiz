using System.Collections;
using UnityEngine;


public class BlinkJutsu : Jutsu
{
    [Header("Blink")]
    [SerializeField] private float Range;

    protected override void OnCast(Ninja caster, Vector3 from, Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(from, direction, out hit, Range - 1F))
            caster.Teleport(from + (direction * (hit.distance - 1F)));
        else
            caster.Teleport(from + (direction * (Range - 1F)));
    }

    public override bool HasTag()
    {
        return false;
    }
}