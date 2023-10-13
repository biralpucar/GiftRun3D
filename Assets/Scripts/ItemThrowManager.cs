using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemThrowManager : MonoSingleton<ItemThrowManager>
{
    public List<Vector3> gridPoints = new List<Vector3>();
    public List<PickableItem> AllItems;
    [SerializeField] int bagCount;
    public Vector3 throwPos;
    int randomPointIndex;
    int randomItemIndex;

    public TextMeshProUGUI itemCount;
    public void GetCellPos(Vector3 points)
    {
        gridPoints.Add(points);
    }

    public void GetColectables()
    {
        if (gridPoints.Count == 0 || bagCount == 0) return;


        bagCount--;
        for (int i = 0; i < 1; i++)
        {
            randomPointIndex = Random.Range(0, gridPoints.Count);
            randomItemIndex = Random.Range(0, AllItems.Count);
            PickableItem mergeItem = Instantiate(AllItems[randomItemIndex], throwPos, Quaternion.identity);
            mergeItem.transform.DOMove(gridPoints[randomPointIndex], 0.5f);
            gridPoints.RemoveAt(randomPointIndex);

        }
      
    }

    public void IncreaseBagCount()
    {
        bagCount++;
    }

    private void Update()
    {
        itemCount.text = bagCount.ToString() + "X";

    }


}
