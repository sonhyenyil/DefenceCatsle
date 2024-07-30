using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class GameStart : MonoBehaviour
{
    public static GameStart Instance;

    [Header("듀토리얼을 위한 변수값들")]
    [SerializeField, Tooltip("듀토리얼을 위한 패널")] GameObject tutorialPanel;
    [SerializeField, Tooltip("체력UI 위치를 위한 변수")] GameObject uiHP;
    [SerializeField, Tooltip("마력UI 위치를 위한 변수")] GameObject uiMP;
    [SerializeField, Tooltip("유닛UI 위치를 위한 변수")] GameObject uiUnit;
    [SerializeField, Tooltip("업그레이드UI 위치를 위한 변수")] GameObject uiUpgrade;
    [SerializeField, Tooltip("타이머UI 위치를 위한 변수")] GameObject uiTimer;

    [Header("게임스타트시 텍스트 출력을 위한 변수")]
    [SerializeField, Tooltip("시작 텍스트")] TMP_Text startText;
    Color startTextColor;
    float textTime = 3.0f;
    float textTimer = 0.0f;
    bool isStart = true;

    private void Awake()
    {
        defaultSetting();
    }
    private void Update()
    {
        timerSetting();
    }

    private void defaultSetting()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        gameObject.SetActive(true);
    }

    public bool tutorialCheck()
    {
        if (isStart == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void timerSetting()
    {
        if (isStart == true)
        {
            if (textTimer < textTime)
            {
                setText();
                textTimer += Time.deltaTime;
            }
            else
            {
                if (textTimer > textTime)
                {
                    startTextColor = Color.black;
                    textTimer = textTime;
                    startText.gameObject.SetActive(false);
                }
                return;
            }
        }
        else
        {
            return;
        }
    }
    private void setText()
    {
        if (textTimer < textTime)
        {
            startTextColor = startText.color;
            startTextColor -= new Color(Time.deltaTime, Time.deltaTime, Time.deltaTime, 0.0f);

            if (textTimer >= (textTime / 2))
            {
                startTextColor.a -= (Time.deltaTime/textTime);
            }
            startText.color = startTextColor;
        }
    }
}
