using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private int rewindCount;
    private int winCount;

    private void Awake()
    {
        rewindCount = 0;
        winCount = 0;
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
    }

    private void OnDestroy()
    {
        foreach(var player in players)
        {
            var playerMovement = player.GetComponent<PlayerMovement>();
            if(playerMovement != null)
            {
                playerMovement.PlayerWin -= PlayerMovementPlayerWin;
            }
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 20f, tileLayerMask))
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
            RewindAllPlayers();
        }
        else
        {
            StopAllPlayers();
        }
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

            var rewindPlayer = player.GetComponent<RewindPlayer>();
            if(rewindPlayer != null)
            {
                rewindPlayer.StopRewind();
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
        winCount++;

        if(winCount == players.Count)
        {
            Debug.Log("WINNER");
        }
    }
}
