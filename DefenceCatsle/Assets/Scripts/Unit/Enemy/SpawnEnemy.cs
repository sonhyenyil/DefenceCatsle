using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Header("스포너의 초기 위치값과 몬스터 종류")]
    [SerializeField] Transform enemyRespawnTrs;
    [SerializeField] GameObject[] enemySpawnList;

    [Header("몬스터 소환 패턴을 위해서 필요한 변수들")]
    [SerializeField, Tooltip("몬스터의 스폰시간 초기값 리스트번호 0:기본슬라임, 1:투척슬라임, 2:방패해골, 3:해골마법사, 4:보스")]
    float[] spawnTime;
    float[] spawnTimer; //몬스터들의 스폰시간을 체크하기위한 변수 0:기본슬라임, 1:투척슬라임, 2:방패해골, 3:해골마법사, 4:보스
    private int spawnCode = 0;
    float valanceTime = 15.0f;
    float valanceTimer = 0.0f;
    int valance = 0;
    int curseCheck = 0;
    float[] cursedelayTime = new float[3];
    float[] cursedebuffval = new float[3];

    enum spawnList
    {
        nomalSlime,
        rangeSlime,
        shiledSkull,
        SkullMage,
        Boss
    }

    //Boss boss; //Boss관련 키워드 보스 구현시 재구축할것
    towerStat towerst;

    private void Awake()
    {
        defaultSetting();
        gameObject.transform.position = enemyRespawnTrs.position;
    }

    private void Start()
    {
        //boss.GetComponent<Boss>();
        towerst = towerStat.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        activeTimer();
    }

    /// <summary>
    /// 몬스터를 생성해주는 함수
    /// </summary>
    private void EnemySpawnPattern()
    {
        //curseCheck = towerst.dubuffEnemy();
        //if (boss.bossSpawn() == true)
        //{

        //}
        //else
        //{
        if (towerst.TowerBreak() == true)
        {
            return;
        }
        GameObject createEnemy = Instantiate(enemySpawnList[spawnCode], enemyRespawnTrs);

        //}
    }

    /// <summary>
    /// 시작시에 타이머와 저주옵션 값들을 지정해주는 함수
    /// </summary>
    private void defaultSetting()
    {
        spawnTimer = new float[spawnTime.Length];
        for (int iNum = 0; iNum < spawnTime.Length; iNum++)
        {
            spawnTimer[iNum] = 0.0f;
        }
        cursedelayTime[0] = 0.0f;
        cursedelayTime[1] = 10.0f;
        cursedelayTime[2] = 15.0f;
        cursedebuffval[0] = 0.0f;
        cursedebuffval[0] = 0.2f;
        cursedebuffval[0] = 0.25f;
    }

    /// <summary>
    /// 각각의 적에대한 타이머를 설정해서 실행해주는 코드
    /// </summary>
    private void activeTimer()
    {
        for (int iNum = 0; iNum < spawnTimer.Length; iNum++)
        {
            spawnTimer[iNum] += Time.deltaTime;
            if (spawnTimer[iNum] >= spawnTime[iNum])
            {
                EnemySpawnPattern();
                spawnTimer[iNum] = 0.0f;
                checkType((spawnList)iNum);
                spawnTime[iNum] = spawnTime[iNum] - (valance * 0.1f);
            }
        }
        //if (boss.bossSpawn() == true)
        //{
        //    return;
        //}
        //else
        //{
        valanceTimer += Time.deltaTime;
        if (valanceTimer >= valanceTime)
        {
            valanceTimer = 0.0f;
            valance++;
        }
        //}

    }

    /// <summary>
    /// 생성될 적 타입을 지정해주는 함수
    /// </summary>
    /// <param name="_enemyName"></param>
    private void checkType(spawnList _enemyName)
    {
        spawnCode = (int)_enemyName;
    }
}
