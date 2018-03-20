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
	private RewindController rewindController;

	private Vector2 positionVector;

	private void Awake()
	{
		positionVector = Vector2.zero;
	}

	private void Update()
	{
		if(!rewindController.Rewind)
		{
			MovePlayer();
		}
	}

	private void MovePlayer()
	{
		positionVector = player.transform.position;
		var moveTo = playerSpeed * Time.deltaTime * Vector2.right;
		positionVector += moveTo;
		var command = new MoveCommand(positionVector.x, positionVector.y, player);
		rewindController.AddCommands(command);
		command.Execute();
	}


}
