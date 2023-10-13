using System.Collections.Generic;
using UnityEngine;

public enum GizmoMesh
{
    Arrow, Circle, Disk, DottedCircle, Exclamation, HollowPanel, Panel, Prism, Question
}

public static class NytroGizmos
{
    private static Dictionary<GizmoMesh, Mesh> gizmoMeshes = new Dictionary<GizmoMesh, Mesh>();

    private static Mesh GetGizmoMesh(GizmoMesh meshType)
    {
        if (gizmoMeshes == null) gizmoMeshes = new Dictionary<GizmoMesh, Mesh>();

        if (!gizmoMeshes.ContainsKey(meshType))
        {
            string meshPath = "GizmoMesh/" + meshType.ToString();
            Mesh mesh = Resources.Load<Mesh>(meshPath);
            gizmoMeshes[meshType] = mesh;
        }

        return gizmoMeshes[meshType];
    }

    public static void DrawMesh(GizmoMesh meshType, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Mesh mesh = GetGizmoMesh(meshType);
        Gizmos.DrawMesh(mesh, position, rotation, scale);
    }

    public static void DrawMesh(GizmoMesh meshType, Transform transform, Vector3 scale)
    {
        if (transform == null) return;
        DrawMesh(meshType, transform.position, transform.rotation, scale);
    }

    public static void DrawTransformHandles(Transform transform, Vector3 scale)
    {
        if (transform == null) return;

        Color oldColor = Gizmos.color;
        Quaternion rotation = transform.rotation;

        // forward
        Gizmos.color = Color.blue;
        DrawMesh(GizmoMesh.Arrow, transform.position, rotation, scale);

        // up
        Quaternion upwards = Quaternion.AngleAxis(-90, transform.right) * rotation;
        Gizmos.color = Color.green;
        DrawMesh(GizmoMesh.Arrow, transform.position, upwards, scale);

        // right
        Quaternion rightwards = Quaternion.AngleAxis(90, transform.up) * rotation;
        Gizmos.color = Color.red;
        DrawMesh(GizmoMesh.Arrow, transform.position, rightwards, scale);

        Gizmos.color = oldColor; // let's play nice and restore the old gizmo color
    }

    public static void DrawWireCircle(Vector3 position, Quaternion rotation, float radius = 1f, int resolution = 18)
    {
        Vector3 forward = (rotation * (Vector3.forward * radius));
        Vector3 up = rotation * Vector3.up;

        float anglePerDot = 360f / (float)resolution;
        Quaternion rot = Quaternion.AngleAxis(anglePerDot, up);

        for (int i = 0; i < resolution; i++)
        {
            Vector3 newForward = rot * forward;
            Gizmos.DrawLine(forward + position, newForward + position);
            forward = newForward;
        }
    }

    public static void DrawWireCircle(Transform transform, float radius = 1f, int resolution = 18)
    {
        if (transform == null) return;
        DrawWireCircle(transform.position, transform.rotation, radius: radius, resolution: resolution);
    }

    public static void DrawParabola(Vector3 start, Vector3 end, float height, int resolution = 15)
    {
        float increment = 1f / (float)resolution;
        float t = 0;
        Vector3 currentPos = start;
        for (int i = 0; i < resolution; i++)
        {
            Vector3 newPos = NytroUtilities.ParabolicLerp(start, end, height, t);

            Gizmos.DrawLine(currentPos, newPos);

            currentPos = newPos;
            t += increment;
        }

        Gizmos.DrawLine(currentPos, end);
    }

    public static void DrawTransformGizmo(Transform target, float sphereSize = 0.25f, float rayLength = 3)
    {
        if (target != null)
        {
            Gizmos.DrawSphere(target.position, sphereSize);
            Gizmos.DrawRay(target.position, target.forward * rayLength);
        }
    }
}
