using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartScreen : MonoBehaviour 
{
    [SerializeField]
    private Animator animator;

    private void Start()
    {
        animator.Play("Walk");
    }
}
