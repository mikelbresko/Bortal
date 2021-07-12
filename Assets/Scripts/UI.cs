using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    int time;
    int cystalsCollected;

    Text uiBox;

    GameObject gameMaster;
    GameObject player;
    GameObject textUI;

    // Start is called before the first frame update
    void Start()
    {
        this.textUI = GameObject.FindGameObjectWithTag("TextUI");
        this.gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.uiBox = textUI.GetComponent<Text>();
        this.cystalsCollected = gameMaster.GetComponent<GameController>().getLevel();
        this.time = player.GetComponent<PController>().getTime(); 
    }

    // Update is called once per frame
    void Update()
    {
        cystalsCollected = gameMaster.GetComponent<GameController>().getLevel();
        time = player.GetComponent<PController>().getTime();
        
        string timePeriod = getTimePeriod(time);
        setText("Time Period: " + timePeriod + "\n" + "Crystals: " + cystalsCollected.ToString());
    }

    void setText(string s)
    {
        uiBox.text = s;
    }

    private string getTimePeriod(int t)
    {
        string timePeriod = "";
        int time = t;
        if (time == 0)
        {
            timePeriod = "Prehistoric";
        }
        if (time == 1)
        {
            timePeriod = "Medieval";
        }
        if (time == 2)
        {
            timePeriod = "20th Century";
        }
        if (time == 3)
        {
            timePeriod = "Future";
        }
        return timePeriod;
    }
}
