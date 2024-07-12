using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public static EndGame Instance;

    [Header("����Ŭ���� �Ǵ� �й�� ����� ������")]
    [SerializeField] GameObject defeatPanel;
    [SerializeField] GameObject clearPanel;
    [SerializeField] TMP_Text curTimeText;
    [SerializeField] TMP_Text clearTimeText;
    private bool clearCheck = false;

    //�ð����� float���·� �����Ͽ� ����ϱ� ���� ������
    private float curTime = 0.0f;
    private float curTimsec = 0.0f;
    private int curTimemin = 0;
    private float clearTime = 0.0f;
    
    //����� �ð��� ���� Ŭ����Ÿ�Ӱ� ����ð��� �ؽ�Ʈ�� ����ϱ����� ����
    private string curTimeStringsec = "";
    private string curTimeStringmin = "";
    private string clearTimeStingsec = "";
    private string clearTimeStingmin = "";


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        calTime();
        clearTimecheck();
    }

    /// <summary>
    /// ���ӿ��� �¸� Ȥ�� �й�ÿ� ����ϴ� �Լ�
    /// </summary>
    /// <param name="_result"></param>
    /// <param name="_isBoss"></param>
    public void gameResult(bool _result, bool _isBoss)
    {
        if (_result == true)
        {
            clearCheck = true;
            clearTime = curTime;
            clearTimeStingsec = (clearTime % 60).ToString("f1");
            clearTimeStingmin = (clearTime / 60).ToString("f0");
        }
        if (_result == true && _isBoss == false)
        {
            defeatPanel.SetActive(true);
        }
        else if (_result == true && _isBoss == true)
        {
            clearPanel.SetActive(true);
            clearTimeText.SetText($"Clear Time : {clearTimeStingmin} min {clearTimeStingsec} sec");
        }
    }
    private void clearTimecheck()
    {
        if (clearCheck == true)
        {
            return;
        }
        else
        {
            curTime += Time.deltaTime;
            curTimeText.SetText($"{curTimeStringmin} : {curTimeStringsec}");
        }
    }

    private void calTime() 
    {
        if (clearCheck == true)
        {
            return;
        }
        else
        {
            curTimsec = curTime % 60;
            curTimemin = (int)curTime / 60;
            curTimeStringsec = curTimsec.ToString("f1");
            curTimeStringmin = curTimemin.ToString("D2");
        }
    }
}
