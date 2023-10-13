using UnityEngine;

public static class NytroUtilities
{
    static readonly Vector3 redHSV = new Vector3(0, 1, 1);
    static readonly Vector3 greenHSV = new Vector3(120f / 360f, 1, 1);

    public static Color LerpFromRedToGreen(float t)
    {
        Vector3 hsvColor = Vector3.Lerp(redHSV, greenHSV, t);
        return Color.HSVToRGB(hsvColor.x, hsvColor.y, hsvColor.z);
    }

    public static Vector3 ParabolicLerp(Vector3 start, Vector3 end, float height, float t)
    {
        static float ParabolaFunction(float x, float h)
        {
            return (-2 * h) * x * x + ((2 * h) * x);
        }

        Vector3 tPoint = Vector3.Lerp(start, end, t);
        return new Vector3(tPoint.x, ParabolaFunction(t, height) + Mathf.Lerp(start.y, end.y, t), tPoint.z);
    }

    // exponential decay
    // https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/
    public static float Decay(float from, float to, float lambda, float dt)
    {
        if (Mathf.Approximately(from, to)) return to;
        else return Mathf.Lerp(from, to, 1f - Mathf.Exp(-lambda * dt));
    }

    public static Vector3 Decay(Vector3 from, Vector3 to, float lambda, float dt)
    {
        Vector3 decayed = new Vector3(
            Decay(from.x, to.x, lambda, dt),
            Decay(from.y, to.y, lambda, dt),
            Decay(from.z, to.z, lambda, dt)
            );

        return decayed;
    }

    public static Vector3 ClosestPointOnLine(Vector3 lineDir, Vector3 lineOrigin, Vector3 point)
    {
        Vector3 intersect = point - lineOrigin;
        float projectedLenght = Vector3.Dot(intersect, lineDir);

        Vector3 closestPointOnLine = (lineDir * projectedLenght) + lineOrigin;
        return closestPointOnLine;
    }

    public static Quaternion ShortestRotation(Quaternion from, Quaternion to)
    {
        if (Quaternion.Dot(to, from) < 0)
        {
            return to * Quaternion.Inverse(from.ScalarMultiply(-1f));
        }

        else return to * Quaternion.Inverse(from);
    }

    public static Vector3 InsideUnitCircle(float scale = 1f)
    {
        Vector3 pos = Random.insideUnitCircle;

        pos.z = pos.y;
        pos.y = 0;
        pos *= scale;

        return pos;
    }
}
