using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour 
{
    [SerializeField]
    private Animator playerAnimator;

    public void PlayWalk()
    {
        if(playerAnimator != null)
        {
            playerAnimator.Play("Walk");
        }
    }

    public void PlayIdle()
    {
        if(playerAnimator != null)
        {
            playerAnimator.Play("Idle");
        }
    }
}
