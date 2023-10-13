using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LowerItem : MonoBehaviour
{

    public enum LowerLevels
    {
        Level0, Level1, Level2, LevelMax
    }
    [SerializeField] bool isMergeMax;
    public LowerLevels lowerLevels;
    [SerializeField] PickableItem pickableItem;


    private void Awake()
    {
        pickableItem = GetComponent<PickableItem>();
    }
    private void Start()
    {
        if (lowerLevels == LowerLevels.LevelMax)
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
