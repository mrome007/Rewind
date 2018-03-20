using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command
{
	private Player player;
	private float xPosition;
	private float yPosition;

	public MoveCommand(float x, float y, Player p)
	{
		player = p;
		xPosition = x;
		yPosition = y;
	}
	
	public override void Execute()
	{
		player.PlayerMove(xPosition, yPosition);
	}
}
