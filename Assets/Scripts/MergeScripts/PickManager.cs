using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickManager : MonoSingleton<PickManager>
{
    public LayerMask PickibleLayer;
    public float coolDown;
    public bool isMultiTap;
    [SerializeField] bool itemSelected;
    private void Start()
    {
        InputManager.instance.TouchContinueEvent += OnContinueEvent;
        InputManager.instance.TouchEndEvent += OnEndEvent;
    }

    private void OnEndEvent(InputManager.TouchData data)
    {
        isMultiTap = true;
    }

    private void OnContinueEvent(InputManager.TouchData data)
    {
        RaycastToPick();
    }


    private void Update()
    {
        if (!isMultiTap) { return; }
        if (isMultiTap)
        {
            coolDown -= Time.deltaTime;
            if (coolDown <= 0f)
            {
                coolDown = 0.5f;
                itemSelected = false;
                isMultiTap = false;
            }
        }

    }
    void RaycastToPick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(InputManager.instance.fingerScreenPos);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, PickibleLayer))
        {
            if (hit.collider.TryGetComponent(out PickableItem item))
            {

                if (itemSelected) return;
                itemSelected = true;
                item.GetPicked();
                isMultiTap = false;
            }
        }
    }

}
