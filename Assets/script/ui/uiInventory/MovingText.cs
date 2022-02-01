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

    private void Start()
    {
        timerText = GameObject.FindGameObjectWithTag("tmpro-running-count").GetComponent<TMP_Text>();
    }
    private void Update()
    {
        Vector2 followPos = Camera.main.WorldToScreenPoint(this.transform.position);
        movingText.gameObject.transform.position = followPos;
        timerText.text = Player.Instance.startTime.ToString();
        if(Settings.readyToPerformRunningMoves == true)
        {
            timerText.text = "";
            timerText.text = "ready to perform running moves";
        }
    }
}
