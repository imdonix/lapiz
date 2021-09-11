using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Story/Wave", order = 1)]
public class Wave : ScriptableObject
{
    [SerializeField] public LivingEntity[] Entities;
}
