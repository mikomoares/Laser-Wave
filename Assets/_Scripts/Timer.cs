using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public Text points;
    float time;
    int seconds;
    private GM gm;
    public Text winText;
    private void Start() {
        gm = GetComponent<GM>();
    }

    void Update()
    {
        winText.text = $"in {seconds} seconds";
        if(!gm.winned && !gm.losed){
            time += Time.deltaTime;
            seconds = (int)time;
            points.text = $"{seconds}";
        }
    }
}
