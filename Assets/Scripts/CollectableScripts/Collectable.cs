using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    private void Update()
    {
        transform.Rotate(0f,20f * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement movement))
        {
            ItemThrowManager.instance.IncreaseBagCount();
            Destroy(gameObject, 0.1f);
        }
    }
}
