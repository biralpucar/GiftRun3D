using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] PlayerMovement movement;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Running();
    }

   
    public void Running()
    {
        playerAnimator.SetBool("Run", movement.Tap());
    }
}
