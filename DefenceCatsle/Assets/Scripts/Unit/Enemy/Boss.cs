using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Unit
{
    public static Boss Instance;
    private bool bossDied = false;//������ �������� �÷��̾��� Ÿ���� �ı��Ȱ����� Ȯ���ϱ����� ���� 
    private float BossHp = 0;
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
        if(Input.GetKeyDown(KeyCode.S))//test ���߿� �����
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
}
