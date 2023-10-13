using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour
{

    //private StackController stackController;
    //[SerializeField] private float scanRadius;

    //private void Awake()
    //{
    //    stackController = GetComponent<StackController>();
    //}
    //private void FixedUpdate()
    //{
    //    Collider[] hitColliders = Physics.OverlapSphere(transform.position, scanRadius);
    //    foreach (var hitCollider in hitColliders)
    //    {
    //        if (hitCollider.TryGetComponent(out Collectable collectable))
    //        {
    //            if (stackController.Push(collectable.transform))
    //            {
    //                collectable.Collected(true, stackController.stackRoot);
                   
    //            }


    //        }
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, scanRadius);
    //    Gizmos.color = Color.yellow;
    //}
}
