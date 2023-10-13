using UnityEngine;

public class GizmoTester : MonoBehaviour
{
    [Header("Parabola")]
    public float height;
    public GizmoMesh gizmoMesh;

    [Header("Circle")]
    public int resolution;
    public float radius;

    private void OnDrawGizmos()
    {
        NytroGizmos.DrawTransformHandles(transform, Vector3.one);

        Gizmos.color = Color.red;

        NytroGizmos.DrawWireCircle(transform.position, transform.rotation, radius: radius, resolution: resolution);

        if (transform.childCount > 1)
        {
            Vector3 start = transform.GetChild(0).position;
            Vector3 end = transform.GetChild(1).position;

            NytroGizmos.DrawParabola(start, end, height);

            NytroGizmos.DrawMesh(gizmoMesh, transform.GetChild(0), Vector3.one);
            NytroGizmos.DrawMesh(gizmoMesh, transform.GetChild(1), Vector3.one);
        }
    }
}
