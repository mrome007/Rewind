using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	[SerializeField]
	private Player player;

	[SerializeField]
	private float playerSpeed;

	[SerializeField]
	private int MaxNumberOfCommands;

	private List<Command> commands;
	private Vector2 positionVector;

	private int firstCommandIndex;
	private int lastCommandIndex;

	private bool rewind = false;

	private void Awake()
	{
		commands = new List<Command>();
		positionVector = Vector2.zero;

		Reset();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Return) && !rewind)
		{
			StartCoroutine(Rewind());
		}
		
		if(!rewind)
		{
			MovePlayer();
		}
	}

	private void MovePlayer()
	{
		positionVector = player.transform.position;
		var moveTo = playerSpeed * Time.deltaTime * Vector2.right;
		positionVector += moveTo;
		AddMoveCommands(positionVector);
	}

	private void AddMoveCommands(Vector2 movePosition)
	{
		if(commands.Count < MaxNumberOfCommands) //List is not full.
		{
			firstCommandIndex = 0;
			lastCommandIndex++;

			var command = new MoveCommand(movePosition.x, movePosition.y, player);

			commands.Add(command);
			command.Execute();
		}
		else //List is full.
		{
			firstCommandIndex++;
			firstCommandIndex %= MaxNumberOfCommands;

			lastCommandIndex++;
			lastCommandIndex %= MaxNumberOfCommands;

			var command = new MoveCommand(movePosition.x, movePosition.y, player);

			commands[lastCommandIndex] = command;
			command.Execute();
		}
	}

	private IEnumerator Rewind()
	{
		rewind = true;

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

		rewind = false;
	}

	private void Reset()
	{
		firstCommandIndex = -1;
		lastCommandIndex = -1;
	}
}
