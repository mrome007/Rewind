using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileEventArgs : EventArgs
{
    public bool HasAdjacent { get; private set; }

    public MoveTileEventArgs(bool adjacent)
    {
        HasAdjacent = adjacent;
    }
}
