using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceUi : MonoBehaviour
{

    public GameObject RaceView;
    public GameObject RaceLobbyView;

    // Use this for initialization
    void Start()
    {
        // RaceView.SetActive(false);
        // RaceLobbyView.SetActive(true);
    }

    public void StartGame()
    {
		RaceView.SetActive(false);
        RaceLobbyView.SetActive(true);
    }
}
