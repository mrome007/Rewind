using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour 
{
    public MoveTile CurrentAdjacentTile { get; private set; }

    public MoveTile NextTile;
    public MoveTile PreviousTile;

    private bool direction;

    private void Awake()
    {
        direction = false;
        ToggleDirection();
    }

    public void ToggleDirection()
    {
        direction = !direction;
        CurrentAdjacentTile = direction ? NextTile : PreviousTile;
    }
}
