using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class SpawnUnit : MonoBehaviour
{
    [Header("���� ������ ���� ������ 0:����, 1:�ü�, 2:������, 3:������, 4:��������")]
    [SerializeField, Tooltip("���� ������ġ")] Transform unitCreateTrs;
    [SerializeField, Tooltip("������ ����")] GameObject[] unitList;
    [SerializeField, Tooltip("������ ���� �ڽ�Ʈ")] float[] unitCost;
    [SerializeField, Tooltip("������ ���� ��Ÿ��")] float[] unitCool;
    float[] unitCoolTimer;

    [Header("���� ��Ÿ�� UI ������ �̹���")]
    [SerializeField,
        Tooltip("���ֹ�ư����Ʈ, 0:����, 1:�ü�, 2:������, 3:������, 4:��������")]
    Image[] unitButtonList;

    [Header("������ �ڽ�Ʈ�� UI�� ǥ���ϱ����� �ؽ�Ʈ")]
    [SerializeField,
        Tooltip("���ֹ�ư����Ʈ, 0:����, 1:�ü�, 2:������, 3:������, 4:��������")]
    TMP_Text[] unitCostText;

    [Header("���ֻ����� �������� �ǳڸ���Ʈ")]
    [SerializeField,
        Tooltip("�ǳڸ���Ʈ, 0:����, 1:�ü�, 2:������, 3:������, 4:��������")]
    GameObject[] unitDisPanel;

    [Header("�����Ǵ� ���� ������Ʈ�� �������� ������Ʈ")]
    [SerializeField] Transform unitSpwanOb;

    towerStat towerSt;
    float curMp = 0.0f;

    private void Awake()
    {
        defaultSetting();
        
    }

    private void Start()
    {
        towerSt = towerStat.Instance;
    }
    void Update()
    {
        checkMp();
        checkUnitCost();
        unitCostSetting();
        activeUnitCoolTime();
        checkTower();
    }

    private void defaultSetting()
    {
        unitCoolTimer = new float[unitCool.Length];
        for (int iNum = 0; iNum < unitCool.Length; iNum++)
        {
            unitCoolTimer[iNum] = 0.0f;
        }
    }


    private void checkMp()
    {
        curMp = towerSt.returnMp();
    }

    public void spawnUnit()
    {
        for (int iNum = 0; iNum < unitList.Length; iNum++)
        {
            GameObject go = Instantiate(unitList[iNum], unitCreateTrs.position, Quaternion.identity, unitSpwanOb);
            go.transform.position = unitCreateTrs.position;
            towerSt.useUnitCost(unitCost[iNum]);
            unitCoolTimer[iNum] = 0.0f;
        }
    }

    private void checkUnitCost()
    {
        for (int iNum = 0; iNum < unitList.Length; iNum++)
        {
            if (curMp < unitCost[iNum] || unitCoolTimer[iNum] < unitCool[iNum])
            {
                unitDisPanel[iNum].SetActive(true);
            }
            else
            {
                unitDisPanel[iNum].SetActive(false);
            }

        }
    }

    public void activeUnitCoolTime()
    {
        for (int iNum = 0; iNum < unitCoolTimer.Length; iNum++)
        {
            if (unitCoolTimer[iNum] >= unitCool[iNum])
            {
                unitButtonList[iNum].fillAmount = 1.0f;
                return;
            }
            else if (unitCoolTimer[iNum] < unitCool[iNum])
            {
                unitButtonList[iNum].fillAmount = (unitCoolTimer[iNum] / unitCool[iNum]);
            }
            unitCoolTimer[iNum] += Time.deltaTime;
        }

    }

    private void checkTower() 
    {
        if (towerSt.TowerBreak() == true)
        {
            Destroy(gameObject);   
        }
    }

    private void unitCostSetting()
    {
        for(int iNum = 0; iNum<unitCost.Length; iNum++) 
        {
            unitCostText[iNum].SetText(unitCost[iNum].ToString());
        }
    }
}
