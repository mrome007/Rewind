using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour 
{
    [SerializeField]
    private MoveTile nextTile;

    [SerializeField]
    private MoveTile previousTile;

    [SerializeField]
    private MoveTile Up;

    [SerializeField]
    private MoveTile Down;

    [SerializeField]
    private MoveTile Left;

    [SerializeField]
    private MoveTile Right;

    public enum TileDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public MoveTile NextTile { get { return nextTile; } }
    public MoveTile PreviousTile { get { return previousTile; } }

    public void ChangeNextTile(TileDirection direction)
    {
        switch(direction)
        {
            case TileDirection.Up:
                nextTile = Up;
                break;

            case TileDirection.Down:
                nextTile = Down;
                break;

            case TileDirection.Left:
                nextTile = Left;
                break;

            case TileDirection.Right:
                nextTile = Right;
                break;

            default:
                break;
        }
    }
}


