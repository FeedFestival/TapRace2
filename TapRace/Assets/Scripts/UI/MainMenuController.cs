using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Level
{
    main,           // 0
    Workshop,       // 1
    Race,           // 2
    LocalLeague,    // 3
    lobby,          // 4
    game			// 5
}

public class MainMenuController : MonoBehaviour
{
    public void GoFacility(int level)
    {
        Level levelName = (Level)level;
        SceneManager.LoadScene(levelName.ToString());
    }
}
