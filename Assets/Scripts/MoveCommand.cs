using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{   
    public event EventHandler ExecuteDone;
    
    private MoveTile previousTile;
    private PlayerMovement playerMovement;
    
    public MoveCommand(MoveTile prevTile, PlayerMovement plyMov)
    {
        previousTile = prevTile;
        playerMovement = plyMov;
    }
    
    public void Execute()
    {

    }
        
}
