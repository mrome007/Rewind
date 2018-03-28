using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    event EventHandler ExecuteDone;
    void Execute();
    void Undo();
    void Stop();
}
