using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedOmeter : MonoBehaviour
{
    private RectTransform _parent;
    private RectTransform _rect;

    public GameObject StableIzer;

    public GameObject SpeedMeter;

    public RectTransform Rect_SM;

    private float _animationTime = 2f;
    private float _stableizeTime = 0.5f;

    private float _gainSpeedTime = 0.4f;
    private IEnumerator _rotateStableIzerRoutine;
    private IEnumerator _decreaseSpeedRoutine;

    private Quaternion _originalRotation;
    private Quaternion _originalSpeedMeeterRotation;

    private float _rotValue;

    private float _perUnit;

    // Use this for initialization
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _parent = transform.parent.GetComponent<RectTransform>();

        Rect_SM = SpeedMeter.GetComponent<RectTransform>();

        float sizeY = _rect.sizeDelta.y;
        _rect.sizeDelta = new Vector2(sizeY, sizeY);

        _originalRotation = StableIzer.GetComponent<RectTransform>().rotation;
        _originalSpeedMeeterRotation = SpeedMeter.GetComponent<RectTransform>().rotation;

        _perUnit = 360f / 12f;

        StartStableIzerRotation();
    }

    public void OnAcceleration()
    {
        StartStableIzerRotation(false);

        _rotValue = (360f - StableIzer.transform.eulerAngles.z);
        if (_rotValue > 350f)
        {
            _rotValue = 350f;
        }

        var additionalSpeed = _rotValue / _perUnit;
        var plusSpeed = additionalSpeed * 8;

        StartCoroutine(StableizeStableIzer());
        StartCoroutine(IncreaseSpeed(plusSpeed));
    }

    IEnumerator IncreaseSpeed(float plusSpeed)
    {
        StartDecreaseInSpeed(false);

        var addedRot = new Vector3(0, 0, -plusSpeed);

        var value = Rect_SM.eulerAngles.z + (-plusSpeed);

        // Debug.Log(@"currentRot: " + _rSM.eulerAngles.z + @"
        //             plusSpeed: " + (-plusSpeed) + @"
        //             newValue: " +  + value + @"
        // ");

        var newRot = Rect_SM.eulerAngles + addedRot;

        if (Rect_SM.eulerAngles.z > 0 && value < 0)
        {
            newRot = new Vector3(newRot.x, newRot.y, 0f);
        }

        // Debug.Log(newRot);

        LeanTween.resume(SpeedMeter);
        LeanTween.rotateLocal(SpeedMeter, newRot, _gainSpeedTime).setEaseLinear();

        yield return new WaitForSeconds(_gainSpeedTime);

        StartDecreaseInSpeed();
    }

    private void StartDecreaseInSpeed(bool start = true)
    {
        if (start)
        {
            _decreaseSpeedRoutine = DecreaseInSpeed();
            StartCoroutine(_decreaseSpeedRoutine);
        }
        else
        {
            if (_decreaseSpeedRoutine == null)
                return;

            LeanTween.pause(SpeedMeter);
            LeanTween.cancel(SpeedMeter);
            StopCoroutine(_decreaseSpeedRoutine);
            _decreaseSpeedRoutine = null;
        }
    }

    // Decrease the speed by 10 every 1 second
    IEnumerator DecreaseInSpeed()
    {
        var zValue = Rect_SM.eulerAngles.z;


        if (zValue > 348f)
        {
            Rect_SM.rotation = _originalSpeedMeeterRotation;
            StartDecreaseInSpeed(false);
            yield break;
        }

        var addRot = new Vector3(0, 0, -10f);
        var newRot = Rect_SM.eulerAngles - addRot;

        Debug.Log(newRot);

        LeanTween.rotateLocal(SpeedMeter, newRot, 1f).setEaseLinear();

        yield return new WaitForSeconds(1f);

        StartDecreaseInSpeed();
    }

    IEnumerator RotateStableIzer()
    {
        StableIzer.GetComponent<RectTransform>().rotation = _originalRotation;
        LeanTween.rotateAround(StableIzer, -Vector3.forward, 360f, _animationTime).setEaseInCirc();

        yield return new WaitForSeconds(_animationTime);

        StartStableIzerRotation();
    }

    IEnumerator StableizeStableIzer()
    {
        yield return new WaitForSeconds(0.3f);

        LeanTween.rotateLocal(StableIzer, _originalRotation.eulerAngles, _stableizeTime).setEaseLinear();
        yield return new WaitForSeconds(_stableizeTime);

        StartStableIzerRotation();
    }

    private void StartStableIzerRotation(bool start = true)
    {
        if (start)
        {
            _rotateStableIzerRoutine = RotateStableIzer();
            StartCoroutine(_rotateStableIzerRoutine);
        }
        else
        {
            if (_rotateStableIzerRoutine == null)
                return;

            LeanTween.pause(StableIzer);
            LeanTween.cancel(StableIzer);
            StopCoroutine(_rotateStableIzerRoutine);
            _rotateStableIzerRoutine = null;
        }
    }



    private float GetPercent(float value, float percent)
    {
        return (value / 100f) * percent;
    }

    private float GetValuePercent(float value, float maxValue)
    {
        return (value * 100f) / maxValue;
    }
}