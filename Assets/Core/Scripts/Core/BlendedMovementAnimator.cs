using UnityEngine;

public class BlendedMovementAnimator : MonoBehaviour
{
    static readonly int motionKey = Animator.StringToHash("Motion");

    public float maxSpeed = 1f;
    public bool useLocalPosition;
    public bool discardVerticalMovement;

    [Header("Debug")]
    [SerializeField] Vector3 lastPos;
    [SerializeField] Animator animator;
    [SerializeField] Transform previousParent;
    [SerializeField] BlendableFloat motion;

    private void Reset()
    {
        motion = new BlendableFloat();
        motion.smoothing = 10;
    }

    private void OnValidate()
    {
        maxSpeed = Mathf.Max(maxSpeed, 0.1f);
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>(true);

        motion.currentValue = 0;
        motion.targetValue = 0;

        lastPos = GetCurrentPos();

        previousParent = transform.parent;
    }

    private void LateUpdate()
    {
        Vector3 currentPos = GetCurrentPos();

        if (useLocalPosition && previousParent != transform.parent)
        {
            Vector3 convertedLastPos = lastPos;
            if (previousParent != null)
            {
                convertedLastPos = previousParent.TransformPoint(convertedLastPos);
            }

            if (transform.parent != null)
            {
                convertedLastPos = transform.parent.InverseTransformPoint(convertedLastPos);
            }

            lastPos = convertedLastPos;
            previousParent = transform.parent;
        }

        Vector3 displacement = lastPos - currentPos;
        if (discardVerticalMovement) displacement.y = 0;

        float diff = displacement.magnitude;
        float dt = Mathf.Max(Time.deltaTime, 0.001f);
        float velocity = diff / dt;

        motion.targetValue = Mathf.Clamp01(velocity / maxSpeed);
        motion.Blend();

        animator.SetFloat(motionKey, motion.currentValue);

        lastPos = currentPos;
    }

    private Vector3 GetCurrentPos()
    {
        Vector3 currentPos = useLocalPosition ? transform.localPosition : transform.position;
        return currentPos;
    }
}
