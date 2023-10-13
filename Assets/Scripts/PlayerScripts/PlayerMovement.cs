using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] float moveSpeed;
    [SerializeField] float swaySpeed;
    [SerializeField] float xSideLimit;
    [SerializeField] float forwardDamper;


    private float targetX;
    private bool isTouch;
    private bool isAlive;
    private void Awake()
    {
    }


    private void Start()
    {
        InputManager.instance.TouchStartEvent += OnTouchStart;
        InputManager.instance.TouchContinueEvent += OnTouchContinue;
        InputManager.instance.TouchEndEvent += OnTouchEnd;
        isAlive = true;
    }

    private void OnTouchEnd(InputManager.TouchData data)
    {
        isTouch = false;

    }

    private void OnTouchStart(InputManager.TouchData data)
    {
        isTouch = true;
    }

    private void Update()
    {
        Move(isTouch);

       
    }


    private void OnTouchContinue(InputManager.TouchData data)
    {
        float input = data.frameDelta.x * swaySpeed * Time.deltaTime;
        targetX = Mathf.Clamp(targetX + input, -xSideLimit, xSideLimit);

    }

    private void Move(bool state)
    {

        if (state && isAlive)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Lerp(pos.x, targetX, Time.deltaTime * 10);
            pos.z += moveSpeed * Time.deltaTime * forwardDamper;
            transform.position = pos;

        }
    }

    public bool Tap()
    {
        return isTouch;
    }

    
}
