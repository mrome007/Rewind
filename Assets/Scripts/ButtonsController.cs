﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour 
{
    [SerializeField]
    private ToggleGroup buttonsToggleGroup;

    [SerializeField]
    private Toggle rewindToggle;

    [SerializeField]
    private Toggle playToggle;

    [SerializeField]
    private Toggle stopToggle;

    [SerializeField]
    private List<GameObject> players;

    private int rewindCount;

    private void Awake()
    {
        rewindCount = 0;
    }

    public void RewindButtonPressed()
    {
        if(rewindToggle.isOn)
        {
            RewindAllPlayers();
        }
    }

    public void PlayButtonPressed()
    {
        if(playToggle.isOn)
        {
            StartMovePlayers();
        }
    }

    public void StopButtonPressed()
    {
        if(stopToggle.isOn)
        {
            StopAllPlayers();
        }
    }

    public void StartMovePlayers()
    {
        foreach(var player in players)
        {
            var playerMovement = player.GetComponent<PlayerMovement>();
            if(playerMovement != null)
            {
                playerMovement.StartMove();
            }
        }
    }

    private void RewindAllPlayers()
    {
        rewindCount = 0;
        for(int index = 0; index < players.Count; index++)
        {
            var player = players[index];
            var rewindPlayer = player.GetComponent<RewindPlayer>();
            if(rewindPlayer != null)
            {
                rewindPlayer.RewindDone += PlayerRewindDone;
                rewindPlayer.Rewind();
            }
        }
    }

    private void StopAllPlayers()
    {
        foreach(var player in players)
        {
            var playerMovement = player.GetComponent<PlayerMovement>();
            if(playerMovement != null)
            {
                playerMovement.StopMove();
            }
        }
    }

    private void PlayerRewindDone(object sender, System.EventArgs e)
    {
        rewindCount++;

        if(rewindCount == players.Count)
        {
            //rewind is done for all players.
            for(int index = 0; index < players.Count; index++)
            {
                var player = players[index];
                var rewindPlayer = player.GetComponent<RewindPlayer>();
                if(rewindPlayer != null)
                {
                    rewindPlayer.RewindDone -= PlayerRewindDone;
                }
            }

            buttonsToggleGroup.SetAllTogglesOff();
        }
    }
}
