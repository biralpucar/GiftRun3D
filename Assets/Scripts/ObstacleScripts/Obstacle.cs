using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public bool isMove;
    public bool isRotate;

    private void Start()
    {
        if (isMove)
        {
            transform.DOMoveX(2f, 5f).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo);
        }

        if (isRotate)
        {
            transform.DORotate(Vector3.forward * 20f, 7.5f).SetLoops(-1, LoopType.Yoyo);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement movement))
        {
            movement.transform.DOMoveZ(transform.position.z - 3f, 0.5f);
           
        }
    }
}
