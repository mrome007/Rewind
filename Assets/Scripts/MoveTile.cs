using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour 
{
    [SerializeField]
    private MoveTile nextTile;

    [SerializeField]
    private MoveTile previousTile;

    public MoveTile NextTile { get { return nextTile; } }
    public MoveTile PreviousTile { get { return previousTile; } }
}
