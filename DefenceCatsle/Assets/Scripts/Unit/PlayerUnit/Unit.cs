using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("������ ���ݼ���")]
    [SerializeField, Tooltip("������ �ִ�ü��")] float unitMaxHp;
    [SerializeField, Tooltip("������ ���ݷ�")] float unitAttack;
    [SerializeField, Tooltip("������ ����")] float unitDefence;
    [SerializeField, Tooltip("������ �̵��ӵ�")] float unitSpeed;
    [SerializeField, Tooltip("������ ���ݵ�����")] float unitAtkCool;
    [SerializeField, Tooltip("���� ���׷��̵� ���ݷ� ����(�⺻1)")] float[] upgradeAtk;
    [SerializeField, Tooltip("���� ���׷��̵� ����")] float[] upgradeDef;

    int unitUpgrade = 0;
    protected float curUnitHp = 0.0f;
    float curUnitAttack = 0.0f;
    float curUnitDefence = 0.0f;
    float calDmg = 0.0f;
    float unitAtkCoolTimer = 0.0f;
    int enemyCheckCount = 0;//������������ �������� �󸶳� ���Ҵ��� Ȯ���ϱ� ���� ����

    int unitUplevel = 0;

    Animator unitAnim;
    towerStat towerSt;
    Enemy enemy;

    [SerializeField] bool isAttack = false;
    bool isMove = true;
    bool isAttackCool = false;

    private void Awake()
    {
        setDefaultStat();
        unitAnim = GetComponent<Animator>();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);    
    }

    void Start()
    {
        towerSt = towerStat.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        unitMove();
        activeUnitAtkCool();
        unitUpcheck();
    }

    private void setDefaultStat()
    {
        unitAtkCoolTimer = unitAtkCool;
        curUnitHp = unitMaxHp;
        curUnitAttack = unitAttack;
        curUnitDefence = unitDefence;
    }

    private void unitMove()
    {
        if (isMove == true )
        {
            gameObject.transform.position += Vector3.right.x * Time.deltaTime * new Vector3(unitSpeed, 0, 0);
        }
        else
        {
            return;
        }
    }
    
    public void unitOnTriggerEnter(UnitHitBox.playerUnitHitBoxType _hitBoxType, Collider2D _collsion)
    {
        enemy = _collsion.GetComponent<Enemy>();
        if (_hitBoxType == UnitHitBox.playerUnitHitBoxType.attack && _collsion.tag == "Enemy")
        {
            isAttackCool = true;
            unitAnim.SetBool("isAttackCool", isAttackCool);
            enemy.enemyHit(curUnitAttack);
        }
        if (_hitBoxType == UnitHitBox.playerUnitHitBoxType.checkEnemy && _collsion.tag == "Enemy")
        {
            isMove = false;
            enemyCheckCount++;
            isAttack = true;
            unitAnim.SetBool("isMove", isMove);
            unitAnim.SetBool("isAttack", isAttack);
        }
    }
    public void unitOnTriggerExit(UnitHitBox.playerUnitHitBoxType _hitBoxType, Collider2D _collsion)
    {
        if (_hitBoxType == UnitHitBox.playerUnitHitBoxType.checkEnemy && _collsion.tag == "Enemy")
        {
            enemyCheckCount--;
            if (enemyCheckCount <= 0)
            {
                isMove = true;
                isAttack = false;
                unitAnim.SetBool("isMove", isMove);
                unitAnim.SetBool("isAttack", isAttack);
            }
        }
    }
    public float UnitHit(float _damage)
    {
        calDmg = (_damage - curUnitDefence);
        if (calDmg <= 0)
        {
            calDmg = 1;
        }
        GameManager.Instance.createDamagePrint((int)calDmg, gameObject.transform.position, false);
        if (curUnitHp > 0)
        {
            curUnitHp -= calDmg;
            return curUnitHp;
        }
        else
        {
            curUnitHp = 0;
            Destroy(gameObject);
            return 0;
        }


    }

    private void activeUnitAtkCool()
    {
        if (isAttack == true)
        {
            unitAtkCoolTimer += Time.deltaTime;
            if (unitAtkCoolTimer >= unitAtkCool)
            {
                isAttackCool = false;
                unitAnim.SetBool("isAttackCool", isAttackCool);
                unitAtkCoolTimer = 0.0f;
            }
        }
        else
        {
            return;
        }
    }

    private void unitUpcheck() 
    {
       unitUplevel = towerSt.unitUpgradeLevel();
        switch (unitUplevel) 
        {
            case 1:
                curUnitDefence = unitDefence + upgradeDef[(unitUplevel-1)];
                curUnitAttack = unitAttack * upgradeAtk[(unitUplevel-1)];
                break;
            case 2:
                curUnitDefence = unitDefence + upgradeDef[(unitUplevel-1)];
                curUnitAttack = unitAttack * upgradeAtk[(unitUplevel-1)];
                break;
            case 3:
                curUnitDefence = unitDefence + upgradeDef[(unitUplevel-1)];
                curUnitAttack = unitAttack * upgradeAtk[(unitUplevel-1)];
                break;
        }
    }
}
