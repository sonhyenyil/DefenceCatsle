using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class SpawnUnit : MonoBehaviour
{
    [Header("유닛 생성을 위한 변수들 0:전사, 1:궁수, 2:마법사, 3:대포병, 4:영웅유닛")]
    [SerializeField, Tooltip("유닛 생성위치")] Transform unitCreateTrs;
    [SerializeField, Tooltip("유닛의 종류")] GameObject[] unitList;
    [SerializeField, Tooltip("유닛의 생성 코스트")] float[] unitCost;
    [SerializeField, Tooltip("유닛의 생성 쿨타임")] float[] unitCool;
    float[] unitCoolTimer;

    [Header("유닛 쿨타임 UI 설정용 이미지")]
    [SerializeField,
        Tooltip("유닛버튼리스트, 0:전사, 1:궁수, 2:마법사, 3:대포병, 4:영웅유닛")]
    Image[] unitButtonList;

    [Header("유닛의 코스트를 UI로 표시하기위한 텍스트")]
    [SerializeField,
        Tooltip("유닛버튼리스트, 0:전사, 1:궁수, 2:마법사, 3:대포병, 4:영웅유닛")]
    TMP_Text[] unitCostText;

    [Header("유닛생성을 막기위한 판넬리스트")]
    [SerializeField,
        Tooltip("판넬리스트, 0:전사, 1:궁수, 2:마법사, 3:대포병, 4:영웅유닛")]
    GameObject[] unitDisPanel;

    [Header("생성되는 유닛 오브젝트를 관리해줄 오브젝트")]
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
