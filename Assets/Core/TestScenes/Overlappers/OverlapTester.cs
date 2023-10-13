using System;
using UnityEngine;
using static NytroPhysicsUtilities;

public class OverlapTester : MonoBehaviour
{
    public Material material;
    public SphereOverlapper sphereOverlapper;
    public BoxOverlapper boxOverlapper;

    private void Update()
    {
        sphereOverlapper.center = transform.position;

        ReadOnlySpan<Collider> overlaps = sphereOverlapper.GetOverlappingColliders();
        for (int i = 0; i < overlaps.Length; i++)
        {
            Collider cldr = overlaps[i];
            Debug.DrawRay(cldr.transform.position, Vector3.up);
        }

        // veya...

        //sphereOverlapper.FindAndProcessOverlappedColliders((Collider collider) =>
        //{
        //    ; //Debug.DrawRay(collider.transform.position, Vector3.up);
        //});
    }

    private void LateUpdate()
    {
        ReadOnlySpan<MeshRenderer> overlappedRenderers = sphereOverlapper.GetOverlappingComponents<MeshRenderer>(false);
        for (int i = 0; i < overlappedRenderers.Length; i++)
        {
            MeshRenderer renderer = overlappedRenderers[i];
            renderer.material = material;
        }

        // veya...

        //sphereOverlapper.FindAndProcessOverlappedComponents<MeshRenderer>((MeshRenderer renderer) =>
        //{
        //    renderer.material = material;
        //}, true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sphereOverlapper.radius);
    }
}
