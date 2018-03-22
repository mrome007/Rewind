using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindController : MonoBehaviour 
{
	public bool Rewind { get; private set; }
	
	[SerializeField]
	private int MaxNumberOfCommands;
	
	private List<Command> commands;
	
	private int firstCommandIndex;
	private int lastCommandIndex;

	private void Awake()
	{
		Rewind = false;
		commands = new List<Command>();
		Reset();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Return) && !Rewind)
		{
			StartCoroutine(RewindCommands());
		}
	}

	public void AddCommands(Command newCommand)
	{
		if(commands.Count < MaxNumberOfCommands) //List is not full.
		{
			firstCommandIndex = 0;
			lastCommandIndex++;

			commands.Add(newCommand);
		}
		else //List is full.
		{
			firstCommandIndex++;
			firstCommandIndex %= MaxNumberOfCommands;

			lastCommandIndex++;
			lastCommandIndex %= MaxNumberOfCommands;

			commands[lastCommandIndex] = newCommand;
		}
	}

	private IEnumerator RewindCommands()
	{
		Rewind = true;

		while(lastCommandIndex != firstCommandIndex)
		{
			var command = commands[lastCommandIndex];
			command.Execute();

			yield return null;

			lastCommandIndex--;
			if(lastCommandIndex < 0)
			{
				lastCommandIndex += MaxNumberOfCommands;
			}
		}

		Reset();
		commands.Clear();

		yield return new WaitForSeconds(5f);

		Rewind = false;
	}

	private void Reset()
	{
		firstCommandIndex = -1;
		lastCommandIndex = -1;
	}
}
