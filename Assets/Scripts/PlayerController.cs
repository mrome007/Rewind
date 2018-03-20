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
	private float maxJumpSpeed;

	[SerializeField]
	private float jumpAcceleration;

	[SerializeField]
	private RewindController rewindController;

	private Vector2 positionVector;
	private float currentSpeed;
	private float currentJumpSpeed;
	private bool jumping;
	private Coroutine jumpRoutine = null;

	private void Awake()
	{
		positionVector = Vector2.zero;
		currentSpeed = 0f;
		currentJumpSpeed = 0f;
		jumping = false;
	}

	private void Update()
	{
		if(rewindController.Rewind)
		{
			if(jumpRoutine != null)
			{
				StopCoroutine(jumpRoutine);
				jumpRoutine = null;
				jumping = false;
			}
			return;
		}
		
		if(Input.GetKeyDown(KeyCode.Space) && !jumping)
		{
			if(jumpRoutine == null)
			{
				jumpRoutine = StartCoroutine(Jump());
			}
		}

		if(!jumping)
		{
			MovePlayer(Vector2.right);
		}
	}

	private float GetSpeed(float curSpd, float acc, float maxVel)
	{
		var resultSpeed = curSpd;
		if(curSpd < maxVel)
		{
			resultSpeed = resultSpeed + acc * Time.deltaTime;
		}
		else
		{
			resultSpeed = maxVel;
		}

		return resultSpeed;
	}

	private IEnumerator Jump()
	{
		jumping = true;
		var count = 0;
		currentJumpSpeed = 0f;

		while(count < 30)
		{
			MovePlayer(Vector2.one);
			yield return null;
			count++;
		}

		count = 0;
		currentJumpSpeed = 0f;

		while(count < 30)
		{
			MovePlayer(new Vector2(1f, -1f));
			yield return null;
			count++;
		}

		jumping = false;
		jumpRoutine = null;
	}

	private void MovePlayer(Vector2 direction)
	{
		positionVector = player.transform.position;
		currentSpeed = GetSpeed(currentSpeed, acceleration, maxSpeed);
		currentJumpSpeed = GetSpeed(currentJumpSpeed, jumpAcceleration, maxJumpSpeed);
		var moveX = currentSpeed * Time.deltaTime;
		var moveY = currentJumpSpeed * Time.deltaTime;
		var moveTo = direction;
		moveTo.x *= moveX;
		moveTo.y *= moveY;
		positionVector += moveTo;
		var command = new MoveCommand(positionVector.x, positionVector.y, player);
		rewindController.AddCommands(command);
		command.Execute();
	}
}
