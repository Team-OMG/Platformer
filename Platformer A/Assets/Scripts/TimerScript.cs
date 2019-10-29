using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    int Time = 0;
    Text txt;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();

        InvokeRepeating("GameTimer", 0f, 1f);
        //Calling This Every Second
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void GameTimer()
    {

        Time++;
        Debug.Log("Game Has Been Running For: " + Time + " Seconds");
        txt.text = "Time: " + Time;

    }
}
