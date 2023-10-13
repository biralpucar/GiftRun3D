using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeDetector : MonoBehaviour
{
   


    public LayerMask layerMask;
    RaycastHit hit;
    Ray ray;


    public bool isReadyToMerge;

    private void Awake()
    {
      
    }

    private void Update()
    {
        CheckMergeCompatibility();
    }

    void CheckMergeCompatibility()
    {
        ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.TryGetComponent(out GridCell cell))
            {
                //for (int i = 0; i < cell.Items.Count; i++)
                //{
                //    //if (cell.isOccupied && cell.Items[i].TryGetComponent(out Item newItem))
                //    //{
                //    //    if (selectedItem != newItem && selectedItem.level == newItem.level)
                //    //    {
                //    //        isReadyToMerge = true;
                //    //    }

                //    //}

                //    //if (!cell.isOccupied)
                //    //{
                //    //    isReadyToMerge = false;
                //    //}
                //}

            }
        }

    }


}
