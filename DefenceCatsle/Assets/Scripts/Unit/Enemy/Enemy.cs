using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("�������� ������")]
    [SerializeField, Tooltip("���������� Ȯ���ϴ� bool��")] protected bool isAttack = false;
    [SerializeField, Tooltip("���� �̵��ӵ�")] float enemySpeed;

    [Header("���� ���ݽ��� ����")]
    [SerializeField, Tooltip("���� ���ݷ�")] float enemyDamage;
    [SerializeField, Tooltip("���� ���ݵ�����")] float enemyAtkCool;
    [SerializeField, Tooltip("���� ü��")] float enemyMaxHp;

    float enemyCurHp = 0.0f;
    bool isAtkCool = false;
    float enemyAtkCoolTimer = 0.0f;

    int playerUnitCheckCount = 0;//�������� ���� �÷��̾� ������ �󸶳� ���Ҵ��� üũ�ϱ����� ����

    EnemyAttack enemyAtk;
    Animator enemyAnim;
    GameManager gameManager;
    DamagePrint dmgPrint;

    private void Awake()
    {
        setDefaultSetting();
    }

    private void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyAtk = GetComponentInChildren<EnemyAttack>();
        gameManager = GameManager.Instance;
        dmgPrint = GetComponent<DamagePrint>();
    }

    private void Update()
    {
        enemyMove(enemySpeed);
        checkTimer();
    }
    private void setDefaultSetting()
    {
        isAttack = false;
        enemyAtkCoolTimer = enemyAtkCool;
        enemyCurHp = enemyMaxHp;
    }


    private void enemyMove(float _speed) //���� ���ݻ������� �ƴ����� �Ǻ��ؼ� �̵��� �ϴ� �Լ�
    {

        if (isAttack == false)
        {
            gameObject.transform.position += Vector3.left * _speed * Time.deltaTime;//���ݻ��°� �ƴϸ� �̵��ӵ���ŭ �������� �̵��ϰ� ����
        }
        if (isAttack == true)
        {
            return;
        }
    }

    public void enemyOnTirrgerEnter(EnemyHitBox.enemyHitBoxType _hitBox, Collider2D _collision)
    {
        if ((_hitBox == EnemyHitBox.enemyHitBoxType.checkPlayer && _collision.tag == "Player") ||
            (_hitBox == EnemyHitBox.enemyHitBoxType.checkPlayer && _collision.tag == "PlayerTower"))
        {
            playerUnitCheckCount++;
            isAttack = true;
            enemyAnim.SetBool("isAttack", true);
        }

        if (_hitBox == EnemyHitBox.enemyHitBoxType.attack && _collision.tag == "Player")
        {
            enemyAtk.enemyUnitAttack(enemyDamage);
            isAtkCool = true;
        }
        else if (_hitBox == EnemyHitBox.enemyHitBoxType.attack && _collision.tag == "PlayerTower")
        {
            enemyAtk.enemyTowerAttack(enemyDamage);
            isAtkCool = true;
        }
    }

    public void enemyOnTirrgerExit(EnemyHitBox.enemyHitBoxType _hitBox, Collider2D _collision)
    {
        if ((_hitBox == EnemyHitBox.enemyHitBoxType.checkPlayer && _collision.tag == "Player") ||
            (_hitBox == EnemyHitBox.enemyHitBoxType.checkPlayer && _collision.tag == "PlayerTower"))
        {
            playerUnitCheckCount--;
            if (playerUnitCheckCount <= 0)
            {
                isAttack = false;
                enemyAnim.SetBool("isAttack", false);
            }
        }
    }

    private void checkTimer()
    {
        if (isAtkCool == true)
        {
            enemyAtkCoolTimer += Time.deltaTime;
            checkAtkCool();
        }
        if (isAtkCool == false)
        {
            return;
        }

    }
    private void checkAtkCool()
    {
        if (isAttack == true && enemyAtkCoolTimer < enemyAtkCool)
        {
            enemyAnim.SetBool("isAttack", false);
        }
        else if (isAttack == true && enemyAtkCoolTimer >= enemyAtkCool)
        {
            enemyAnim.SetBool("isAttack", true);
            enemyAtkCoolTimer = 0.0f;
        }
    }

    public void enemyHit(float _damage)
    {
        if (gameObject.tag == "Enemy")
        {
            damgePrint((int)_damage);
            enemyCurHp -= _damage;
            if (enemyCurHp <= 0)
            {
                enemyCurHp = 0.0f;
                Destroy(gameObject);
            }
        }
    }

    private void damgePrint(int _damage)
    {
        gameManager.createDamagePrint(_damage, transform.position + new Vector3(0, 0.2f, 0), true);
    }
}
