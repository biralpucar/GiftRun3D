using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] private bool isPicked;
    [SerializeField] private bool isMergeAvailable;
    Tween pickAnimate;

    [SerializeField] Vector3 mergePos;
    [SerializeField] Vector3 initializePos;
    public float xLimit;
    public float zLimit;


    private void OnEndEvent(InputManager.TouchData data)
    {
        if (isMergeAvailable)
        {
            GetReleased(mergePos);
           

        }
        else
        {
            GetReleased(initializePos);

        }
    }

    private void OnTouchContinue(InputManager.TouchData data)
    {
        if (isPicked)
        {
            Vector3 frameDelta = data.frameDelta;
            frameDelta.z = frameDelta.y;
            frameDelta.y = 0;
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
            pos.z = Mathf.Clamp(transform.position.z, -zLimit, zLimit);
            transform.position = pos;
            transform.position += frameDelta * Time.deltaTime * 740;
        }
    }


    void SubscribeInput()
    {
        InputManager.instance.TouchContinueEvent += OnTouchContinue;
        InputManager.instance.TouchEndEvent += OnEndEvent;
    }

    void UnsubscribeInput()
    {
        InputManager.instance.TouchContinueEvent -= OnTouchContinue;
        InputManager.instance.TouchEndEvent -= OnEndEvent;
    }
    public void GetPicked()
    {
        isPicked = true;
        pickAnimate = transform.DOMoveY(10f, 0.1f);
        SubscribeInput();


    }
    public void GetReleased(Vector3 pos)
    {

        if (isPicked)
        {
            isPicked = false;


        }
        pickAnimate = transform.DOMove(pos, 0.1f);
        UnsubscribeInput();

    }

    public bool MergeAvailable(bool state)
    {
        return isMergeAvailable = state;
    }
    public void GetInitailizePos(Vector3 pos)
    {
        initializePos = pos;
    }

    public void MergeAvailablePosition(Vector3 _mergePos)
    {
        mergePos = _mergePos;

    }
    public bool IsPicked() { return isPicked; }

}
