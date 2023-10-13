using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UpperItem : MonoBehaviour
{

    public enum UpperLevels
    {
        Level0, Level1, Level2, LevelMax
    }
    public UpperLevels upperLevels;
    [SerializeField] bool isMergeMax;
    [SerializeField] PickableItem pickableItem;


    private void Awake()
    {
        pickableItem = GetComponent<PickableItem>();
    }
    private void Start()
    {
        if (upperLevels == UpperLevels.LevelMax)
        {
            isMergeMax = true;
        }
    }

    public bool IsMax()
    {
        return isMergeMax;
    }


    public void Pos(Vector3 pos)
    {
        pickableItem.GetInitailizePos(pos);
    }

}
