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

    [Header("Ÿ���� �ʱ� ��ġ��")]
    [SerializeField] Transform towerDefaultTrs;

    [Header("Ÿ���� ���׷��̵�� ����")]
    [SerializeField] float maxTowerHp;
    [SerializeField] float maxTowerMp;
    [SerializeField, Range(0, 3)] int mpUpgradelevel;
    [SerializeField, Range(0, 3)] int unitUpgradelevel;
    [SerializeField, Range(0, 2)] int enemyCurselevel;

    [Header("Ÿ���� ���׷��̵� �ð����� �����ϱ� ���� ������")]
    [SerializeField, Tooltip("0�� 1�ܰ� 1�� 2�ܰ� 2�� 3�ܰ� ���׷��̵带 ���� �ð�")] float[] mpUpgradeTime;
    [SerializeField, Tooltip("0�� 1�ܰ� 1�� 2�ܰ� 2�� 3�ܰ� ���׷��̵带 ���� ���")] float[] mpUpgradeCost;
    [SerializeField, Tooltip("0�� 1�ܰ� 1�� 2�ܰ� 2�� 3�ܰ� ���׷��̸� ���� �ð�")] float[] unitUpgradeTime;
    [SerializeField, Tooltip("0�� 1�ܰ� 1�� 2�ܰ� 2�� 3�ܰ� ���׷��̵带 ���� ���")] float[] unitUpgradeCost;
    [SerializeField, Tooltip("0�� 1�ܰ� 1�� 2�ܰ� 2�� 3�ܰ� ���׷��̸� ���� �ð�")] float[] enemyCurseUpgradeTime;
    [SerializeField, Tooltip("0�� 1�ܰ� 1�� 2�ܰ� 2�� 3�ܰ� ���׷��̵带 ���� ���")] float[] enemyCruseUpgradeCost;
    protected float[] upGradeTimer = { 0f, 0f, 0f };
    /// <summary>
    /// ���׷��̵� üũ�� ���� ������ 0�� mp, 1�� ����, 2�� �� �����̴�.
    /// </summary>
    bool[] upChecker = { false, false, false };

    [Header("Ÿ���� ���ݰ���� ���� ������")]
    protected float curTowerHp;
    protected float curTowerMp;
    float regenTimer;

    [Header("UI ǥ�ø� ���� ����")]
    [SerializeField] TMP_Text hpText;
    [SerializeField] Image hpBar;
    [SerializeField] TMP_Text mpText;
    [SerializeField] Image mpBar;

    [Header("Mp ���׷��̵� �г��� ���� ������")]
    [SerializeField] TMP_Text mpUpText;
    [SerializeField] Image mpUpImage;
    [SerializeField] Image mpUpdisable;

    [Header("Unit ���׷��̵� �г��� ���� ������")]
    [SerializeField] TMP_Text unitUpText;
    [SerializeField] Image unitUpdisable;
    [SerializeField] Image unitUpImage;
    [SerializeField, Tooltip("����Ʈ ���� ����� �ڵ尡 �������� �ǵ帮�� �ʱ�" +
        " 0~3�� ���׷��̵� ������ 4�� ��Ȱ��ȭ�� �̹���")]
    Sprite[] unitUpImageList;

    [Header("Enemy Curse ���׷��̵� �г��� ���� ������")]
    [SerializeField] TMP_Text enemyCurseUpText;
    [SerializeField] Image enemyCurseImage;
    [SerializeField] Image enemyCurseUpdisable;

    [Header("Ÿ�� �ı��ÿ� �ʿ��� ������")]
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
        towerObj.transform.position = towerDefaultTrs.position;//Ÿ�� ������Ʈ�� �ʱ� ��ġ���� ������ ��ǥ�� �̵���Ŵ
    }

    void Update()
    {
        setTowerMp();
        regenMp();
        towerUiSet();
        upgradeUpdate();
    }

    /// <summary>
    /// ���׷��̵� ��ġ�� ���� Ÿ�� �������� ��������
    /// ���� ���� ���׷��̵� ��ġ�� �ٸ��� �ʴٸ� return�ؼ� �ڵ带 ��� ��ȯ����
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

    #region mp�� ���õ��Լ�
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

    #region mp���׷��̵�� ���õ� �Լ�
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
    /// mp ���׷��̵� �̹����� Ŭ���� �ش��Լ��� ȣ��Ǿ� ���׷��̵尡 ����ǰ� ����
    /// </summary>
    public void mpUpgradeEvent()
    {
        curTowerMp -= mpUpgradeCost[mpUpgradelevel];
        upGradeChecker(0);
    }
    #endregion

    #region ���� ���׷��̵带 ���� �ڵ�
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

    #region �� ���� ���׷��̵� �ڵ�
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
    /// ���׷��̵庰 ���׷��̵� �ð��� üũ�ϱ����� �Լ�
    /// ���׷��̵� Ÿ���� 0, 1, 2�� �ְ� 0�� mp, 1�� ����, 2�� �������̴�.
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
    /// Ÿ���� ü�� �� �������� �ǽð����� UI���� �����Ű�� ���� �ڵ�
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
    /// Ÿ���� �ǰݽÿ� �������� �ޱ����� ����
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
    /// Ÿ�� �ı��ÿ� ����Ǵ� �ڵ�
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
