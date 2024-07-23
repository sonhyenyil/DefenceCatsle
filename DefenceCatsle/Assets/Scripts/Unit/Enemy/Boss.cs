using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Unit
{
    public static Boss Instance;
    private bool bossDied = false;//������ �������� �÷��̾��� Ÿ���� �ı��Ȱ����� Ȯ���ϱ����� ���� 
    private bool isBossSpawn = false;//������ �����Ǿ��ִ����� Ȯ���ϱ� ���� ����
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
        bossSpawn();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))//test ���߿� �����
        {
            bossDied = true;
            EndGame.Instance.gameResult(bossDied, true);
        }
    }

    protected void UnitHit(float _damage)
    {
        base.UnitHit(_damage);
        if (curUnitHp <= 0)
        {
            bossDied = true;
        }

        EndGame.Instance.gameResult(bossDied, true);
    }

    public bool bossSpawn()
    {
        if (isBossSpawn == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
