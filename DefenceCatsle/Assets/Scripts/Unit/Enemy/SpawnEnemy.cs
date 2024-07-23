using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Header("�������� �ʱ� ��ġ���� ���� ����")]
    [SerializeField] Transform enemyRespawnTrs;
    [SerializeField] GameObject[] enemySpawnList;

    [Header("���� ��ȯ ������ ���ؼ� �ʿ��� ������")]
    [SerializeField, Tooltip("������ �����ð� �ʱⰪ ����Ʈ��ȣ 0:�⺻������, 1:��ô������, 2:�����ذ�, 3:�ذ񸶹���, 4:����")]
    float[] spawnTime;
    float[] spawnTimer; //���͵��� �����ð��� üũ�ϱ����� ���� 0:�⺻������, 1:��ô������, 2:�����ذ�, 3:�ذ񸶹���, 4:����
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

    //Boss boss; //Boss���� Ű���� ���� ������ �籸���Ұ�
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
    /// ���͸� �������ִ� �Լ�
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
    /// ���۽ÿ� Ÿ�̸ӿ� ���ֿɼ� ������ �������ִ� �Լ�
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
    /// ������ �������� Ÿ�̸Ӹ� �����ؼ� �������ִ� �ڵ�
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
    /// ������ �� Ÿ���� �������ִ� �Լ�
    /// </summary>
    /// <param name="_enemyName"></param>
    private void checkType(spawnList _enemyName)
    {
        spawnCode = (int)_enemyName;
    }
}
