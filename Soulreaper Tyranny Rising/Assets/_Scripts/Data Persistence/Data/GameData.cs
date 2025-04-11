using Kryz.CharacterStats;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool HypermodeUnlocked;
    public bool useGamePad, invertMouseH, invertMouseV, invertGamepadH,
        invertGamepadV, forceSlotTwoUnlocked, forceSlotThreeUnlocked;

    public int MaximumHypermodeAttacks;
    public int Current_UpgradeResources;

    public int MaxLifePoints, MaxEndurancePoints;
    public int Evocation;
    public int Violence;
    public int EnergyProjection;
    public int Thaumaturgy;
    public float PhysicalDefense;
    public float MagicalDefense;
    public float BlockedDamagePercentage;
    public int AmbientEnduranceGain;

    [SerializeField]
    public List<ElementType> Weakness, Resistance, Immunity, Absorbtion;

    public float ExponentialPower;

    public bool PoisonImmune, DegenImmune, DrainImmune, FreezeImmune, CurseImmune;

    /*
    StatModifier
        TwentyPercentAdditivePri = new StatModifier(.20f, StatModType.PercentAdd),
        TwentyPercentSubtractivePri = new StatModifier(.20f, StatModType.PercentSub),
        PoisonSub = new StatModifier(.30f, StatModType.PercentSub);
    */

    public GameData()
    {
        MaxLifePoints = 100;
        MaxEndurancePoints = 100;
        Evocation = 1;
        Violence = 1;
        EnergyProjection = 1;
        Thaumaturgy = 1;
        PhysicalDefense = 0.05f;
        MagicalDefense = 0.05f;
        AmbientEnduranceGain = 1;
        BlockedDamagePercentage = 0.5f;

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
        forceSlotTwoUnlocked = false;
        forceSlotThreeUnlocked = false;

        PoisonImmune = false;
        DegenImmune = false;
        DrainImmune = false;
        FreezeImmune = false;
        CurseImmune = false;

        MaximumHypermodeAttacks = 3;
        Current_UpgradeResources = 5;
    }
}


