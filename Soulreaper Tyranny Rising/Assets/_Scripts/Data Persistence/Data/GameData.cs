using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool HypermodeUnlocked;
    public bool useGamePad, invertMouseH, invertMouseV, invertGamepadH,
        invertGamepadV;

    public int MaximumHypermodeAttacks;
    public int CurrentUpgradeResources;

    public int MaxLifePoints;
    public int Evocation;
    public int Violence;
    public float PhysicalDefense;
    public float MagicalDefense;

    [SerializeField]
    public List<ElementType> Weakness, Resistance, Immunity, Absorbtion;

    public float ExponentialPower;

    public bool PoisonImmune, IgniteImmune, FreezeImmune, CurseImmune;

    /*
    StatModifier
        TwentyPercentAdditivePri = new StatModifier(.20f, StatModType.PercentAdd),
        TwentyPercentSubtractivePri = new StatModifier(.20f, StatModType.PercentSub),
        PoisonSub = new StatModifier(.30f, StatModType.PercentSub);
    */

    public GameData()
    {
        MaxLifePoints = 100;
        Evocation = 1;
        Violence = 1;
        PhysicalDefense = 0.05f;
        MagicalDefense = 0.05f;

        Weakness = new List<ElementType>();
        Resistance = new List<ElementType>();
        Immunity = new List<ElementType>();
        Absorbtion = new List<ElementType>();

        ExponentialPower = 1.1f;

        HypermodeUnlocked = false;
        useGamePad = false;
        invertMouseH = false;
        invertMouseV = false;
        invertGamepadH = false;
        invertGamepadV = false;

        PoisonImmune = false;
        IgniteImmune = false;
        FreezeImmune = false;
        CurseImmune = false;

        MaximumHypermodeAttacks = 3;
        CurrentUpgradeResources = 5;
    }
}


