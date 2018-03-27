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

    public event EventHandler<MoveTileEventArgs> MoveToTileDone;

    private MoveTile adjacentTile;
    private MoveTile previousTile;
    private Coroutine moveToTileRoutine;

    private bool direction = true;
    public bool Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;

            var command = new MoveCommand(currentTile, previousTile, previousTile, this);
            rewindController.AddCommands(command);
            command.Execute();
        }
    }

    private void Start()
    {
        moveToTileRoutine = null;
        previousTile = currentTile;
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

            do
            {
                var direction = currTile.transform.position - prevTile.transform.position;
                distance = (currTile.transform.position - transform.position).sqrMagnitude;
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);
                yield return null;
            } while(distance > 0.015);
        }
            
        var handler = MoveToTileDone;
        if(handler != null)
        {
            handler(this, new MoveTileEventArgs(GetAdjacentTile(currTile) != null, loop));
        }
    }

    public MoveTile GetAdjacentTile(MoveTile currTile)
    {
        var adjTile = direction ? currTile.NextTile : currTile.PreviousTile;
        return adjTile;
    }

    public void StartMove()
    {
        var command = new MoveCommand(currentTile, previousTile, GetAdjacentTile(currentTile), this);
        rewindController.AddCommands(command);
        command.Execute();
    }

    private void PlayerMovementMoveToTileDone(object sender, MoveTileEventArgs e)
    {
        moveToTileRoutine = null;

        if(e.Loop)
        {
            if(!e.HasAdjacent)
            {
                Direction = !Direction;
            }
            else
            {
                var command = new MoveCommand(currentTile, previousTile, GetAdjacentTile(currentTile), this);
                rewindController.AddCommands(command);
                command.Execute();
            }
        }
    }
}
