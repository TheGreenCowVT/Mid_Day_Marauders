using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon Info")]
public class WeaponInfo : ScriptableObject
{
    public GameObject WeaponPrefab;
    public GameObject WeaponAttackEastPrefab = null;
    public GameObject WeaponAttackWestPrefab = null;
    public GameObject WeaponAttackSouthPrefab = null;
    public GameObject WeaponAttackNorthPrefab = null;

    [Tooltip("Time till second attack can be used.")]
    public float First_to_Second_Time = 0.3f;

    [Tooltip("Time till East Weapon Attack Prefab spawn.")]
    public float WeaponAttackEastPrefabTime = 0;
    [Tooltip("Time till North Weapon Attack Prefab spawn.")]
    public float WeaponAttackNorthPrefabTime = 0;
    [Tooltip("Time till South Weapon Attack Prefab spawn.")]
    public float WeaponAttackSouthPrefabTime = 0;
    [Tooltip("Time till West Weapon Attack Prefab spawn.")]
    public float WeaponAttackWestPrefabTime = 0;

    [Tooltip("Time till first attack ends and attacks reset.")]
    public float First_End_Time = 1f;
    
    [Tooltip("Time till final attack can be used.")]
    public float Second_To_Final = 0.4f;
    
    [Tooltip("Time till second attack ends and attacks reset.")]
    public float Second_End_Time = 1;

    [Tooltip("Time till final attack ends and attacks reset.")]
    public float Final_End_Time = 1.3f;

    [Tooltip("Time till first attack damage.")]
    public float First_Damage_Time = 0.3f;

    [Tooltip("Time till second attack damage.")]
    public float Second_Damage_Time = 0.3f;

    [Tooltip("Time till final attack damage.")]
    public float Final_Damage_Time = 0.3f;

    [Tooltip("Forward movement for the first attack")]
    public float First_Move_Time = 0.3f;
    public float First_Move_Force = 10f;

    [Tooltip("Forward movement for the second attack")]
    public float Second_Move_Time = 0.3f;
    public float Second_Move_Force = 10f;

    [Tooltip("Forward movement for the final attack")]
    public float Final_Move_Time = 0.3f;
    public float Final_Move_Force = 10f;

    // Specials

    //North
    public float North_End_Time = 1f;
    public float North_Damage_Start_Time = 0.3f;
    public float North_Attack_Radius = 1f;
    public float North_Attack_Range = 0;
    public int North_Num_Of_Attacks = 1;
    public float North_Time_Between_Attacks;
    public float North_Move_Time = 0.3f;
    public float North_Move_Force = 15f;
    public Damage North_Special_Damage;


    //South
    public float South_End_Time = 1f;
    public float South_Damage_Start_Time = 0.3f;
    public float South_Attack_Radius = 1f;
    public float South_Attack_Range = 0;
    public int South_Num_Of_Attacks = 1;
    public float South_Time_Between_Attacks;
    public float South_Move_Time = 0.3f;
    public float South_Move_Force = 15f;
    public Damage South_Special_Damage;


    //East
    public float East_End_Time = 1f;
    public float East_Damage_Start_Time = 0.3f;
    public float East_Attack_Radius = 1f;
    public float East_Attack_Range = 0;
    public int East_Num_Of_Attacks = 1;
    public float East_Time_Between_Attacks;
    public float East_Move_Time = 0.3f;
    public float East_Move_Force = 15f;
    public Damage East_Special_Damage;


    //West
    public float West_End_Time = 1f;
    public float West_Damage_Start_Time = 0.3f;
    public float West_Attack_Radius = 1f;
    public float West_Attack_Range = 0;
    public int West_Num_Of_Attacks = 1;
    public float West_Time_Between_Attacks;
    public float West_Move_Time = 0.3f;
    public float West_Move_Force = 15f;
    public Damage West_Special_Damage;
}
