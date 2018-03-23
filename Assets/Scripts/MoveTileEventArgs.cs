using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileEventArgs : EventArgs
{
    public bool HasAdjacent { get; private set; }
    public bool Loop { get; private set; }

    public MoveTileEventArgs(bool adjacent, bool loop)
    {
        HasAdjacent = adjacent;
        Loop = loop;
    }
}
