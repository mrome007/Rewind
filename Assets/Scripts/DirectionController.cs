using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour 
{
    [SerializeField]
    private List<MoveTile> tiles;

    [SerializeField]
    private PlayerMovement playerMovement;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ToggleTileDirection();
            playerMovement.MovePlayer();
        }
    }

    private void ToggleTileDirection()
    {
        foreach(var tile in tiles)
        {
            tile.ToggleDirection();
        }
    }
}
