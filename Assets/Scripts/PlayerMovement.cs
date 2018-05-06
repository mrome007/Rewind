using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    [SerializeField]
    private MoveTile currentTile;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private RewindPlayer rewindController;

    [SerializeField]
    private MoveTile.TileColor playerColor;

    [SerializeField]
    private Transform playerContainer;

    public event EventHandler<MoveTileEventArgs> MoveToTileDone;
    public event EventHandler PlayerWin;
    public bool HasWon { get; private set; }

    private MoveTile adjacentTile;
    private MoveTile previousTile;
    private Coroutine moveToTileRoutine;
    private MoveTile.TileDirection changedCachedDirection;

    private void Start()
    {
        moveToTileRoutine = null;
        previousTile = currentTile;
        changedCachedDirection = MoveTile.TileDirection.None;
    }

    private void Awake()
    {
        MoveToTileDone += PlayerMovementMoveToTileDone;
    }

    private void OnDestroy()
    {
        MoveToTileDone -= PlayerMovementMoveToTileDone;
    }

    public void MovePlayer(MoveTile currTile, MoveTile prevTile, MoveTile currAdjTile, bool loop = true)
    {
        if(moveToTileRoutine != null)
        {
            StopCoroutine(moveToTileRoutine);
            moveToTileRoutine = null;
        }

        HasWon = false;
        moveToTileRoutine = StartCoroutine(MoveToTile(currTile, prevTile, currAdjTile, loop));
    }

    private IEnumerator MoveToTile(MoveTile currTile, MoveTile prevTile, MoveTile currAdjTile, bool loop)
    {
        if(currAdjTile != null)
        {
            var distance = 0f;
            prevTile = currTile;
            currTile = currAdjTile;

            //set private members.
            previousTile = prevTile;
            currentTile = currTile;

            var direction = currTile.transform.position - prevTile.transform.position;
            playerContainer.transform.rotation = Quaternion.LookRotation(direction.normalized);

            do
            {
                distance = (currTile.transform.position - transform.position).sqrMagnitude;
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);
                yield return null;
            } while(distance > 0.05);
        }
            
        var handler = MoveToTileDone;
        if(handler != null)
        {
            handler(this, new MoveTileEventArgs(loop));
        }
    }

    private MoveTile GetAdjacentTile()
    {
        if(currentTile.Mode == MoveTile.TileMode.Changed)
        {
            changedCachedDirection = currentTile.Direction;
        }

        if(currentTile.Mode == MoveTile.TileMode.Forced)
        {
            if(previousTile.Mode == MoveTile.TileMode.Changed)
            {
                currentTile.ModifyAdjacentTile(previousTile, changedCachedDirection);
            }
            else
            {
                currentTile.ModifyAdjacentTile(previousTile);
            }
            changedCachedDirection = MoveTile.TileDirection.None;
        }

        var adjTile = currentTile.NextTile;

        return adjTile;
    }

    public void StartMove()
    {
        var latestCommand = rewindController.GetLatestCommand();

        if(latestCommand != null)
        {
            latestCommand.Execute();
        }
        else
        {
            InitiateMove();
        }
    }

    public void StopMove()
    {
        if(moveToTileRoutine != null)
        {
            StopCoroutine(moveToTileRoutine);
            moveToTileRoutine = null;

            var latestCommand = rewindController.GetLatestCommand();
            if(latestCommand != null)
            {
                latestCommand.Stop();
            }
        }
    }

    private void PlayerMovementMoveToTileDone(object sender, MoveTileEventArgs e)
    {
        moveToTileRoutine = null;

        if(e.Loop)
        {
            if(currentTile.IsWin())
            {
                if(playerColor == currentTile.Color)
                {
                    HasWon = true;

                    var handler = PlayerWin;
                    if(handler != null)
                    {
                        handler(this, null);
                    }
                    return;
                }
            }
            InitiateMove();
        }
    }

    private void InitiateMove()
    {
        var adjacentTile = GetAdjacentTile();
        if(adjacentTile != null)
        {
            var command = new MoveCommand(currentTile, currentTile.PreviousTile, adjacentTile, this);
            rewindController.AddCommands(command);
            command.Execute();
        }
    }
}
