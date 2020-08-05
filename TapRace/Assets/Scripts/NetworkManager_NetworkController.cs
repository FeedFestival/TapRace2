using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManager_NetworkController : NetworkManager
{
    [Header("Scene Properties")]

    [SerializeField]
    public InputField IpInput;

    [SerializeField]
    public InputField PortInput;

    public Text InfoText;
    public GameObject HostGameButton;
    public GameObject AutoJoinButton;
    public GameObject CancelAutoJoinButton;

    [SerializeField]
    public GameObject ConnectionPanel;

    [SerializeField]
    public NetworkDiscovery NetworkDiscovery;

    //

    private Canvas _canvas;

    private IEnumerator _checkForBroadcasts;
    private int _timesChecked;

    private int _dotIndex = 3;

    void Start()
    {
        DontDestroyOnLoad(transform.parent.gameObject);

        if (_canvas == null)
            _canvas = transform.Find("Canvas").gameObject.GetComponent<Canvas>();

        InfoText.text = "";
        //
        IpInput.text = "localhost";
        ResetPort();
        //
        ShowConnectionPanel(true);
    }

    public void StartUpHost()
    {
        SetPort();

        StartCoroutine(StartHostGame());
    }

    public void AutoJoinGame()
    {
        StartCoroutine(JoinHost());
    }

    public void JoinGame()
    {
        StartCoroutine(JoiningGame());
    }

    IEnumerator JoiningGame()
    {
        var ipAddress = IpInput.text;
        var networkPort = Convert.ToInt32(PortInput.text);
        byte error;

        singleton.networkAddress = ipAddress;
        singleton.networkPort = networkPort;

        var networkClient = singleton.StartClient();

        yield return new WaitForSeconds(1f);

        //networkClient.Connect();

        //short s = 1_034;
        //var msg = new RegisterHostMessage();
        //msg.gameName = "s";
        //msg.comment = "test";

        //networkClient.Send(s, msg);

        //Debug.Log("Attempting Connection to: singleton.networkAddress = " + singleton.networkAddress + " , singleton.networkPort " + singleton.networkPort);

        ////
        //NetworkDiscovery.StopBroadcast();
        ////
        //ShowConnectionPanel(false);

        //NetworkTransport.Connect(NetworkDiscovery.hostId, ipAddress, networkPort, 0, out error);

        //Debug.Log(error);
        //Debug.Log("AttemptConnect: " + NetworkDiscovery.hostId + ", " + ipAddress + ", " + networkPort);
    }

    //Detect when a client connects to the Server
    public override void OnClientConnect(NetworkConnection connection)
    {
        Debug.Log("Connected: " + connection.connectionId);
    }

    //Detect when a client connects to the Server
    public override void OnStopClient()
    {
        //Debug.Log("Not Connected: " + connection.connectionId);
    }

    public override void OnClientError(NetworkConnection connection, int errorCode)
    {
        Debug.Log("Not Connected: " + connection.connectionId + ", errorCode: " + errorCode);
    }

    //Detect when a client connects to the Server
    public override void OnClientDisconnect(NetworkConnection connection)
    {
        Debug.Log("Not Connected: " + connection.connectionId);
    }

    public void DisconnectFromGame()
    {
        InfoText.text = "";
        singleton.StopHost();
        //
        ShowConnectionPanel(true);
    }

    public void CancelSearch()
    {
        _timesChecked = 0;
        InfoText.text = "No games nearby.";
        NetworkDiscovery.StopBroadcast();
        StopCoroutine(_checkForBroadcasts);
        ShowConnectionPanel(true);
    }

    public string GetIpAddress()
    {
        return "";
        //return Network.player.ipAddress;
    }

    public void ResetPort()
    {
        PortInput.text = "7777";
    }

    private IEnumerator StartHostGame()
    {
        NetworkDiscovery.Initialize();
        InfoText.text = "Setup.";

        yield return new WaitForSeconds(0.5f);

        NetworkDiscovery.StartAsServer();
        InfoText.text = "Start server..";

        yield return new WaitForSeconds(0.5f);

        NetworkManager.singleton.StartHost();
        //
        //NetworkDiscovery.StopBroadcast();
        //
        //ShowConnectionPanel(false);
    }

    private IEnumerator JoinHost()
    {
        NetworkDiscovery.Initialize();
        ShowConnectionPanel(true, hostGameButton: false, autoJoinButton: false, cancelAutoJoinButton: true);

        InfoText.text = "Setup.";

        yield return new WaitForSeconds(0.5f);

        InfoText.text = "Listening..";

        NetworkDiscovery.StartAsClient();

        _timesChecked = 0;
        _checkForBroadcasts = CheckForBroadcasts();
        StartCoroutine(_checkForBroadcasts);
    }

    private IEnumerator CheckForBroadcasts()
    {
        yield return new WaitForSeconds(1f);

        UpdateSearchingInfo();

        Dictionary<string, NetworkBroadcastResult> broadcastsReceived = NetworkDiscovery.broadcastsReceived;

        if (NetworkDiscovery.broadcastsReceived != null && NetworkDiscovery.broadcastsReceived.Count > 0)
        {
            NetworkBroadcastResult result = default(NetworkBroadcastResult);

            foreach (KeyValuePair<string, NetworkBroadcastResult> pair in broadcastsReceived)
            {
                result = pair.Value;

                Debug.Log(@"
                    key = " + pair.Key + @"
                    result.serverAddress = " + result.serverAddress + @"
                    result.broadcastData = " + BytesToString(result.broadcastData) + @"
                ");
            }

            //string dataString = BytesToString(result.broadcastData);
            //var items = dataString.Split(':');

            string dataString = result.serverAddress;
            var items = dataString.Split(':');

            foreach (string item in items)
            {
                Debug.Log(item);
            }

            //NetworkManager.singleton.networkAddress = items[2];
            //NetworkManager.singleton.networkPort = Convert.ToInt32(7777);
            //NetworkManager.singleton.StartClient();

            IpInput.text = items[3];

            InfoText.text = "Game found !";
            ShowConnectionPanel(true);

            StopCoroutine(_checkForBroadcasts);
        }
        else
        {
            if (_timesChecked > 30)
            {
                CancelSearch();
                yield break;
            }

            _timesChecked++;
            _checkForBroadcasts = CheckForBroadcasts();
            StartCoroutine(_checkForBroadcasts);

            Debug.Log("Checking for games: " + _timesChecked);
        }
    }

    private string BytesToString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    private void UpdateSearchingInfo()
    {
        if (_dotIndex == 3)
        {
            _dotIndex = 0;
            InfoText.text = "Searching...";
        }
        else if (_dotIndex == 2)
        {
            InfoText.text = "Searching..";
        }
        else
        {
            InfoText.text = "Searching.";
        }
        _dotIndex++;
    }

    private void ShowConnectionPanel(
        bool val,
        bool hostGameButton = true,
        bool autoJoinButton = true,
        bool cancelAutoJoinButton = false
        )
    {
        ConnectionPanel.gameObject.SetActive(val);

        HostGameButton.SetActive(hostGameButton);
        AutoJoinButton.SetActive(autoJoinButton);
        CancelAutoJoinButton.SetActive(cancelAutoJoinButton);

        // TODO: Get an OnEnable callback to handle this.
        StartCoroutine(SetCamera());
    }

    IEnumerator SetCamera()
    {
        yield return new WaitForSeconds(0.05f);
        if (_canvas.worldCamera == null)
            _canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void SetNetworkCamera(Camera cam)
    {
        if (_canvas == null)
            _canvas = transform.Find("Canvas").gameObject.GetComponent<Canvas>();
        _canvas.worldCamera = cam;
    }

    private void SetIpAddress()
    {
        NetworkManager.singleton.networkAddress = IpInput.text;

    }

    private void SetPort()
    {
        NetworkManager.singleton.networkPort = Convert.ToInt32(PortInput.text);

    }
}

public class RegisterHostMessage : MessageBase
{
    public string gameName;
    public string comment;
    public bool passwordProtected;
}

public class MasterClient
{
    public NetworkClient client;

    public const short RegisterHostMsgId = 888;

    public void RegisterHost(string name)
    {
        RegisterHostMessage msg = new RegisterHostMessage();
        msg.gameName = name;
        msg.comment = "test";
        msg.passwordProtected = false;

        client.Send(RegisterHostMsgId, msg);
    }
}