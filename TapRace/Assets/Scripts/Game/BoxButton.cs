using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BoxButton : NetworkBehaviour
{
    public GameObject gcPrefab;
    public GameObject butR;

    public But Button;

    public int PlayerConId;

    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        var but = (GameObject)GameObject.Instantiate(butR, Vector3.zero, Quaternion.identity);
        but.transform.SetParent(UiController.single.AccelerationPanel.transform, false);
        
        foreach (NetworkConnection connection in NetworkServer.localConnections)
        {
            if (connection.connectionId == connectionToClient.connectionId)
                PlayerConId = connection.connectionId;
        }

        Button = but.GetComponent<But>();
        Button.Init(PlayerConId, OnClick);
    }
    
    public void OnClick()
    {
        CmdIncreaseScore(PlayerConId);
    }

    [Command]
    public void CmdIncreaseScore(int p)
    {
        Debug.Log("-> CmdIncreaseScore");

        ScoreController.inst.RpcIncreaseScore(p);
    }
}
