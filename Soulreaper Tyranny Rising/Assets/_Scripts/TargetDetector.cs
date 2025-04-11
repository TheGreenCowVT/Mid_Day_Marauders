using System.Linq;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _forgetTime; // Used to determine when a target is forgotten
    [SerializeField] private bool _targetInSight;
    [SerializeField] private bool _playerDetected;
    [SerializeField] private bool _inRange;
    [SerializeField] private float _fov;
    [SerializeField] private bool _insideFov;
    [SerializeField] private float _lineOfSightRange;
    [SerializeField] private bool _hasLOS;

    [SerializeField] private float angleToTarget;
    [SerializeField] private Vector3 targetDir;

    public LayerMask lineOfSightMask;

    private float _currentForgetTime;
    private SphereCollider _collider;

    [SerializeField] private string[] validTargets;

    private void Start()
    {
        _targetInSight = false;
        _inRange = false;
        _currentForgetTime = 0;
    }

    private void OnValidate()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        //if(_player == null)
            //_player = GameManager.instance.GetPlayerTransform();

        if (_inRange)
        {
            targetDir = _target.transform.position - transform.position;
            angleToTarget = Vector3.Angle(targetDir, transform.forward);

            Debug.DrawRay(transform.position, targetDir);

            RaycastHit hit;
            if(Physics.Raycast(transform.position, targetDir, out hit))
            {
                if (validTargets.Contains(hit.collider.tag))
                {
                    _hasLOS = true;
                }
                else
                {
                    _hasLOS = false;
                }
            }
            
            if(angleToTarget <= _fov)
            {
                _insideFov = true;
            }
            else
            {
                _insideFov = false;
            }

            _targetInSight = _insideFov | _hasLOS;
        }

        if (!_inRange)
        {
            _currentForgetTime += Time.deltaTime;
            if(_currentForgetTime >= _forgetTime)
            {
                // Forget
                _targetInSight = false;
                _playerDetected = false;
                _target = null;
                _inRange = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (validTargets.Contains(other.tag) && _target == null)
        {
             _target = other.transform;
            _inRange = true;
            if(other.CompareTag("Player")) _playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (validTargets.Contains(other.tag) && _target == other.transform)
        {
            _inRange = false;
        }
    }

    public bool PlayerDetected() => _playerDetected;

    public void SeePlayer()
    {
        // var playerTransform = GameManager.instance.GetPlayerTransform();
        // Set _target = playerTransform
        // set _currentForgetTime = _forgetTime;
        // Set _inRange = true
    }

    public bool HasTarget() => _targetInSight;

    public Transform GetTarget() => _target;

    void OnDrawGizmosSelected()
    {
        float rayRange = _collider.radius;
        float halfFOV = _fov / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
    }
}
