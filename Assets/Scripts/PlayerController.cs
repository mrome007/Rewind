using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	[SerializeField]
	private Player player;

	[SerializeField]
	private float maxSpeed;

	[SerializeField]
	private float acceleration;

	[SerializeField]
	private RewindController rewindController;

	private Vector2 positionVector;
	private float currentSpeed;

	private void Awake()
	{
		positionVector = Vector2.zero;
		currentSpeed = 0f;
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
		currentSpeed = GetSpeed(currentSpeed);
		var moveTo = currentSpeed * Time.deltaTime * Vector2.right;
		positionVector += moveTo;
		var command = new MoveCommand(positionVector.x, positionVector.y, player);
		rewindController.AddCommands(command);
		command.Execute();
	}

	private float GetSpeed(float currentSpeed)
	{
		var resultSpeed = currentSpeed;
		if(currentSpeed < maxSpeed)
		{
			resultSpeed = resultSpeed + acceleration * Time.deltaTime;
		}
		else
		{
			resultSpeed = maxSpeed;
		}

		return resultSpeed;
	}
}
