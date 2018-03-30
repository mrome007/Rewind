using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour 
{
    [SerializeField]
    private PlayerMovement playerMovement;

    //With changing directions I need individual blocks.
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
        }
    }
}
