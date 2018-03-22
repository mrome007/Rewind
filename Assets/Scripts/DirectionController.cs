using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour 
{
    [SerializeField]
    private PlayerMovement playerMovement;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.Direction = !playerMovement.Direction;
            playerMovement.MovePlayer();
        }
    }
}
