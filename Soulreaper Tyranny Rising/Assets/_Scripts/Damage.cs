using UnityEngine;

[System.Serializable]
public struct Damage
{
    public ElementType ElementType;
    public NegativeEffectType NegativeEffectType;
    public EntityStatus myEntityStatus;
    public int attackPower, statusPower, statusBuild;
    public float attackPotencyMin, attackPotencyMax, pushForce;
    [Tooltip("Denotes if this attack will knockdown those struck")]
    public bool knockdown, push;

    public Damage(EntityStatus sender, ElementType attackElement, NegativeEffectType negEffect, int atkPower, int stPower, float potencyMin, float potencyMax = 1f, int pushFrc = 0)
    {
        myEntityStatus = sender;
        ElementType = attackElement;
        NegativeEffectType = negEffect;
        attackPower = atkPower;
        statusPower = stPower;
        statusBuild = 0;
        attackPotencyMin = potencyMin;
        attackPotencyMax = potencyMax;
        pushForce = pushFrc;
        knockdown = false;
        push = false;
    }
}
