using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Text introStory;
    public Animator menuAnim;
    private bool storyOver;
    public CanvasGroup menuCanvas;

    public void Start()
    {
        introStory.gameObject.SetActive(false);
        storyOver = false;
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            storyOver = true;
            if (storyOver)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
    public void LoadScene(int i)
    {
        Debug.Log("WTF");
        LoadStory();
    } 
    public void LoadStory()
    {
        GameObject storyCanvas = GameObject.FindGameObjectWithTag("StoryCanvas");
        GameObject mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        Canvas c1 = mainCanvas.GetComponent<Canvas>();
        Canvas c = storyCanvas.GetComponent<Canvas>();
        c1.sortingOrder = 9;
        menuCanvas.alpha = 0;
        menuAnim.SetTrigger("NewGame");
        introStory.gameObject.SetActive(true);
        c.sortingOrder = 10;

    }
}
