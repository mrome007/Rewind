using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    [SerializeField]
    private MoveTile CurrentTile;

    [SerializeField]
    private float moveSpeed;

    public event EventHandler<MoveTileEventArgs> MoveToTileDone;

    private bool direction;
    public bool Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
            SetAdjacentTile();
        }
    }

    private MoveTile adjacentTile;
    private Coroutine moveToTileRoutine;

    private void Start()
    {
        moveToTileRoutine = null;
        Direction = true;
        MovePlayer();
    }

    private void Awake()
    {
        MoveToTileDone += PlayerMovementMoveToTileDone;
    }

    private void OnDestroy()
    {
        MoveToTileDone -= PlayerMovementMoveToTileDone;
    }

    public void MovePlayer()
    {
        if(moveToTileRoutine == null)
        {
            moveToTileRoutine = StartCoroutine(MoveToTile());
        }
        else
        {
            Debug.Log("wassup");
        }
    }

    private IEnumerator MoveToTile()
    {
        if(adjacentTile != null)
        {
            var distance = 0f;
            do
            {
                var direction = adjacentTile.transform.position - CurrentTile.transform.position;
                distance = (adjacentTile.transform.position - transform.position).sqrMagnitude;
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);
                yield return null;
            } while(distance > 0.001);

            CurrentTile = adjacentTile;
            SetAdjacentTile();
        }

        var handler = MoveToTileDone;
        if(handler != null)
        {
            handler(this, new MoveTileEventArgs(adjacentTile != null));
        }
    }

    private void SetAdjacentTile()
    {
        adjacentTile = direction ? CurrentTile.NextTile : CurrentTile.PreviousTile;
    }

    private void PlayerMovementMoveToTileDone(object sender, MoveTileEventArgs e)
    {
        moveToTileRoutine = null;

        if(e.HasAdjacent)
        {
            MovePlayer();
        }
    }
}
