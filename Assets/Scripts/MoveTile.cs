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
        Normal,
        Right,
        Up,
        Left,
        Down
    }

    [SerializeField]
    private TileDirection tileDirection;

    public MoveTile NextTile { get { return nextTile; } }
    public MoveTile PreviousTile { get { return previousTile; } }
    public TileDirection Direction { get{ return tileDirection; } }

    private int currentTileIndex;

    private void Start()
    {
        currentTileIndex = 0;
        if(tileDirection != TileDirection.Normal)
        {
            currentTileIndex = (int)tileDirection;
            ChangeNextTile(tileDirection);
        }
    }

    public void CycleNextTile()
    {
        currentTileIndex++;
        currentTileIndex %= 5;

        if(currentTileIndex == 0)
        {
            currentTileIndex++;
        }

        tileDirection = (TileDirection)currentTileIndex;
        ChangeNextTile(tileDirection);
    }

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

        RotateTile(direction);
    }

    private void RotateTile(TileDirection direction)
    {
        var rotationVector = Vector3.zero;

        switch(direction)
        {
            case TileDirection.Up:
                rotationVector.y = 270f;
                break;

            case TileDirection.Down:
                rotationVector.y = 90f;
                break;

            case TileDirection.Left:
                rotationVector.y = 180f;
                break;

            case TileDirection.Right:
                rotationVector.y = 0f;
                break;
            default:
                break;
        }

        transform.localRotation = Quaternion.Euler(rotationVector);
    }
}


