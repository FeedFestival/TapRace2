using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public GameObject GameView;
    public GameObject GameLobbyView;

    public GameObject AccelerationPanel;

    public Text IpAddressText;

    private NetworkManager_NetworkController _ntwrkManager;

    private static UiController _uiController;
    public static UiController single { get { return _uiController; } }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _uiController = this;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        GameView.SetActive(false);
        GameLobbyView.SetActive(true);

        _ntwrkManager = GetComponent<SceneLoading>().NtwrkManager;
        IpAddressText.text = _ntwrkManager.GetIpAddress();
    }

    public void StartGame()
    {

    }

    public void ShareIpAddress()
    {

    }

    public void DisconectGame(){
        _ntwrkManager.DisconnectFromGame();
    }
}
