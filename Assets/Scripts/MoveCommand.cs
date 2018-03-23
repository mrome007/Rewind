using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{   
    public event EventHandler ExecuteDone;
    
    private MoveTile previousTile;
    private MoveTile currentTile;
    private MoveTile adjacentTile;
    private PlayerMovement playerMovement;
    
    public MoveCommand(MoveTile currTile, MoveTile prevTile, MoveTile adjTile, PlayerMovement plyMov)
    {
        currentTile = currTile;
        previousTile = prevTile;
        adjacentTile = adjTile;
        playerMovement = plyMov;
    }
    
    public void Execute()
    {
        playerMovement.MoveToTileDone += PlayerMovementMoveToTileDone;
        playerMovement.MovePlayer(currentTile, previousTile, adjacentTile, true);
    }

    public void Undo()
    {
        playerMovement.MoveToTileDone += PlayerMovementMoveToTileDone;
        playerMovement.MovePlayer(adjacentTile, previousTile, currentTile, false);
    }

    private void PlayerMovementMoveToTileDone (object sender, MoveTileEventArgs e)
    {
        playerMovement.MoveToTileDone -= PlayerMovementMoveToTileDone;

        var handler = ExecuteDone;
        if(handler != null)
        {
            handler(this, null);
        }
    }
        
}
