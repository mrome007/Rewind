using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindPlayer : MonoBehaviour 
{
    public event EventHandler RewindDone;
    private Stack<ICommand> commands;
    private ICommand currentCommand;

    private void Awake()
    {
        commands = new Stack<ICommand>();
        currentCommand = null;
    }

    public void AddCommands(ICommand command)
    {
        commands.Push(command);
    }

    public ICommand GetLatestCommand()
    {
        return commands.Count > 0 ? commands.Peek() : null;
    }

    public void Rewind()
    {
        if(commands.Count > 0)
        {
            currentCommand = commands.Peek();
            currentCommand.ExecuteDone += CommandExecuteDone;
            currentCommand.Undo();
        }
    }

    public void StopRewind()
    {
        if(currentCommand != null)
        {
            currentCommand.ExecuteDone -= CommandExecuteDone;
        }
    }

    private void CommandExecuteDone(object sender, System.EventArgs e)
    {
        currentCommand.ExecuteDone -= CommandExecuteDone;

        if(commands.Count > 0)
        {
            commands.Pop();
            Rewind();
        }
        else
        {
            var handler = RewindDone;
            if(handler != null)
            {
                handler(this, null);
            }
        }
    }
}
