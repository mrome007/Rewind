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
        Down,
        None
    }

    public enum TileMode
    {
        Forced,
        Changed,
        Win
    }

    public enum TileColor
    {
        None,
        Blue,
        Red
    }

    [SerializeField]
    private TileDirection tileDirection;

    [SerializeField]
    private TileMode tileMode;

    [SerializeField]
    private TileColor tileColor;

    public MoveTile NextTile { get { return nextTile; } }
    public MoveTile PreviousTile { get { return previousTile; } }
    public TileDirection Direction { get{ return tileDirection; } }
    public TileMode Mode { get { return tileMode; } }
    public TileColor Color { get { return tileColor; } }

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
        tileDirection = direction;
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

    public void ModifyAdjacentTile(MoveTile previous, TileDirection dir = TileDirection.None)
    {
        previousTile = previous;
        //Return the same direction as currentTile.

        var prevDirection = dir == TileDirection.None ? previous.Direction : dir;
        switch(prevDirection)
        {
            case TileDirection.Up:
                nextTile = Up;
                tileDirection = TileDirection.Up;
                break;

            case TileDirection.Down:
                nextTile = Down;
                tileDirection = TileDirection.Down;
                break;

            case TileDirection.Left:
                nextTile = Left;
                tileDirection = TileDirection.Left;
                break;

            case TileDirection.Right:
                nextTile = Right;
                tileDirection = TileDirection.Right;
                break;

            default:
                break;
        }

        //No tiles on the same direction.
        if(nextTile == null)
        {
            switch(prevDirection)
            {
                case TileDirection.Up:
                    {
                        nextTile = Up;
                        tileDirection = TileDirection.Up;
                            
                        if(nextTile == null)
                        {
                            nextTile = Right;
                            tileDirection = TileDirection.Right;
                        }

                        if(nextTile == null)
                        {
                            nextTile = Left;
                            tileDirection = TileDirection.Left;
                        }

                        if(nextTile == null)
                        {
                            nextTile = Down;
                            tileDirection = TileDirection.Down;
                        }
                    }
                    break;

                case TileDirection.Down:
                    {
                        nextTile = Down;
                        tileDirection = TileDirection.Down;

                        if(nextTile == null)
                        {
                            nextTile = Left;
                            tileDirection = TileDirection.Left;
                        }

                        if(nextTile == null)
                        {
                            nextTile = Right;
                            tileDirection = TileDirection.Right;
                        }

                        if(nextTile == null)
                        {
                            nextTile = Up;
                            tileDirection = TileDirection.Up;
                        }
                    }
                    break;

                case TileDirection.Left:
                    {
                        nextTile = Left;
                        tileDirection = TileDirection.Left;

                        if(nextTile == null)
                        {
                            nextTile = Down;
                            tileDirection = TileDirection.Down;
                        }

                        if(nextTile == null)
                        {
                            nextTile = Up;
                            tileDirection = TileDirection.Up;
                        }

                        if(nextTile == null)
                        {
                            nextTile = Right;
                            tileDirection = TileDirection.Right;
                        }
                    }
                    break;

                case TileDirection.Right:
                    {
                        nextTile = Right;
                        tileDirection = TileDirection.Right;

                        if(nextTile == null)
                        {
                            nextTile = Up;
                            tileDirection = TileDirection.Up;
                        }

                        if(nextTile == null)
                        {
                            nextTile = Down;
                            tileDirection = TileDirection.Down;
                        }

                        if(nextTile == null)
                        {
                            nextTile = Left;
                            tileDirection = TileDirection.Left;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public bool IsWin()
    {
        return tileMode == TileMode.Win;
    }
}


