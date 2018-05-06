using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour 
{
    private void Start()
    {
        Invoke("StartBackTrack", 3f);
    }

    private void StartBackTrack()
    {
        SceneManager.LoadScene(1);
    }
}
