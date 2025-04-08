using UnityEngine;

public class SphereGizmo : MonoBehaviour
{
    public Color color = Color.white;
    public float radius = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
