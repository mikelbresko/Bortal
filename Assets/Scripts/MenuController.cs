using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    GameObject mainCanvas;
    Canvas c;
    // Start is called before the first frame update
    void Start()
    {
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        this.c = mainCanvas.GetComponent<Canvas>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HowToPlay()
    {
       c.sortingOrder = 2;
    }

    public void BackToMainMenu()
    {
        c.sortingOrder = 4;
    }

    public void Exit()
    {
        Debug.Log("SHOULD EXIT");
        Application.Quit();
    }
}
