using UnityEngine;

public class LineGizmo : MonoBehaviour
{
    public Color color = Color.white;
    public float length;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawRay(transform.position, transform.forward * length);
    }
}
