using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int currentLevel;
    private string[] Levels = { "FirstLevel", "SecondLevel", "ThirdLevel" };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int getLevel()
    {
        return currentLevel;
    }

    public void NextLevel()
    {
        if (currentLevel < 2)
        {
            SceneManager.LoadScene(Levels[currentLevel + 1]);
        } else
        {
            SceneManager.LoadScene(Levels[currentLevel]);
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(Levels[currentLevel]);
    }
}
