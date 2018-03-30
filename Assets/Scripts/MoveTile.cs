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
        Right,
        Up,
        Left,
        Down
    }

    public enum TileMode
    {
        Forced,
        Changed
    }

    [SerializeField]
    private TileDirection tileDirection;

    [SerializeField]
    private TileMode tileMode;

    public MoveTile NextTile { get { return nextTile; } }
    public MoveTile PreviousTile { get { return previousTile; } }
    public TileDirection Direction { get{ return tileDirection; } }
    public TileMode Mode { get { return tileMode; } }

    private int currentTileIndex;

    private void Start()
    {
        currentTileIndex = (int)tileDirection;
        ChangeNextTile(tileDirection);
    }

    public void CycleNextTile()
    {
        currentTileIndex++;
        currentTileIndex %= 4;

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

    public MoveTile GetAdjacentTile(MoveTile previous)
    {
        MoveTile adjTile = null;
        previousTile = previous;
        //Return the same direction as currentTile.
        switch(previous.Direction)
        {
            case TileDirection.Up:
                adjTile = Up;
                break;

            case TileDirection.Down:
                adjTile = Down;
                break;

            case TileDirection.Left:
                adjTile = Left;
                break;

            case TileDirection.Right:
                adjTile = Right;
                break;

            default:
                break;
        }

        //No tiles on the same direction.
        if(adjTile == null)
        {
            switch(previous.Direction)
            {
                case TileDirection.Up:
                    {
                        adjTile = Up;

                        if(adjTile == null)
                        {
                            adjTile = Right;
                        }

                        if(adjTile == null)
                        {
                            adjTile = Left;
                        }

                        if(adjTile == null)
                        {
                            adjTile = Down;
                        }
                    }
                    break;

                case TileDirection.Down:
                    {
                        adjTile = Down;

                        if(adjTile == null)
                        {
                            adjTile = Left;
                        }

                        if(adjTile == null)
                        {
                            adjTile = Right;
                        }

                        if(adjTile == null)
                        {
                            adjTile = Up;
                        }
                    }
                    break;

                case TileDirection.Left:
                    {
                        adjTile = Left;

                        if(adjTile == null)
                        {
                            adjTile = Down;
                        }

                        if(adjTile == null)
                        {
                            adjTile = Up;
                        }

                        if(adjTile == null)
                        {
                            adjTile = Right;
                        }
                    }
                    break;

                case TileDirection.Right:
                    {
                        adjTile = Right;

                        if(adjTile == null)
                        {
                            adjTile = Up;
                        }

                        if(adjTile == null)
                        {
                            adjTile = Down;
                        }

                        if(adjTile == null)
                        {
                            adjTile = Left;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        return adjTile;
    }
}


