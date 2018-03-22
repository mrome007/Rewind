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

    private void Start()
    {
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
        StartCoroutine(MoveToTile());
    }

    private IEnumerator MoveToTile()
    {
        if(CurrentTile.CurrentAdjacentTile != null)
        {
            var distance = 0f;
            do
            {
                var direction = CurrentTile.CurrentAdjacentTile.transform.position - CurrentTile.transform.position;
                distance = (CurrentTile.CurrentAdjacentTile.transform.position - transform.position).sqrMagnitude;
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);
                yield return null;
            } while(distance > 0.001);

            CurrentTile = CurrentTile.CurrentAdjacentTile;

            var handler = MoveToTileDone;
            if(handler != null)
            {
                handler(this, new MoveTileEventArgs(CurrentTile != null));
            }
        }
    }

    private void PlayerMovementMoveToTileDone(object sender, MoveTileEventArgs e)
    {
        if(e.HasAdjacent)
        {
            StartCoroutine(MoveToTile());
        }
    }
}
