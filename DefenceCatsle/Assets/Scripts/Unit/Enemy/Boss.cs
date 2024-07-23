using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Unit
{
    public static Boss Instance;
    private bool bossDied = false;//보스가 죽은건지 플레이어의 타워가 파괴된것인지 확인하기위한 변수 
    private bool isBossSpawn = false;//보스가 생성되어있는지를 확인하기 위한 변수
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
        if (Input.GetKeyDown(KeyCode.S))//test 나중에 지울것
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
