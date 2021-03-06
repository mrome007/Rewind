﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour 
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

    [SerializeField]
    private LayerMask tileLayerMask;

    [SerializeField]
    private GameObject rewindEffectObject;

    [SerializeField]
    private AudioSource gameAudio;

    [SerializeField]
    private Text levelText;

    private int rewindCount;
    private static int sceneIndex = 1;

    private void Awake()
    {
        rewindCount = 0;
    }

    private void Start()
    {
        foreach(var player in players)
        {
            var playerMovement = player.GetComponent<PlayerMovement>();
            if(playerMovement != null)
            {
                playerMovement.PlayerWin += PlayerMovementPlayerWin;
            }
        }

        levelText.text = sceneIndex.ToString();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100f, tileLayerMask))
            {
                var moveTile = hit.collider.GetComponent<MoveTile>();
                if(moveTile != null)
                {
                    moveTile.CycleNextTile();
                }
            }
        }
    }

    public void RewindButtonPressed()
    {
        if(rewindToggle.isOn)
        {
            if(!players.Any(player => player.GetComponent<RewindPlayer>().HasCommands()))
            {
                return;
            }
            RewindAllPlayers();
        }
        else
        {
            StopAllPlayers();
        }

        gameAudio.pitch = rewindToggle.isOn ? -1f : 1f;
        rewindEffectObject.SetActive(rewindToggle.isOn);
    }

    public void PlayButtonPressed()
    {
        if(playToggle.isOn)
        {
            StartMovePlayers();
        }
        else
        {
            StopAllPlayers();
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

            var playerAnimation = player.transform.GetChild(0).GetComponent<PlayerAnimation>();
            if(playerAnimation != null)
            {
                playerAnimation.PlayWalk();
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

            var playerAnimation = player.transform.GetChild(0).GetComponent<PlayerAnimation>();
            if(playerAnimation != null)
            {
                playerAnimation.PlayWalk();
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

            var rewindPlayer = player.GetComponent<RewindPlayer>();
            if(rewindPlayer != null)
            {
                rewindPlayer.StopRewind();
            }

            if(playToggle.isOn || rewindToggle.isOn)
            {
                var playerAnimation = player.transform.GetChild(0).GetComponent<PlayerAnimation>();
                if(playerAnimation != null)
                {
                    playerAnimation.PlayWalk();
                }
            }
            else
            {
                var playerAnimation = player.transform.GetChild(0).GetComponent<PlayerAnimation>();
                if(playerAnimation != null)
                {
                    playerAnimation.PlayIdle();
                }
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

    private void PlayerMovementPlayerWin(object sender, System.EventArgs e)
    {
        var won = players.All(player => player.GetComponent<PlayerMovement>().HasWon);
        if(won)
        {
            ChangeLevel();
        }
    }

    private void ChangeLevel()
    {
        sceneIndex++;
        sceneIndex %= SceneManager.sceneCountInBuildSettings;

        SceneManager.LoadScene(sceneIndex);
    }
}
