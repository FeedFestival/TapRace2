using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text Text;

    public bool Assigned;

    private int _score;

    void Start()
    {
        _score = 0;
    }

    public void IncreaseScore()
    {
        _score++;
        Text.text = _score.ToString();
    }

    public int GetScore()
    {
        return _score;
    }
}
