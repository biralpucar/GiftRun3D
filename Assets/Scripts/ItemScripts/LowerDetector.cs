using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LowerItem;
using static UpperItem;

public class LowerDetector : MonoBehaviour
{

    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform detector;
    [SerializeField] Vector3 lowerHasCellPos;
    [SerializeField] LowerItem pickedLowerItem;
    [SerializeField] PickableItem item;
    RaycastHit hit;
    Ray ray;

    private void Awake()
    {
        pickedLowerItem = GetComponent<LowerItem>();
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
                if (cell.GetLowerItem() == null || pickedLowerItem.IsMax())
                {
                    item.MergeAvailable(false);
                    return;
                }
                if (pickedLowerItem != cell.GetLowerItem() && pickedLowerItem.lowerLevels == cell.GetLowerItem().lowerLevels)
                {
                    item.MergeAvailable(true);
                    item.MergeAvailablePosition(cell.GetGridCenter());
                  
                }

            }
        }

    }

 



}
