using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class GameStart : MonoBehaviour
{
    public static GameStart Instance;

    [Header("���丮���� ���� ��������")]
    [SerializeField, Tooltip("���丮���� ���� �г�")] GameObject tutorialPanel;
    [SerializeField, Tooltip("ü��UI ��ġ�� ���� ����")] GameObject uiHP;
    [SerializeField, Tooltip("����UI ��ġ�� ���� ����")] GameObject uiMP;
    [SerializeField, Tooltip("����UI ��ġ�� ���� ����")] GameObject uiUnit;
    [SerializeField, Tooltip("���׷��̵�UI ��ġ�� ���� ����")] GameObject uiUpgrade;
    [SerializeField, Tooltip("Ÿ�̸�UI ��ġ�� ���� ����")] GameObject uiTimer;

    [Header("���ӽ�ŸƮ�� �ؽ�Ʈ ����� ���� ����")]
    [SerializeField, Tooltip("���� �ؽ�Ʈ")] TMP_Text startText;
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
