using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MergeManager : MonoSingleton<MergeManager>
{
    public event System.Action<int> OnUpperMergeCompleted;
    public event System.Action<int> OnLowerMergeCompleted;




    public List<LowerItem> AllLowerItem;
    public List<UpperItem> AllUpperItem;
    public void UpperMerged(int index)
    {
        OnUpperMergeCompleted?.Invoke(index);
    }

    public void LowerMerged(int index)
    {

        OnLowerMergeCompleted?.Invoke(index);
    }
    public void UpgradeLowerItem(LowerItem.LowerLevels levels, Vector3 pos)
    {

        switch (levels)
        {
            case LowerItem.LowerLevels.Level0:
                CreateNewLowerItem(AllLowerItem, 0, pos);

                break;
            case LowerItem.LowerLevels.Level1:
                CreateNewLowerItem(AllLowerItem, 1, pos);

                break;
            case LowerItem.LowerLevels.Level2:
                CreateNewLowerItem(AllLowerItem, 2, pos);

                break;
            case LowerItem.LowerLevels.LevelMax:
                ;
                break;
            default:
                break;
        }
    }

    public void UpgradeUpperItem(UpperItem.UpperLevels levels, Vector3 pos)
    {

        switch (levels)
        {
            case UpperItem.UpperLevels.Level0:
                CreateNewUpperItem(AllUpperItem, 0, pos);

                break;
            case UpperItem.UpperLevels.Level1:
                CreateNewUpperItem(AllUpperItem, 1, pos);

                break;
            case UpperItem.UpperLevels.Level2:
                CreateNewUpperItem(AllUpperItem, 2, pos);

                break;
            case UpperItem.UpperLevels.LevelMax:
                
                break;
            default:
                break;
        }





    }

    void CreateNewLowerItem(List<LowerItem> lowerItem, int index, Vector3 pos)
    {
        LowerItem lower = Instantiate(lowerItem[index], pos, Quaternion.identity);

        LowerMerged(index);
      
    }

    void CreateNewUpperItem(List<UpperItem> upperItem, int index, Vector3 pos)
    {
        UpperItem upper = Instantiate(upperItem[index], pos, Quaternion.identity);
        UpperMerged(index);
       
    }
}
