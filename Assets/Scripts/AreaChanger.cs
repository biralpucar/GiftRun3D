using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaChanger : MonoBehaviour
{

    public GameObject player;
    public GameObject runArea;
    public GameObject mergeArea;
    public GameObject mergeBtn;
    public Image fade;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement movement))
        {
            SetMergeArea();
        }
    }

    void SetMergeArea()
    {

        //CameraManager.instance.gameCam.m_Priority = 0;
        CameraManager.instance.mergeCam.m_Priority = 90;
        player.SetActive(false);
        runArea.SetActive(false);
        mergeArea.SetActive(true);

        FadeInScreen(1.5f);


    }

    IEnumerator PassNewArea()
    {
        yield return new WaitForSeconds(1.2f);
        mergeBtn.SetActive(true);

    }

    void FadeInScreen(float duration)
    {
        fade.DOFade(0, duration).From(1);
        StartCoroutine(PassNewArea());
    }


}
