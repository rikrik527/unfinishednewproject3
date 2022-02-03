using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yushan.movement;
using TMPro;
using UnityEngine.UI;
using Yushan.DarkType;
public class MovingText : MonoBehaviour
{
    public GameObject movingText;

    public TMP_Text timerText;
    public TMP_Text readyText;
    private void Start()
    {
        timerText = GameObject.FindGameObjectWithTag("tmpro-running-count").GetComponent<TMP_Text>();
        readyText = GameObject.FindGameObjectWithTag("tmpro-ready").GetComponent<TMP_Text>();
    }
    private void Update()
    {
        Vector2 followPos = Camera.main.WorldToScreenPoint(this.transform.position);
        movingText.gameObject.transform.position = followPos;
        timerText.text = Player.Instance.startTime.ToString();
        if(Settings.readyToPerformRunningMoves == true)
        {
            timerText.text = "";
            timerText.text = Player.Instance.startTime.ToString();
            readyText.text = "";
            readyText.text = "ready to perform running moves";
        }else if(Settings.readyToPerformRunningMoves == false)
        {
            timerText.text = "";
            timerText.text = Player.Instance.startTime.ToString();
            readyText.text = "";
            readyText.text = "not ready";
        }
    }
}
