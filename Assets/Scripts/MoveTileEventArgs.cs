using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileEventArgs : EventArgs
{
    public bool Loop { get; private set; }

    public MoveTileEventArgs(bool loop)
    {
        Loop = loop;
    }
}
