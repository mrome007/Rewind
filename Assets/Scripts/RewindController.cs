using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindController : MonoBehaviour 
{
    [SerializeField]
    private List<RewindPlayer> rewindPlayers;

    private int rewindCount;

    private void Awake()
    {
        rewindCount = 0;
    }

    public void RewindButtonPressed(bool change)
    {
        if(rewindCount < rewindPlayers.Count)
        {
            RewindAllPlayers();
        }
    }

    private void RewindAllPlayers()
    {
        rewindCount = 0;
        foreach(var player in rewindPlayers)
        {
            player.RewindDone += PlayerRewindDone;
            player.Rewind();
        }
    }

    private void PlayerRewindDone(object sender, System.EventArgs e)
    {
        rewindCount++;

        if(rewindCount == rewindPlayers.Count)
        {
            //rewind is done for all players.
            foreach(var player in rewindPlayers)
            {
                player.RewindDone -= PlayerRewindDone;
            }
        }
    }
}
