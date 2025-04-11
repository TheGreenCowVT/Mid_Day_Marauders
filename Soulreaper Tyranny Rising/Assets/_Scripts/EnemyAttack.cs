using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] bool rotationLocked;

    public bool GetRotationLocked() => rotationLocked;
}
