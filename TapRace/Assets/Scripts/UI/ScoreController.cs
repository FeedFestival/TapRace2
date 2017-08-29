using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyMsgTypes
{
    public static short MSG_LOGIN_RESPONSE = 1000;
    public static short MSG_SCORE = 1005;
};

public struct ScoreMessage
{
    public int score;
}

public class ScoreController : NetworkBehaviour
{
    public Score Score1;
    public Score Score2;

    private static ScoreController _scoreController;
    public static ScoreController inst
    {
        get { return _scoreController; }
    }

    void Awake()
    {
        _scoreController = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    [ClientRpc]
    public void RpcIncreaseScore(int playerId)
    {
        Debug.Log("-> CmdIncreaseScore (" + playerId + ")");

        if (playerId == 1)
        {
            Score1.IncreaseScore();
        }
        else
        {
            Score2.IncreaseScore();
        }
    }
}