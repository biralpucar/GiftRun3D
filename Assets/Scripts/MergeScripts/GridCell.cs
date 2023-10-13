using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GridCell : MonoBehaviour
{

    public bool isOccupied;

    [SerializeField] UpperItem upperItem;
    [SerializeField] LowerItem lowerItem;

    [SerializeField] int upperItemCount;
    [SerializeField] int lowerItemCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out UpperItem _upperItem))
        {

            if (upperItem != null)
            {
                MergeManager.instance.UpgradeUpperItem(upperItem.upperLevels, transform.position);
                Destroy(upperItem.gameObject);
                Destroy(_upperItem.gameObject);
                upperItem.transform.DOKill();
                _upperItem.transform.DOKill();
               
            }

            else
            {
                upperItem = _upperItem;
                lowerItem = null;
                upperItem.Pos(GetGridCenter());
            }

            isOccupied = true;

            ItemThrowManager.instance.gridPoints.Remove(transform.position);
        }

        if (other.TryGetComponent(out LowerItem _lowerItem))
        {
            if (lowerItem != null)
            {
                MergeManager.instance.UpgradeLowerItem(lowerItem.lowerLevels, transform.position);
                Destroy(lowerItem.gameObject);
                Destroy(_lowerItem.gameObject);
                lowerItem.transform.DOKill();
                _lowerItem.transform.DOKill();
            }

            else
            {
                lowerItem = _lowerItem;
                upperItem = null;
                lowerItem.Pos(GetGridCenter());
            }
            isOccupied = true;

            ItemThrowManager.instance.gridPoints.Remove(transform.position);
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out UpperItem _upperItem))
        {

            upperItem = null;
            isOccupied = false;
            ItemThrowManager.instance.gridPoints.Add(transform.position);
        }

        if (other.TryGetComponent(out LowerItem _lowerItem))
        {

            lowerItem = null;
            isOccupied = false;
            ItemThrowManager.instance.gridPoints.Add(transform.position);
        }
    }



    public UpperItem GetUpperItem() { return upperItem; }
    public LowerItem GetLowerItem() { return lowerItem; }




    public Vector3 GetGridCenter()
    {
        Vector3 centerPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        return centerPos;
    }
}
