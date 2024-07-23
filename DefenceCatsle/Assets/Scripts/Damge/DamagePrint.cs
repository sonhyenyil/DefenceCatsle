using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePrint : MonoBehaviour
{

    [Header("������ ��� ����")]
    [SerializeField, Tooltip("������ ��½ð�")] float printTime;
    [SerializeField, Tooltip("������ �ö󰡴� �ӵ�")] float upSpeed;

    TMP_Text text;

    bool setTimer = false;
    float printTimer = 0.0f;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        setDmgTimer();
    }

    public void printDamage(string _damage, bool _isPrint, bool _isEnemy)
    {
        if (_isPrint == true)
        {
            setTimer = true;
        }
        text.SetText(_damage);

        if(_isEnemy == false) 
        {
            text.color = Color.red;
        }
    }

    private void setDmgTimer()
    {
        if (setTimer == true)
        {
            printTimer += Time.deltaTime;

            text.transform.position = transform.transform.position + new Vector3(0, 0 + (Time.deltaTime / printTime /2), 0);
            if (printTimer >= printTime)
            {
                Destroy(gameObject);
                setTimer = false;
                printTimer = 0.0f;
            }
        }
        else
        {
            return;
        }
    }
}
