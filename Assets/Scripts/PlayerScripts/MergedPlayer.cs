using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergedPlayer : MonoBehaviour
{


    [SerializeField] List<GameObject> LowerList;
    [SerializeField] List<GameObject> UpperList;
    private void Start()
    {
        MergeManager.instance.OnUpperMergeCompleted += OnUppderMerged;
        MergeManager.instance.OnLowerMergeCompleted += OnLowerMerged;

    }

    void OnUppderMerged(int index)
    {
        for (int i = 0; i < UpperList.Count; i++)
        {
            UpperList[i].gameObject.SetActive(false);
        }

        UpperList[index + 1].SetActive(true);
    }

    void OnLowerMerged(int index)
    {
        for (int i = 0; i < LowerList.Count; i++)
        {
            LowerList[i].gameObject.SetActive(false);
        }

        LowerList[index + 1].SetActive(true);
    }
}
