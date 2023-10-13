using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;

    [Header("Debug")]
    [SerializeField] private Vector3 targetInput;
    [SerializeField] private Vector3 smoothInput;

    private void Start()
    {
        InputManager.instance.TouchContinueEvent += OnTouchContinue;
        InputManager.instance.TouchEndEvent += OnTouchEnd;
    }

    private void LateUpdate()
    {
        // low-pass filter
        smoothInput = NytroUtilities.Decay(smoothInput, targetInput, 10, Time.deltaTime);

        if (smoothInput.magnitude < 0.001f) return;

        Quaternion rotation = Quaternion.LookRotation(smoothInput, Vector3.up);
        rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        Vector3 position = transform.position;
        position += transform.forward * (Time.deltaTime * movementSpeed * smoothInput.magnitude);
        position.x = Mathf.Clamp(position.x, -4, 4);
        position.z = Mathf.Clamp(position.z, -4, 4);

        transform.SetPositionAndRotation(position, rotation);
    }

    private void OnTouchContinue(InputManager.TouchData data)
    {
        Vector3 input = data.absoluteDelta;

        input = Vector3.ClampMagnitude(input, 0.1f) / 0.1f;
        input.z = input.y;
        input.y = 0;

        targetInput = input;
    }

    private void OnTouchEnd(InputManager.TouchData data)
    {
        targetInput = Vector3.zero;
    }
}
