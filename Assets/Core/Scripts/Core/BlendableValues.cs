using UnityEngine;

[System.Serializable]
public class BlendableFloat : BaseBlendable<float>
{
    protected override void DoBlend(float dt)
    {
        currentValue = NytroUtilities.Decay(currentValue, targetValue, smoothing, dt);
    }
}

[System.Serializable]
public class BlendableVector3 : BaseBlendable<Vector3>
{
    protected override void DoBlend(float dt)
    {
        currentValue = NytroUtilities.Decay(currentValue, targetValue, smoothing, dt);
    }
}

[System.Serializable]
public class BlendableQuaternion : BaseBlendable<Quaternion>
{
    protected override void DoBlend(float dt)
    {
        currentValue = Quaternion.Slerp(currentValue, targetValue, dt * smoothing);
    }
}

public abstract class BaseBlendable<T>
{
    public T currentValue;
    public T targetValue;
    [Range(0, 20)] public float smoothing;
    public bool useUnscaledTime;

    public void Blend(float multiplier = 1)
    {
        if (smoothing < 0f) smoothing = 0f;
        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        dt *= multiplier;
        DoBlend(dt);
    }

    protected abstract void DoBlend(float dt);
}
