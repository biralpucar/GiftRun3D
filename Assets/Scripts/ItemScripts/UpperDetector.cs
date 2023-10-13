using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpperItem;

public class UpperDetector : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform detector;
    [SerializeField] Vector3 upperHasCellPos;
    [SerializeField] UpperItem pickedUpperItem;
    [SerializeField] PickableItem item;
    RaycastHit hit;
    Ray ray;


    private void Awake()
    {
        pickedUpperItem = GetComponent<UpperItem>();
        item = GetComponent<PickableItem>();
    }

    private void Update()
    {
        CheckMergeCompatibility();
    }

    void CheckMergeCompatibility()
    {
        if (!item.IsPicked()) { return; }
        ray = new Ray(detector.position, -transform.up);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.TryGetComponent(out GridCell cell))
            {
                if (cell.GetUpperItem() == null || pickedUpperItem.IsMax())
                {
                    item.MergeAvailable(false);
                    return;
                }

                if (pickedUpperItem != cell.GetUpperItem() && pickedUpperItem.upperLevels == cell.GetUpperItem().upperLevels)
                {

                    item.MergeAvailable(true);
                    item.MergeAvailablePosition(cell.GetGridCenter());


                }

            }
        }

    }




}
