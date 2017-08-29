using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoading : MonoBehaviour
{
    private NetworkManager_NetworkController _ntwrkManager;
    public NetworkManager_NetworkController NtwrkManager
    {
        get
        {
			if (_ntwrkManager == null)
				_ntwrkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager_NetworkController>();
            return _ntwrkManager;
        }
    }
    void Awake()
    {
        NtwrkManager.SetNetworkCamera(GetComponent<Camera>());
    }
}
