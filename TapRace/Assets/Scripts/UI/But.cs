using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class But : NetworkBehaviour
{
    public Text ButtonText;

    [HideInInspector] public Button Button;
    
    public delegate void OnClick();

    public void Init(int id, OnClick onClick)
    {
        Button = GetComponent<Button>();
        Button.GetComponent<Button>().onClick.AddListener(() =>
        {
            onClick();
        });
    }
}
