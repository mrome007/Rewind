using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	private Vector2 moveVector;

	private void Awake()
	{
		moveVector = Vector2.zero;
	}
	
	public void PlayerMove(float x, float y)
	{
		moveVector.x = x;
		moveVector.y = y;
		transform.position = moveVector;
	}
}
