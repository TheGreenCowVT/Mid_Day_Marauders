using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TargetDetector : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float forgetTime; // Used to determine when a target is forgotten
    [SerializeField] private bool targetInSight;
    [SerializeField] private bool playerDetected;
    [SerializeField] private bool inRange;
    [SerializeField] private float fov;
    [SerializeField] private bool insideFov;
    [SerializeField] private float lineOfSightRange;
    [SerializeField] private bool hasLOS;

    [SerializeField] private float angleToTarget;
    [SerializeField] private Vector3 targetDir;

    public LayerMask lineOfSightMask;

    private float currentForgetTime;
    private SphereCollider collider;

    [SerializeField] private string[] validTargets;

    private void Start()
    {
        targetInSight = false;
        inRange = false;
        currentForgetTime = 0;
    }

    private void OnValidate()
    {
        collider = GetComponent<SphereCollider>();
        collider.enabled = true;
        collider.isTrigger = true;
    }

    private void Update()
    {
        //if(player == null)
            //player = GameManager.instance.GetPlayerTransform();

        if (inRange)
        {
            targetDir = target.transform.position - transform.position;
            angleToTarget = Vector3.Angle(targetDir, transform.forward);

            Debug.DrawRay(transform.position, targetDir);

            RaycastHit hit;
            if(Physics.Raycast(transform.position, targetDir, out hit))
            {
                if (validTargets.Contains(hit.collider.tag))
                {
                    hasLOS = true;
                }
                else
                {
                    hasLOS = false;
                }
            }
            
            if(angleToTarget <= fov)
            {
                insideFov = true;
            }
            else
            {
                insideFov = false;
            }

            targetInSight = insideFov | hasLOS;
        }

        if (!inRange)
        {
            currentForgetTime += Time.deltaTime;
            if(currentForgetTime >= forgetTime)
            {
                // Forget
                targetInSight = false;
                playerDetected = false;
                target = null;
                inRange = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (validTargets.Contains(other.tag) && target == null)
        {
             target = other.transform;
            inRange = true;
            if(other.CompareTag("Player")) playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (validTargets.Contains(other.tag) && target == other.transform)
        {
            inRange = false;
        }
    }

    public bool PlayerDetected() => playerDetected;

    public void SeePlayer()
    {
        // var playerTransform = GameManager.instance.GetPlayerTransform();
        // Set target = playerTransform
        // set currentForgetTime = forgetTime;
        // Set inRange = true
    }

    public bool HasTarget() => targetInSight;

    public Transform GetTarget() => target;

    void OnDrawGizmosSelected()
    {
        float rayRange = collider.radius;
        float halfFOV = fov / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
    }
}
