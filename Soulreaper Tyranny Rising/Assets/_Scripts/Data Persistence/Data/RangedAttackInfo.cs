using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Ranged Attack", menuName = "Ranged Attack Info")]
public class RangedAttackInfo : ScriptableObject
{
    public int number;
    public GameObject BulletPrefab;
    public int enduranceCost;
    public Damage BulletDamage;
    public bool unlocked = false;
}
