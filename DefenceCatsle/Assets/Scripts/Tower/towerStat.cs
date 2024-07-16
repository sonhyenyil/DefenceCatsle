using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class towerStat : MonoBehaviour
{
    public static towerStat Instance;

    [Header("타워의 초기 위치값")]
    [SerializeField] Transform towerDefaultTrs;

    [Header("타워의 업그레이드와 스텟")]
    [SerializeField] float maxTowerHp;
    [SerializeField] float maxTowerMp;
    [SerializeField, Range(0, 3)] int mpUpgradelevel;
    [SerializeField, Range(0, 3)] int unitUpgradelevel;
    [SerializeField, Range(0, 2)] int enemyCurselevel;

    [Header("타워의 업그레이드 시간들을 저장하기 위한 변수들")]
    [SerializeField, Tooltip("0은 1단계 1은 2단계 2는 3단계 업그레이드를 위한 시간")] float[] mpUpgradeTime;
    [SerializeField, Tooltip("0은 1단계 1은 2단계 2는 3단계 업그레이드를 위한 비용")] float[] mpUpgradeCost;
    [SerializeField, Tooltip("0은 1단계 1은 2단계 2는 3단계 업그레이를 위한 시간")] float[] unitUpgradeTime;
    [SerializeField, Tooltip("0은 1단계 1은 2단계 2는 3단계 업그레이드를 위한 비용")] float[] unitUpgradeCost;
    [SerializeField, Tooltip("0은 1단계 1은 2단계 2는 3단계 업그레이를 위한 시간")] float[] enemyCurseUpgradeTime;
    [SerializeField, Tooltip("0은 1단계 1은 2단계 2는 3단계 업그레이드를 위한 비용")] float[] enemyCruseUpgradeCost;
    protected float[] upGradeTimer = { 0f, 0f, 0f };
    /// <summary>
    /// 업그레이드 체크를 위한 변수로 0은 mp, 1은 유닛, 2는 적 저주이다.
    /// </summary>
    bool[] upChecker = { false, false, false };

    [Header("타워의 스텟계산을 위한 변수들")]
    protected float curTowerHp;
    protected float curTowerMp;
    float regenTimer;

    [Header("UI 표시를 위한 변수")]
    [SerializeField] TMP_Text hpText;
    [SerializeField] Image hpBar;
    [SerializeField] TMP_Text mpText;
    [SerializeField] Image mpBar;

    [Header("Mp 업그레이드 패널을 위한 데이터")]
    [SerializeField] TMP_Text mpUpText;
    [SerializeField] Image mpUpImage;
    [SerializeField] Image mpUpdisable;

    [Header("Unit 업그레이드 패널을 위한 데이터")]
    [SerializeField] TMP_Text unitUpText;
    [SerializeField] Image unitUpdisable;
    [SerializeField] Image unitUpImage;
    [SerializeField, Tooltip("리스트 순서 변경시 코드가 망가지니 건드리지 않기" +
        " 0~3은 업그레이드 레벨별 4는 비활성화시 이미지")]
    Sprite[] unitUpImageList;

    [Header("Enemy Curse 업그레이드 패널을 위한 데이터")]
    [SerializeField] TMP_Text enemyCurseUpText;
    [SerializeField] Image enemyCurseImage;
    [SerializeField] Image enemyCurseUpdisable;

    [Header("타워 파괴시에 필요한 변수들")]
    [SerializeField] GameObject towerObj;
    bool isDefeat = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        curTowerHp = maxTowerHp;
        resetStat();
    }
    void Start()
    {
        towerObj.transform.position = towerDefaultTrs.position;//타워 오브젝트의 초기 위치값을 지정된 좌표로 이동시킴
    }

    void Update()
    {
        setTowerMp();
        regenMp();
        towerUiSet();
        upgradeUpdate();
    }

    /// <summary>
    /// 업그레이드 수치에 따른 타워 마나량을 설정해줌
    /// 만약 마나 업그레이드 수치가 다르지 않다면 return해서 코드를 즉시 반환해줌
    /// </summary>

    private void resetStat()
    {
        mpUpgradelevel = 0;
        unitUpgradelevel = 0;
        enemyCurselevel = 0;
        curTowerMp = 0.0f;
        curTowerHp = maxTowerHp;
        regenTimer = 0.0f;
    }

    #region mp와 관련된함수
    private void setTowerMp()
    {
        switch (mpUpgradelevel)
        {
            case 0: 
                maxTowerMp = 150;
                break;
            case 1:
                maxTowerMp = 200;
                break;
            case 2:
                maxTowerMp = 300;
                break;
            case 3:
                maxTowerMp = 450;
                break;
        }
    }

    private void regenMp()
    {
        if (isDefeat == true) return;
        if (curTowerMp < maxTowerMp)
        {
            regenMpCal();
        }
        else if (curTowerMp > maxTowerMp)
        {
            curTowerMp = maxTowerMp;
        }
    }

    private void regenMpCal()
    {
        regenTimer += Time.deltaTime;
        if (regenTimer > 1.0f)
        {
            curTowerMp += 6.0f + (mpUpgradelevel * 4);
            regenTimer -= 1.0f;
        }
    }
    #endregion

    #region mp업그레이드와 관련된 함수
    private void mpUpgrade()
    {
        if (mpUpgradelevel == 3)
        {
            mpUpdisable.color = new Color(1, 1, 1, 0.01f);
            return;
        }
        if (curTowerMp < mpUpgradeCost[mpUpgradelevel] || upChecker[0] == true)
        {
            mpUpdisable.gameObject.SetActive(true);
        }
        if (upChecker[0] == true)
        {
            mpUpImage.fillAmount = upGradeTimer[0] / mpUpgradeTime[mpUpgradelevel];
        }
        if (curTowerMp >= mpUpgradeCost[mpUpgradelevel] && upChecker[0] == false)
        {
            mpUpdisable.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// mp 업그레이드 이미지가 클릭시 해당함수가 호출되어 업그레이드가 실행되게 해줌
    /// </summary>
    public void mpUpgradeEvent()
    {
        curTowerMp -= mpUpgradeCost[mpUpgradelevel];
        upGradeChecker(0);
    }
    #endregion

    #region 유닛 업그레이드를 위한 코드
    private void UnitUpgrade()
    {
        if (unitUpgradelevel == 3)
        {
            if (unitUpImage.sprite != unitUpImageList[3])
            {
                unitUpImage.sprite = unitUpImageList[3];
                unitUpdisable.color = new Color(1, 1, 1, 0.01f);
                unitUpdisable.gameObject.SetActive(true);
            }
            return;
        }
        if (curTowerMp < unitUpgradeCost[unitUpgradelevel] || upChecker[1] == true)
        {
            unitUpdisable.gameObject.SetActive(true);
            unitUpImage.sprite = unitUpImageList[unitUpgradelevel];
            if (upChecker[1] == true)
            {
                unitUpImage.fillAmount = upGradeTimer[1] / unitUpgradeTime[unitUpgradelevel];
            }
            return;
        }
        if (curTowerMp >= unitUpgradeCost[unitUpgradelevel] && upChecker[1] == false)
        {
            unitUpdisable.gameObject.SetActive(false);
            unitUpImage.sprite = unitUpImageList[unitUpgradelevel];
        }
    }

    public void unitUpgradeEvent()
    {
        curTowerMp -= unitUpgradeCost[unitUpgradelevel];
        upGradeChecker(1);
    }
    #endregion

    #region 적 저주 업그레이드 코드
    private void enemyCurseUpgeade()
    {
        if (enemyCurselevel == 2)
        {
            enemyCurseUpdisable.color = new Color(1, 1, 1, 0.01f);
            enemyCurseUpdisable.gameObject.SetActive(true);
            return;
        }
        if (curTowerMp < enemyCruseUpgradeCost[enemyCurselevel] || upChecker[2])
        {
            enemyCurseUpdisable.gameObject.SetActive(true);
            if (upChecker[2] == true)
            {
                enemyCurseImage.fillAmount = upGradeTimer[2] / enemyCurseUpgradeTime[enemyCurselevel];
            }
        }
        else if (curTowerMp >= enemyCruseUpgradeCost[enemyCurselevel] && upChecker[2] == false)
        {
            enemyCurseUpdisable.gameObject.SetActive(false);
        }

    }

    public void enemyCurseUpgeadeEvent() 
    {
        curTowerMp -= enemyCruseUpgradeCost[enemyCurselevel];
        upGradeChecker(2);
    }
    #endregion


    /// <summary>
    /// 업그레이드별 업그레이드 시간을 체크하기위한 함수
    /// 업그레이드 타입은 0, 1, 2가 있고 0은 mp, 1은 유닛, 2는 적저주이다.
    /// </summary>
    /// <param name="_upgradeTime"></param>
    /// <param name="_upgradeType"></param>
    /// 
    private void upGradeChecker(int _upgradeType)
    {
        upChecker[_upgradeType] = true;
    }

    private void upTimer()
    {
        for (int iNum = 0; iNum < upChecker.Length; iNum++)
        {
            if (upChecker[iNum] == true)
            {
                upGradeTimer[iNum] += Time.deltaTime;
                upGradeSet();
            }

        }
    }
    private void upGradeSet()
    {
        if (upChecker[0] == true && (upGradeTimer[0] >= mpUpgradeTime[mpUpgradelevel]))
        {
            upChecker[0] = false;
            upGradeTimer[0] = 0.0f;
            mpUpgradelevel++;
            if (mpUpgradelevel >= 3)
            {
                mpUpgradelevel = 3;
            }
        }
        if (upChecker[1] == true && (upGradeTimer[1] >= unitUpgradeTime[unitUpgradelevel]))
        {
            upChecker[1] = false;
            upGradeTimer[1] = 0.0f;
            unitUpgradelevel++;
            if (unitUpgradelevel >= 3)
            {
                unitUpgradelevel = 3;
            }
        }
        if (upChecker[2] == true && (upGradeTimer[2] >= enemyCurseUpgradeTime[enemyCurselevel]))
        {
            upChecker[2] = false;
            upGradeTimer[2] = 0.0f;
            enemyCurselevel++;
            if (enemyCurselevel >= 3)
            {
                enemyCurselevel = 3;
            }
        }
    }

    private void upgradeUpdate() 
    {
        UnitUpgrade();
        mpUpgrade();
        upTimer();
        enemyCurseUpgeade();
    }

    /// <summary>
    /// 타워의 체력 및 마나값을 실시간으로 UI에서 변경시키기 위한 코드
    /// </summary>
    private void towerUiSet()
    {
        hpText.SetText($"{curTowerHp} / {maxTowerHp}");
        hpBar.fillAmount = curTowerHp / maxTowerHp;

        mpText.SetText($"{curTowerMp} / {maxTowerMp}");
        mpBar.fillAmount = curTowerMp / maxTowerMp;

        mpUpText.SetText($"{mpUpgradeCost[mpUpgradelevel]}");
        if (mpUpgradelevel == 3)
        {
            mpUpText.SetText("max");
        }
        unitUpText.SetText($"{unitUpgradeCost[unitUpgradelevel]}");
        if (unitUpgradelevel == 3)
        {
            unitUpText.SetText("max");
        }
        enemyCurseUpText.SetText($"{enemyCruseUpgradeCost[enemyCurselevel]}");
        if (enemyCurselevel == 2)
        {
            enemyCurseUpText.SetText("max");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TowerHit(200);
        }
    }

    /// <summary>
    /// 타워가 피격시에 데미지를 받기위한 공식
    /// </summary>
    /// <param name="_damage"></param>
    public void TowerHit(float _damage)
    {
        curTowerHp -= _damage;
        if (curTowerHp <= 0)
        {
            TowerBreak();
        }
    }

    /// <summary>
    /// 타워 파괴시에 실행되는 코드
    /// </summary>
    private void TowerBreak()
    {
        if (curTowerHp <= 0)
        {
            curTowerHp = 0;
            isDefeat = true;
            EndGame.Instance.gameResult(isDefeat, false);
        }

    }
}
