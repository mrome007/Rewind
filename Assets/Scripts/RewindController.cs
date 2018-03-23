using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindController : MonoBehaviour 
{
    private Stack<ICommand> commands;
    private ICommand currentCommand;

    private void Awake()
    {
        commands = new Stack<ICommand>();
        currentCommand = null;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Rewind();
        }
    }

    public void AddCommands(ICommand command)
    {
        commands.Push(command);
    }

    private void Rewind()
    {
        if(commands.Count > 0)
        {
            currentCommand = commands.Pop();
            currentCommand.ExecuteDone += CommandExecuteDone;
            currentCommand.Undo();
        }
    }

    private void CommandExecuteDone(object sender, System.EventArgs e)
    {
        currentCommand.ExecuteDone -= CommandExecuteDone;

        if(commands.Count > 0)
        {
            Rewind();
        }
    }
}
