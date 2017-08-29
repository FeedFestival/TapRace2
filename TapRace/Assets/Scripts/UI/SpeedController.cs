using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    public Text SpeedScoreText;

	public SpeedOmeter SpeedOmeter;

    private int _speed;

    void Start()
    {
        _speed = 0;
        SpeedScoreText.text = _speed.ToString();

		// SpeedOmeter
    }

    void Update()
    {
        var reverseCount = 360 - System.Convert.ToInt32(SpeedOmeter.Rect_SM.eulerAngles.z);
        var speedValue = reverseCount / 28;
        SpeedScoreText.text =  speedValue.ToString();
    }
}
