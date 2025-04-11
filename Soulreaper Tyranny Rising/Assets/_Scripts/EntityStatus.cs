using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EntityStatus : MonoBehaviour
{

    public delegate void EntityStatusUpdate();

    public EntityStatusUpdate OnStatusUpdate;

    public delegate void DamageEvent(int amount);

    public delegate void HealEvent(int amount);

    public delegate void EnduranceUsed(float amount);

    public delegate void EnduranceGained(float amount);

    public DamageEvent OnDamaged;
    public DamageEvent OnHealed;
    public EnduranceUsed OnEnduranceUsed;
    public EnduranceGained OnEnduranceGained;

    public SkinnedMeshRenderer meshRenderer;
    [SerializeField] private Material flinchShieldMaterial;
    public bool useFlinchShield = false;

    public int CurrentLifePoints;
    public float CurrentEndurancePoints;
    public int MaxFlinchShield;
    public int CurrentFlinchShield;
    private bool Alive = false;

    public EntityStats stats;

    [SerializeField] protected CharacterStat MaxLifePoints, MaxEndurancePoints;
    [SerializeField] protected CharacterStat Evocation;
    [SerializeField] protected CharacterStat WeaponSkill;
    [SerializeField] protected CharacterStat EnergyProjection;
    [SerializeField] protected CharacterStat Thaumaturgy;
    [SerializeField] protected CharacterStat Artifice;
    [SerializeField] protected CharacterStat PhysicalDefense;
    [SerializeField] protected CharacterStat MagicalDefense;
    [SerializeField] protected CharacterStat AmbientEnduranceGain;

    [SerializeField] protected List<ElementType> Weakness, Resistance, Immunity, Absorbtion;

    [SerializeField] protected int CurrentUpgradeResource;

    public int CurrentUpgrades
    {
        get { return CurrentUpgradeResource; }
    }

    [SerializeField] float ExponentialPower;

    [SerializeField] public bool PoisonImmune, BleedImmune, DrainImmune, KnockdownImmune;

    StatModifier
        TwentyPercentAdditivePri = new StatModifier(.20f, StatModType.PercentAdd),
        TwentyPercentSubtractivePri = new StatModifier(.20f, StatModType.PercentSub),
        PoisonSub = new StatModifier(.30f, StatModType.PercentSub),
        CurseSub = new StatModifier(0.5f, StatModType.PercentSub),
        BlessingAdd = new StatModifier(0.5f, StatModType.PercentAdd);


    public Transform DamageNumberPoint;

    public delegate void EntityDeath();

    [FormerlySerializedAs("lightFlinch")]
    [SerializeField]
    private float lightFlinchAmount;

    [FormerlySerializedAs("heavyFlinch")]
    [SerializeField]
    private float heavyFlinchAmount;
    [SerializeField] float lightFlinchPercentage, heavyFlinchPercentage;

    private Vector3 currentDamagePosition;
    [SerializeField] private bool flinch = false;
    [SerializeField] private bool heavyFlinch = false;
    [SerializeField] private bool ignoreDamage = false;
    [SerializeField] private float ignoreDamageTime = 0f;

    public void Start()
    {

    }

    private void OnEnable()
    {
        if (!Alive)
        {
            Alive = true;
            UpdateStatus();
            Weakness = stats.Weakness;
            Resistance = stats.Resistance;
            Immunity = stats.Immunity;
            Absorbtion = stats.Absorbtion;
        }

    }

    public bool IsAlive() => Alive;

    public float GetCurrentLifePointsNormalized()
    {
        return CurrentLifePoints / MaxLifePoints.Value;
    }

    public float GetCurrentEndurancePointsNormalized()
    {
        return CurrentEndurancePoints / MaxEndurancePoints.Value;
    }

    public virtual void UpdateStatus()
    {
        MaxLifePoints.BaseValue = stats.MaxLifePoints.BaseValue;
        Evocation.BaseValue = stats.Violence.BaseValue;
        WeaponSkill.BaseValue = stats.Avarice.BaseValue;
        PhysicalDefense.BaseValue = stats.PhysicalDefense.BaseValue;
        MagicalDefense.BaseValue = stats.MagicalDefense.BaseValue;

        CurrentLifePoints = (int)MaxLifePoints.Value;
        CurrentEndurancePoints = (int)MaxEndurancePoints.Value;

        if (OnStatusUpdate != null)
        {
            OnStatusUpdate.Invoke();
        }

        lightFlinchAmount = MaxLifePoints.Value * lightFlinchPercentage;
        heavyFlinchAmount = MaxLifePoints.Value * heavyFlinchPercentage;
        CurrentFlinchShield = MaxFlinchShield;
    }

    void Update()
    {
        if (Alive)
        {
            CurrentEndurancePoints += (MaxEndurancePoints.Value * 0.01f) * (Time.deltaTime);
            if (CurrentEndurancePoints > MaxEndurancePoints.Value)
                CurrentEndurancePoints = MaxEndurancePoints.Value;
        }

        if (ignoreDamage)
        {
            ignoreDamageTime -= Time.deltaTime;
            if (ignoreDamageTime < 0) ignoreDamage = false;
        }
    }

    public virtual void UseEndurance(int amount)
    {
        CurrentEndurancePoints -= amount;

        if (OnEnduranceUsed != null)
        {
            OnEnduranceUsed.Invoke(CurrentEndurancePoints); // Will send the amount of Endurance left
        }
    }

    public virtual void UseSpark()
    {
        CurrentUpgradeResource--;
    }

    void IgnoreDamage(float time)
    {
        ignoreDamageTime = time;
        ignoreDamage = true;
    }

    #region Level Up

    public bool LevelUp(StatType type)
    {
        if (CurrentUpgradeResource == 0)
        {
            return false;
        }

        if (type == StatType.MaxLP)
        {
            int tempLP = (int)MaxLifePoints.BaseValue;
            tempLP *= (int)(1 * .15f);
            MaxLifePoints.BaseValue = tempLP;
        }

        if (type == StatType.MaxEP)
        {
            int tempEP = (int)MaxEndurancePoints.BaseValue;
            tempEP *= (int)(1 * .15f);
            MaxEndurancePoints.BaseValue = tempEP;
        }

        if (type == StatType.WeaponSkill)
        {
            WeaponSkill.BaseValue++;
        }

        if (type == StatType.Evocation)
        {
            Evocation.BaseValue++;
        }

        if (type == StatType.Thaumaturgy)
        {
            Thaumaturgy.BaseValue++;
        }

        if (type == StatType.Artifice)
        {
            Artifice.BaseValue++;
        }

        if (type == StatType.EnergyProjection)
        {
            EnergyProjection.BaseValue++;
        }

        if (type == StatType.PhysicalDefense)
        {
            if (PhysicalDefense.BaseValue >= 0.5f)
            {
                return false;
            }

            PhysicalDefense.BaseValue += 0.01f;
        }

        if (type == StatType.MagicalDefense)
        {
            if (MagicalDefense.BaseValue >= 0.5f)
            {
                return false;
            }

            MagicalDefense.BaseValue += 0.01f;
        }

        return true;
    }

    #endregion

    #region Get Private Data

    public float GetMaxLP()
    {
        return MaxLifePoints.Value;
    }

    public float GetMaxEP()
    {
        return MaxEndurancePoints.Value;
    }

    public float GetEvocation()
    {
        return Evocation.Value;
    }

    public float GetWeaponSkill()
    {
        return WeaponSkill.Value;
    }

    public float GetEnergyProjection()
    {
        return EnergyProjection.Value;
    }

    public float GetThaumaturgy()
    {
        return Thaumaturgy.Value;
    }

    public float GetArtifice()
    {
        return Artifice.Value;
    }

    public float GetPhysicalDefense()
    {
        return PhysicalDefense.Value;
    }

    public float GetMagicalDefense()
    {
        return MagicalDefense.Value;
    }

    public float GetEnduranceGain()
    {
        return AmbientEnduranceGain.Value;
    }

    public bool HasWeakness(ElementType element)
    {
        if (Weakness.Contains(element))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public bool HasResistance(ElementType element)
    {
        if (Resistance.Contains(element))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public bool HasImmunity(ElementType element)
    {
        if (Immunity.Contains(element))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public bool HasAbsorbtion(ElementType element)
    {
        if (Absorbtion.Contains(element))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    #endregion

    #region Edit Private Data

    #region Add Status Effects

    public void MaxLPUp()
    {
        MaxLifePoints.AddModifier(TwentyPercentAdditivePri);
    }

    public void MaxEPUp()
    {
        MaxEndurancePoints.AddModifier(TwentyPercentAdditivePri);
    }

    public void MaxLPDown()
    {
        MaxLifePoints.AddModifier(TwentyPercentSubtractivePri);
    }

    public void MaxEPDown()
    {
        MaxEndurancePoints.AddModifier(TwentyPercentSubtractivePri);
    }

    public void WeaponSkillUp()
    {
        WeaponSkill.AddModifier(TwentyPercentAdditivePri);
    }

    public void EvocationUp() // Increases Evocation by 20%
    {
        Evocation.AddModifier(TwentyPercentAdditivePri);
    }

    public void EnergyProjectionUp() // Increases Energy Projection by 20%
    {
        EnergyProjection.AddModifier(TwentyPercentAdditivePri);
    }

    public void WeaponSkillDown() // Decreases Weapon Skill by 20%
    {
        WeaponSkill.AddModifier(TwentyPercentSubtractivePri);
    }

    public void EvocationDown() // Decreases Evocation by 20%
    {
        Evocation.AddModifier(TwentyPercentSubtractivePri);
    }

    public void EnergyProjectionDown() // Decreases Energy Projection by 20%
    {
        EnergyProjection.AddModifier(TwentyPercentSubtractivePri);
    }

    public void PhysicalDefenseDown() // Decreases Physical defense by 20%
    {
        PhysicalDefense.AddModifier(TwentyPercentSubtractivePri);
    }

    public void MagicalDefenseDown() // Decreases Magical defense by 20%
    {
        MagicalDefense.AddModifier(TwentyPercentSubtractivePri);
    }

    public void PhysicalDefenseUp() // Increases Physical Defense by 20%
    {
        WeaponSkill.AddModifier(TwentyPercentSubtractivePri);
    }

    public void MagicalDefenseUp() // Increases Physical Defense by 20%
    {
        WeaponSkill.AddModifier(TwentyPercentAdditivePri);
    }

    public void Poison()
    {
        if (PoisonImmune) return;

        MaxLifePoints.AddModifier(PoisonSub);
        MaxEndurancePoints.AddModifier(PoisonSub);
        WeaponSkill.AddModifier(PoisonSub);
        Evocation.AddModifier(PoisonSub);
        EnergyProjection.AddModifier(PoisonSub);
        PhysicalDefense.AddModifier(PoisonSub);
        MagicalDefense.AddModifier(PoisonSub);
    }

    public void Curse()
    {
        MaxLifePoints.AddModifier(CurseSub);
        MaxEndurancePoints.AddModifier(CurseSub);
        WeaponSkill.AddModifier(CurseSub);
        Evocation.AddModifier(CurseSub);
        EnergyProjection.AddModifier(CurseSub);
        PhysicalDefense.AddModifier(CurseSub);
        MagicalDefense.AddModifier(CurseSub);
    }

    public void Blessing()
    {
        MaxLifePoints.AddModifier(BlessingAdd);
        MaxEndurancePoints.AddModifier(BlessingAdd);
        WeaponSkill.AddModifier(BlessingAdd);
        Evocation.AddModifier(BlessingAdd);
        EnergyProjection.AddModifier(BlessingAdd);
        PhysicalDefense.AddModifier(BlessingAdd);
        MagicalDefense.AddModifier(BlessingAdd);
    }

    public void Exponential()
    {

    }

    #endregion

    #region Remove Status Effects

    public void RemoveMaxLPUp()
    {
        MaxLifePoints.RemoveModifier(TwentyPercentAdditivePri);
    }

    public void RemoveMaxEPUp()
    {
        MaxEndurancePoints.RemoveModifier(TwentyPercentAdditivePri);
    }

    public void RemoveMaxLPDown()
    {
        MaxLifePoints.RemoveModifier(TwentyPercentSubtractivePri);
    }

    public void RemoveMaxEPDown()
    {
        MaxEndurancePoints.RemoveModifier(TwentyPercentSubtractivePri);
    }

    public void RemoveWeaponSkillUp()
    {
        WeaponSkill.RemoveModifier(TwentyPercentAdditivePri);
    }

    public void RemoveEvocationUp()
    {
        Evocation.RemoveModifier(TwentyPercentAdditivePri);
    }

    public void RemoveEnergyProjectionUp()
    {
        EnergyProjection.RemoveModifier(TwentyPercentAdditivePri);
    }

    public void RemoveWeaponSkillDown()
    {
        WeaponSkill.RemoveModifier(TwentyPercentSubtractivePri);
    }

    public void RemoveEvocationDown()
    {
        Evocation.RemoveModifier(TwentyPercentSubtractivePri);
    }

    public void RemoveEnergyProjectionDown()
    {
        EnergyProjection.RemoveModifier(TwentyPercentSubtractivePri);
    }

    public void RemovePhysicalDefenseDown()
    {
        PhysicalDefense.RemoveModifier(TwentyPercentSubtractivePri);
    }

    public void RemoveMagicalDefenseDown()
    {
        MagicalDefense.RemoveModifier(TwentyPercentSubtractivePri);
    }

    public void RemovePhysicalDefenseUp()
    {
        WeaponSkill.RemoveModifier(TwentyPercentSubtractivePri);
    }

    public void RemoveMagicalDefenseUp()
    {
        WeaponSkill.RemoveModifier(TwentyPercentAdditivePri);
    }

    public void RemovePoison()
    {
        MaxLifePoints.RemoveModifier(PoisonSub);
        MaxEndurancePoints.RemoveModifier(PoisonSub);
        WeaponSkill.RemoveModifier(PoisonSub);
        Evocation.RemoveModifier(PoisonSub);
        EnergyProjection.RemoveModifier(PoisonSub);
        PhysicalDefense.RemoveModifier(PoisonSub);
        MagicalDefense.RemoveModifier(PoisonSub);
    }

    public void RemoveCurse()
    {
        MaxLifePoints.RemoveModifier(CurseSub);
        MaxEndurancePoints.RemoveModifier(CurseSub);
        WeaponSkill.RemoveModifier(CurseSub);
        Evocation.RemoveModifier(CurseSub);
        EnergyProjection.RemoveModifier(CurseSub);
        PhysicalDefense.RemoveModifier(CurseSub);
        MagicalDefense.RemoveModifier(CurseSub);
    }

    public void RemoveBlessing()
    {
        MaxLifePoints.RemoveModifier(BlessingAdd);
        MaxEndurancePoints.RemoveModifier(BlessingAdd);
        WeaponSkill.RemoveModifier(BlessingAdd);
        Evocation.RemoveModifier(BlessingAdd);
        EnergyProjection.RemoveModifier(BlessingAdd);
        PhysicalDefense.RemoveModifier(BlessingAdd);
        MagicalDefense.RemoveModifier(BlessingAdd);
    }

    public void RemoveAllEfects()
    {
        RemoveMaxLPUp();
        RemoveMaxEPUp();
        RemoveMaxLPDown();
        RemoveMaxEPDown();
        RemovePhysicalDefenseDown();
        RemovePhysicalDefenseUp();
        RemoveMagicalDefenseDown();
        RemoveMagicalDefenseUp();
        RemoveEvocationDown();
        RemoveEvocationUp();
        RemoveWeaponSkillUp();
        RemoveWeaponSkillDown();
        RemoveEnergyProjectionDown();
        RemoveEnergyProjectionUp();
        RemovePoison();
        RemoveCurse();
        RemoveBlessing();
    }

    public void RemoveExponential() //: Increases stats Exponentially
    {
        // Not implemented yet
    }

    #endregion

    public void SetPoisonImmune(bool setting)
    {
        PoisonImmune = setting;
    }

    public void SetBleedImmune(bool setting)
    {
        BleedImmune = setting;
    }

    public void SetDrainImmune(bool setting)
    {
        DrainImmune = setting;
    }

    public void AddWeakness(ElementType element)
    {
        if (Weakness.Contains(element))
        {
            return;
        }

        else
        {
            Weakness.Add(element);
        }
    }

    public void AddResistance(ElementType element)
    {
        if (Resistance.Contains(element))
        {
            return;
        }

        else
        {
            Resistance.Add(element);
        }
    }

    public void AddImmunity(ElementType element)
    {
        if (Immunity.Contains(element))
        {
            return;
        }

        else
        {
            Immunity.Add(element);
        }
    }

    public void AddAbsorbtion(ElementType element)
    {
        if (Absorbtion.Contains(element))
        {
            return;
        }

        else
        {
            Absorbtion.Add(element);
        }
    }

    //
    public void RemoveWeakness(ElementType element)
    {
        if (!Weakness.Contains(element))
        {
            return;
        }

        else
        {
            Weakness.Remove(element);
        }
    }

    public void RemoveResistance(ElementType element)
    {
        if (!Resistance.Contains(element))
        {
            return;
        }

        else
        {
            Resistance.Remove(element);
        }
    }

    public void RemoveImmunity(ElementType element)
    {
        if (!Immunity.Contains(element))
        {
            return;
        }

        else
        {
            Immunity.Remove(element);
        }
    }

    public void RemoveAbsorbtion(ElementType element)
    {
        if (!Absorbtion.Contains(element))
        {
            return;
        }

        else
        {
            Absorbtion.Remove(element);
        }
    }

    #endregion

    #region Damage Reaction

    public bool Flinch() => flinch;

    public bool HeavyFlinch() => heavyFlinch;

    public void HandleReaction()
    {
        flinch = false;
        heavyFlinch = false;
    }

    public Vector3 GetDamagePosition() => currentDamagePosition;

    public string GetRelativePosition(Vector3 position)
    {
        Vector3 toTarget = position - transform.position;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float angle = Vector3.Angle(forward, toTarget); // Angle from forward direction
        float rightDot = Vector3.Dot(toTarget, right); // Check if left or right

        if (angle < 45) return "Front";
        if (angle > 135) return "Back";
        return (rightDot > 0) ? "Right" : "Left";
    }

    public void AddFlinchShield(bool shield, int amount)
    {
        if (shield)
        {
            useFlinchShield = true;
            MaxFlinchShield = amount;
            CurrentFlinchShield = amount;

            // Outline material is the last material
            if (flinchShieldMaterial == null)
                flinchShieldMaterial = meshRenderer.materials[meshRenderer.materials.Length - 1];

            // Activate outline material
            flinchShieldMaterial.SetFloat("Enabled", 1);
        }
        else
        {
            useFlinchShield = false;
        }
    }

    public void TakeDamage(int amount, Vector3 point)
    {
        if (!Alive) return;

        if (ignoreDamage && amount < 0) return;

        if (amount < 0) // Damage
        {
            CurrentLifePoints += amount;
            currentDamagePosition = point;
            CurrentFlinchShield += amount;

            if (CurrentFlinchShield <= 0)
            {
                if (useFlinchShield)
                    flinchShieldMaterial.SetFloat("Enabled", 0);

                if (Mathf.Abs(amount) >= heavyFlinchAmount) heavyFlinch = true;
                else if (Mathf.Abs(amount) >= lightFlinchAmount) flinch = true;
            }

            //Display Damage Amount
        }
        else // Healing
        {
            CurrentLifePoints += amount;
            currentDamagePosition = point;
            CurrentFlinchShield += amount;
            if (CurrentFlinchShield > MaxFlinchShield) CurrentFlinchShield = MaxFlinchShield;
            //Display Damage Amount
        }

        if (CurrentLifePoints <= 0) Alive = false;
    }

    /// <summary>
    /// This version is used for simple damage from sources like status effects
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(int amount)
    {
        if (!Alive) return;
        if (ignoreDamage && amount < 0) return;

        CurrentLifePoints += amount;
    }

    void FlinchAnimationOver()
    {
        // Not Used anymore, will remove later
    }

    public void EnduranceGain(float amount)
    {
        CurrentEndurancePoints += amount;
        if (CurrentEndurancePoints > MaxEndurancePoints.Value)
        {
            CurrentEndurancePoints = MaxEndurancePoints.Value;
        }
    }

    #endregion
}